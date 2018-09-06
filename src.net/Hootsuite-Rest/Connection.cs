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
        readonly LastResponseState _lastResponse = new LastResponseState();
        readonly Action<string> _log = util.logger;
        readonly Restler _rest = new Restler();
        readonly dynamic _options;
        readonly Retry _retry;
        JObject _tokenData;

        /// <summary>
        /// Class LastResponseState.
        /// </summary>
        public class LastResponseState
        {
            /// <summary>
            /// Gets or sets the quota.
            /// </summary>
            /// <value>The quota.</value>
            public string Quota { get; set; }
            /// <summary>
            /// Gets or sets the quota used.
            /// </summary>
            /// <value>The quota used.</value>
            public int? QuotaUsed { get; set; }
            /// <summary>
            /// Gets or sets the rate limit requests remaining.
            /// </summary>
            /// <value>The rate limit requests remaining.</value>
            public int? RateLimitRequestsRemaining { get; set; }
        }

        /// <summary>
        /// Gets the last response.
        /// </summary>
        /// <value>The last response.</value>
        public LastResponseState LastResponse => _lastResponse;

        /// <summary>
        /// Class LoginContext.
        /// </summary>
        public class LoginContext
        {
            /// <summary>
            /// Gets or sets the member identifier.
            /// </summary>
            /// <value>The member identifier.</value>
            public string memberId { get; set; }
            /// <summary>
            /// Gets or sets the organization identifier.
            /// </summary>
            /// <value>The organization identifier.</value>
            public string organizationId { get; set; }
        }

        /// <summary>
        /// Class FrameContext.
        /// </summary>
        public class FrameContext
        {
            /// <summary>
            /// Gets or sets the language.
            /// </summary>
            /// <value>The language.</value>
            public string lang { get; set; }
            /// <summary>
            /// Gets or sets the timezone.
            /// </summary>
            /// <value>The timezone.</value>
            public string timezone { get; set; }
            /// <summary>
            /// Gets or sets the pid.
            /// </summary>
            /// <value>The pid.</value>
            public string pid { get; set; }
            /// <summary>
            /// Gets or sets the uid.
            /// </summary>
            /// <value>The uid.</value>
            public string uid { get; set; }
            /// <summary>
            /// Gets or sets the ts.
            /// </summary>
            /// <value>The ts.</value>
            public string ts { get; set; }
            /// <summary>
            /// Gets or sets the token.
            /// </summary>
            /// <value>The token.</value>
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
            get => _tokenData != null ? (string)_tokenData["access_token"] : null;
            set => _tokenData = !string.IsNullOrEmpty(value) ? dyn.ToJObject(new { access_token = value }) : null;
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

        async Task<dynamic> _request(string url, object data, dynamic options, Restler.Method method, string contentType = null)
        {
            options = dyn.exp(options, true);
            if (!HttpTestExp.IsMatch(url))
            {
                var baseUrl = dyn.getProp(_options, "url", config.api_url);
                url = baseUrl + url;
            }
            _log($"Request: {url}\n");
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
                        dynamic d;
                        if (method == Restler.Method.DELETE)
                        {
                            d = await _rest.request(url, options, method, contentType, (Action<HttpResponseMessage, string>)onResponse);
                            if (d is string)
                                d = new JObject();
                        }
                        else
                        {
                            d = data == null
                                   ? (JObject)await _rest.request(url, options, method, contentType, (Action<HttpResponseMessage, string>)onResponse)
                                   : await _rest.json(url, data, options, method, (Action<HttpResponseMessage, string>)onResponse, fixup: fixup);
                        }
                        if (dyn.hasProp(d, "errors"))
                        {
                            _log($"Request failed: {d}\n");
                            throw new HootsuiteException(HttpStatusCode.OK, d["errors"]);
                        }
                        return d;
                    }
                    catch (RestlerOperationException res)
                    {
                        var errAsString = res.Content as string;
                        if (errAsString != null)
                        {
                            if (errAsString.StartsWith("max rate reached:"))
                                throw errors.MakeRateLimitedError();
                            _log($"Request failed: {errAsString}\n");
                            throw;
                        }
                        var err = res.Content as JObject;
                        if (err != null && err["error"] != null) { _log($"Request failed: {err}\n"); throw; }
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
                    _log($"_request failed: {err}\n"); throw err;
                }
            };
            Func<bool, Task<JObject>> requestFn2 = async (forceOAuth) => await requestFn(GetOAuthToken(forceOAuth));
            return await _retry.start(requestFn2);

            void onResponse(HttpResponseMessage res, string body)
            {
                var headers = res.Headers;
                _lastResponse.Quota = headers.TryGetValues("X-Account-Quota", out IEnumerable<string> values) ? values.FirstOrDefault() : null;
                _lastResponse.QuotaUsed = headers.TryGetValues("X-Account-Quota-Used", out values) ? (int?)int.Parse(values.FirstOrDefault()) : null;
                _lastResponse.RateLimitRequestsRemaining = headers.TryGetValues("X-Account-Rate-Limit-Requests-Remaining", out values) ? (int?)int.Parse(values.FirstOrDefault()) : null;
                OnResponse?.Invoke(this);
            }
        }

        async Task<JObject> GetOAuthToken(bool force = false)
        {
            if (force || _tokenData == null)
            {
                Func<bool, Task<JObject>> requestFn = async (forceOAuth) =>
                {
                    var loginCtx = (LoginContext)dyn.getProp<LoginContext>(_options, "loginCtx");
                    var basic = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{dyn.getProp<string>(_options, "clientId")}:{dyn.getProp<string>(_options, "clientSecret")}"));
                    var options = new
                    {
                        data = new
                        {
                            grant_type = dyn.getProp(_options, "grantType", dyn.hasProp(_options, "code") ? "authorization_code" :
                                loginCtx?.memberId != null ? "member_app" :
                                loginCtx?.organizationId != null ? "organization_app" :
                                "client_credentials"),
                            code = dyn.getProp<string>(_options, "code"),
                            redirect_uri = dyn.getProp<string>(_options, "redirectUri"),
                            member_id = dyn.getProp<string>(_options, "memberId", loginCtx?.memberId),
                            organization_id = dyn.getProp<string>(_options, "organizationId", loginCtx?.organizationId),
                            //scope = dyn.getProp<string>(_options, "scope"),
                            refresh_token = dyn.getProp<string>(_options, "refresh_token"),
                        },
                        headers = new { Authorization = $"Basic {basic}" },
                        timeout = dyn.getProp(_options, "timeout", 20000),
                    };
                    var baseUrl = dyn.getProp(_options, "url", config.api_url);
                    try
                    {
                        var x = await _rest.post($"{baseUrl}/oauth2/token", options);
                        var data = (JObject)x;
                        _log($"Got token: {data}\n");
                        _tokenData = data;
                        OnAccessToken?.Invoke(this);
                        return data;
                    }
                    catch (Exception err) { _log($"GetOAuthToken failed: {err}\n"); throw err; }
                };
                return await _retry.start(requestFn);
            }
            else
            {
                _log($"Using existing token: {_tokenData}\n");
                return _tokenData;
            }
        }
    }
}
