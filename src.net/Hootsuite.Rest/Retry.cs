using Hootsuite.Rest.Require;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Hootsuite.Rest
{
    public class Retry
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

        int NumRetries { get { return dyn.getProp(_options, "maxRetries", DefaultRetryCount); } }
        int InitialDelay { get { return dyn.getProp(_options, "initialDelay", DefaultInitialDelay); } }
        int MaxDelay { get { return dyn.getProp(_options, "maxDelay", DefaultMaxDelay); } }

        Exception _lastError;
        internal Task<JObject> start(Func<bool, Task<JObject>> fetchFn, Connection connection)
        {
            var promise = new TaskCompletionSource<JObject>();
            var forceOAuth = false;
            executeFn();
            _newBackoff.OnReady = (number, delay) =>
            {
                _log($"Requesting for url {{ number: {number}, delay: {delay} }}");
                executeFn();
            };
            _newBackoff.OnFail = (err) =>
            {
                promise.SetException(_lastError);
            };
            void executeFn()
            {
                fetchFn(forceOAuth).ContinueWith(x =>
                {
                    if (!x.IsFaulted)
                    {
                        var result = x.Result;
                        promise.SetResult(result);
                    }
                    else
                    {
                        var err = x.Exception;
                        _lastError = err;
                        if (RetryableError(err))
                        {
                            if (errors.isRateLimited(err))
                            {
                                var task = Task.Run(() => { });
                                if (task.Wait(TimeSpan.FromSeconds(MaxDelay)))
                                    _newBackoff.Backoff_();
                            }
                            else
                            {
                                if (errors.isExpiredToken(err))
                                    forceOAuth = true;
                                _newBackoff.Backoff_();
                            }
                        }
                        else promise.SetException(err);
                    }
                });
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
