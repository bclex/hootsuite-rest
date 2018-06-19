using Hootsuite.Require;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Hootsuite
{
    internal class Retry
    {
        const int DefaultRetryCount = 5;
        const int DefaultInitialDelay = 1000; // 1 second
        const int DefaultMaxDelay = 20000;
        readonly Action<string> _log = util.logger;
        readonly dynamic _options;
        readonly Backoff _newBackoff;

        public Retry(dynamic options)
        {
            _options = options ?? new { };
            _newBackoff = NewBackoff();
        }

        int NumRetries => dyn.getProp(_options, "maxRetries", DefaultRetryCount);
        int InitialDelay => dyn.getProp(_options, "initialDelay", DefaultInitialDelay);
        int MaxDelay => dyn.getProp(_options, "maxDelay", DefaultMaxDelay);

        Exception _lastError;
        internal Task<JObject> start(Func<bool, Task<JObject>> fetchFn)
        {
            var promise = new TaskCompletionSource<JObject>();
            var forceOAuth = false;
            executeFn();
            _newBackoff.OnReady = (number, delay) => { _log($"Requesting for url {{ number: {number}, delay: {delay} }}"); executeFn(); };
            _newBackoff.OnFail = (err) => throw _lastError;
            async void executeFn()
            {
                try
                {
                    var result = await fetchFn(forceOAuth);
                    promise.SetResult(result);
                }
                catch (Exception err)
                {
                    _lastError = err;
                    if (RetryableError(err))
                    {
                        if (errors.isRateLimited(err)) await Task.Delay(MaxDelay);
                        else if (errors.isExpiredToken(err)) forceOAuth = true;
                        await _newBackoff.DoBackoff();
                        return;
                    }
                    var e = (err as RestlerOperationException);
                    // Previous error structure
                    if (e != null && e.Content != null && e.Content is JObject && ((JObject)e.Content)["errors"] is JArray)
                        err = e.StatusCode == HttpStatusCode.Unauthorized || e.StatusCode == HttpStatusCode.Forbidden ?
                            new HootsuiteSecurityException(e.StatusCode, (JObject)((JObject)e.Content)["errors"].FirstOrDefault()) :
                            new HootsuiteException(e.StatusCode, (JObject)((JObject)e.Content)["errors"].FirstOrDefault());
                    // New error structure
                    else if (e != null && e.Content != null && e.Content is JObject && ((JObject)e.Content)["error"] is JToken)
                        err = e.StatusCode == HttpStatusCode.Unauthorized || e.StatusCode == HttpStatusCode.Forbidden ?
                            new HootsuiteSecurityException(e.StatusCode, (JObject)e.Content) :
                            new HootsuiteException(e.StatusCode, (JObject)e.Content);
                    promise.SetException(err);
                }
            }
            return promise.Task;
        }

        bool RetryableError(Exception err)
        {
            return errors.isNetworkError(err) ||
              errors.isExpiredToken(err) ||
              errors.isServerError(err) ||
              errors.isRateLimited(err) ||
              false;
        }

        Backoff NewBackoff()
        {
            var newBackoff = new Backoff(new ExponentialBackoffStrategy(
                randomisationFactor: 0,
                initialDelay: InitialDelay,
                maxDelay: MaxDelay
            ));
            newBackoff.FailAfter(NumRetries);
            return newBackoff;
        }
    }
}
