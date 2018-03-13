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

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionOwly"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ConnectionOwly(dynamic options)
        {
            _options = options ?? new { };
            _retry = new Retry(dyn.getProp(_options, "retry", new { }));
        }

        /// <summary>
        /// Gets the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="options">The options.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>Task&lt;dynamic&gt;.</returns>
        public Task<dynamic> get(string url, dynamic options = null, string contentType = null) => _request(url, null, options, Restler.Method.GET, contentType);
        /// <summary>
        /// Posts the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="options">The options.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>Task&lt;dynamic&gt;.</returns>
        public Task<dynamic> post(string url, dynamic options = null, string contentType = null) => _request(url, null, options, Restler.Method.POST, contentType);
        /// <summary>
        /// Puts the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="options">The options.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>Task&lt;dynamic&gt;.</returns>
        public Task<dynamic> put(string url, dynamic options = null, string contentType = null) => _request(url, null, options, Restler.Method.PUT, contentType);
        /// <summary>
        /// Deletes the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="options">The options.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>Task&lt;dynamic&gt;.</returns>
        public Task<dynamic> del(string url, dynamic options = null, string contentType = null) => _request(url, null, options, Restler.Method.DELETE, contentType);
        /// <summary>
        /// Heads the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="options">The options.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>Task&lt;dynamic&gt;.</returns>
        public Task<dynamic> head(string url, dynamic options = null, string contentType = null) => _request(url, null, options, Restler.Method.HEAD, contentType);
        /// <summary>
        /// Patches the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="options">The options.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>Task&lt;dynamic&gt;.</returns>
        public Task<dynamic> patch(string url, dynamic options = null, string contentType = null) => _request(url, null, options, Restler.Method.PATCH, contentType);
        /// <summary>
        /// Jsons the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        /// <param name="options">The options.</param>
        /// <returns>Task&lt;dynamic&gt;.</returns>
        public Task<dynamic> json(string url, object data, dynamic options = null) => _request(url, data, options, Restler.Method.GET);
        /// <summary>
        /// Posts the json.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        /// <param name="options">The options.</param>
        /// <returns>Task&lt;dynamic&gt;.</returns>
        public Task<dynamic> postJson(string url, object data, dynamic options = null) => _request(url, data, options, Restler.Method.POST);
        /// <summary>
        /// Puts the json.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        /// <param name="options">The options.</param>
        /// <returns>Task&lt;dynamic&gt;.</returns>
        public Task<dynamic> putJson(string url, object data, dynamic options = null) => _request(url, data, options, Restler.Method.PUT);

        private async Task<dynamic> _request(string url, object data, dynamic options, Restler.Method method, string contentType = null)
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
                        var y = await _rest.request(url, options, method, contentType);
                        var d = (JObject)y;
                        if ((string)d["success"] == "false" && d["errors"] != null)
                        {
                            _log($"Request failed: {d}");
                            throw new InvalidOperationException(d["errors"].ToString());
                        }
                        return (dynamic)d;
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
