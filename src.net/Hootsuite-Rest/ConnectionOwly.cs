using Hootsuite.Require;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Hootsuite
{
    /// <summary>
    /// Class ConnectionOwly.
    /// </summary>
    public class ConnectionOwly
    {
        readonly Action<string> _log = util.logger;
        readonly Restler _rest = new Restler();
        readonly dynamic _options;
        readonly Retry _retry;

        public ConnectionOwly(dynamic options)
        {
            _options = options ?? new { };
            _retry = new Retry(dyn.getProp(_options, "retry", new { }));
        }

        public Task<JObject> get(string url, dynamic options = null) => _request(url, null, options, Restler.Method.GET);
        public Task<JObject> post(string url, dynamic options = null) => _request(url, null, options, Restler.Method.POST);
        public Task<JObject> put(string url, dynamic options = null) => _request(url, null, options, Restler.Method.PUT);
        public Task<JObject> del(string url, dynamic options = null) => _request(url, null, options, Restler.Method.DELETE);
        public Task<JObject> head(string url, dynamic options = null) => _request(url, null, options, Restler.Method.HEAD);
        public Task<JObject> patch(string url, dynamic options = null) => _request(url, null, options, Restler.Method.PATCH);
        public Task<JObject> json(string url, object data, dynamic options = null) => _request(url, data, options, Restler.Method.GET);
        public Task<JObject> postJson(string url, object data, dynamic options = null) => _request(url, data, options, Restler.Method.POST);
        public Task<JObject> putJson(string url, object data, dynamic options = null) => _request(url, data, options, Restler.Method.PUT);

        private async Task<JObject> _request(string url, object data, dynamic options, Restler.Method method)
        {
            options = dyn.exp(options, true);
            if (!Connection.HttpTestExp.IsMatch(url))
            {
                var baseUrl = dyn.getProp(_options, "url", config.apiOwly_url);
                url = baseUrl + url;
            }
            _log($"Request: {url}");
            Func<bool, Task<JObject>> requestFn = async (forceOAuth) =>
            {
                    try
                    {
                        var y = await _rest.request(url, options, method);
                        var d = (JObject)y;
                        if ((string)d["success"] == "false" && d["errors"] != null)
                        {
                            _log($"Request failed: {d}");
                            throw new InvalidOperationException(d["errors"].ToString());
                        }
                        return d;
                    }
                    catch (RestlerOperationException res)
                    {
                        var err = (JObject)res.Content;
                        if (err["errors"] != null) { _log($"Request failed: {err}"); throw; }
                        else
                        {
                            var statusCode = Math.Floor((int)res.StatusCode / 100M);
                            if (statusCode == 5) throw res;
                            else if (statusCode == 4) throw res;
                            else throw res;
                        }
                    }
            };
            return await _retry.start(requestFn);
        }
    }
}
