using Hootsuite.Require;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hootsuite
{
    /// <summary>
    /// Class Connection.
    /// </summary>
    public class Connection
    {
        internal static readonly Regex HttpTestExp = new Regex(@"^https?://", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        readonly ApiUsage _lastApiUsage = new ApiUsage();
        readonly Action<string> _log = util.logger;
        readonly Restler _rest = new Restler();
        readonly dynamic _options;
        readonly Retry _retry;
        JObject _tokenData;

        /// <summary>
        /// Class ApiUsage.
        /// </summary>
        public class ApiUsage
        {
            public string Quota { get; set; }
            public int? QuotaUsed { get; set; }
            public int? RateLimitRequestsRemaining { get; set; }
        }

        /// <summary>
        /// Gets the last API usage.
        /// </summary>
        /// <value>The last API usage.</value>
        public ApiUsage LastApiUsage => _lastApiUsage;

        /// <summary>
        /// Class LoginContext.
        /// </summary>
        public class LoginContext
        {
            public string memberId { get; set; }
            public string organizationId { get; set; }
        }

        /// <summary>
        /// Class FrameContext.
        /// </summary>
        public class FrameContext
        {
            public string lang { get; set; }
            public string timezone { get; set; }
            public string pid { get; set; }
            public string uid { get; set; }
            public string ts { get; set; }
            public string token { get; set; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public Connection(dynamic options)
        {
            _options = options ?? new { };
            _tokenData = !string.IsNullOrEmpty(dyn.getProp<string>(_options, "accessToken")) ? dyn.ToJObject(new { access_token = dyn.getProp<string>(_options, "accessToken") }) : null;
            _retry = new Retry(dyn.getProp(_options, "retry", new { }));
        }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>The access token.</value>
        public string AccessToken
        {
            get { return _tokenData != null ? (string)_tokenData["access_token"] : null; }
            set { _tokenData = !string.IsNullOrEmpty(value) ? dyn.ToJObject(new { access_token = value }) : null; }
        }

        /// <summary>
        /// Gets or sets the on access token.
        /// </summary>
        /// <value>The on access token.</value>
        public Action<Connection> OnAccessToken { get; set; }
        /// <summary>
        /// Gets or sets the on response.
        /// </summary>
        /// <value>The on response.</value>
        public Action<Connection> OnResponse { get; set; }

        /// <summary>
        /// Gets the frame authentication token.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>System.String.</returns>
        public string GetFrameAuthToken(string url)
        {
            var secret = dyn.getProp<string>(_options, "secret");
            var frameCtx = dyn.getProp<FrameContext>(_options, "frameCtx");
            using (var hmac = new HMACSHA512(secret))
            {
                var bytes = Encoding.UTF8.GetBytes(frameCtx.uid + frameCtx.ts + (url ?? string.Empty));
                var hash = hmac.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
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
            if (!HttpTestExp.IsMatch(url))
            {
                var baseUrl = dyn.getProp(_options, "url", config.api_url);
                url = baseUrl + url;
            }
            _log($"Request: {url}");
            Func<Task<JObject>, Task<JObject>> requestFn = async (x) =>
            {
                if (!x.IsFaulted)
                {
                    var token = x.Result;
                    options.headers = dyn.getObj(options, "headers");
                    options.headers.Authorization = $"Bearer {token["access_token"]}";
                    try
                    {
                        Func<string, string> fixup = y => y.Replace("scim__user", "urn:ietf:params:scim:schemas:extension:Hootsuite:2.0:User");
                        var d = data == null ? (JObject)await _rest.request(url, options, method, contentType, (Action<HttpResponseMessage, string>)onResponse) : await _rest.json(url, data, options, method, (Action<HttpResponseMessage, string>)onResponse, fixup: fixup);
                        if (d["errors"] != null)
                        {
                            _log($"Request failed: {d}");
                            throw new HootsuiteException(HttpStatusCode.OK, d["errors"]);
                        }
                        return (dynamic)d;
                    }
                    catch (RestlerOperationException res)
                    {
                        var err = (JObject)res.Content;
                        if (err != null && err["errors"] != null) { _log($"Request failed: {err}"); throw; }
                        else
                        {
                            var e = res.E;
                            if (e != null) throw e;
                            else throw;
                            //var statusCode = Math.Floor((int)res.StatusCode / 100M);
                            //if (statusCode == 5) throw e;
                            //else if (statusCode == 4) throw e;
                            //else throw e;
                        }
                    }
                }
                else
                {
                    var err = x.Exception;
                    _log($"_request failed: {err}"); throw err;
                }
            };
            Func<bool, Task<JObject>> requestFn2 = async (forceOAuth) => await requestFn(GetOAuthToken(forceOAuth));
            return await _retry.start(requestFn2);

            void onResponse(HttpResponseMessage res, string body)
            {
                var headers = res.Headers;
                _lastApiUsage.Quota = headers.TryGetValues("X-Account-Quota", out IEnumerable<string> values) ? values.FirstOrDefault() : null;
                _lastApiUsage.QuotaUsed = headers.TryGetValues("X-Account-Quota-Used", out values) ? (int?)int.Parse(values.FirstOrDefault()) : null;
                _lastApiUsage.RateLimitRequestsRemaining = headers.TryGetValues("X-Account-Rate-Limit-Requests-Remaining", out values) ? (int?)int.Parse(values.FirstOrDefault()) : null;
                OnResponse?.Invoke(this);
            }
        }

        private async Task<JObject> GetOAuthToken(bool force = false)
        {
            if (force || _tokenData == null)
            {
                Func<bool, Task<JObject>> requestFn = async (forceOAuth) =>
                {
                    var loginCtx = dyn.getProp<LoginContext>(_options, "loginCtx");
                    var frameCtx = dyn.getProp<FrameContext>(_options, "frameCtx");
                    var options = new
                    {
                        data = new
                        {
                            grant_type = dyn.getProp(_options, "grantType", loginCtx != null || frameCtx != null ? "client_credentials" : "password"),
                            client_id = dyn.getProp<string>(_options, "clientId"),
                            client_secret = dyn.getProp<string>(_options, "clientSecret"),
                            username = dyn.getProp<string>(_options, "username"),
                            password = dyn.getProp<string>(_options, "password"),
                            // scope= dyn.getProp(_options, "scope", "oob"),
                        },
                        timeout = dyn.getProp(_options, "timeout", 20000),
                    };
                    var baseUrl = dyn.getProp(_options, "url", config.api_url);
                    try
                    {
                        var x = await _rest.post($"{baseUrl}/auth/oauth/v2/token", options);
                        var data = (JObject)x;
                        if (loginCtx != null || frameCtx != null)
                        {
                            _log($"Got pre-token: {data}");
                            try { return await GetOnBehalfAuthToken(data, loginCtx, frameCtx); }
                            catch (Exception err) { _log($"GetOAuthToken[pre-token] failed: {err}"); throw err; }
                        }
                        else
                        {
                            _log($"Got token: {data}");
                            _tokenData = data;
                            OnAccessToken?.Invoke(this);
                            return data;
                        }
                    }
                    catch (Exception err) { _log($"GetOAuthToken failed: {err}"); throw err; }
                };
                return await _retry.start(requestFn);
            }
            else
            {
                _log($"Using existing token: {_tokenData}");
                return _tokenData;
            }
        }

        private async Task<JObject> GetOnBehalfAuthToken(dynamic tokenData, LoginContext loginCtx, FrameContext frameCtx)
        {
            dynamic ctx;
            if (loginCtx != null)
                ctx = loginCtx.organizationId != null ? (dynamic)new { loginCtx.organizationId } : new { loginCtx.memberId };
            else if (frameCtx != null)
                ctx = new { memberId = frameCtx.uid };
            else throw new InvalidOperationException();

            Func<bool, Task<JObject>> requestFn = async (forceOAuth) =>
            {
                var options = new
                {
                    headers = new { Authorization = $"Bearer {tokenData.access_token}" },
                    timeout = dyn.getProp(_options, "timeout", 20000),
                };
                var baseUrl = dyn.getProp(_options, "url", config.api_url);
                try
                {
                    var x = await _rest.postJson($"{baseUrl}/v1/tokens", ctx, options);
                    var data = (JObject)x;
                    _log($"Got token: {data}");
                    _tokenData = data;
                    OnAccessToken?.Invoke(this);
                    return data;
                }
                catch (Exception err) { _log($"GetOnBehalfAuthToken failed: {err}"); throw err; }
            };
            return await _retry.start(requestFn);
        }
    }
}
