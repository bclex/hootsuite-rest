using Hootsuite.Rest.Require;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

//https://stackoverflow.com/questions/38996593/promise-equivalent-in-c-sharp
//https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/console-webapiclient
namespace Hootsuite.Rest
{
    public class Connection
    {
        readonly dynamic _options;
        readonly Retry _retry;
        readonly Restler _rest = new Restler();
        readonly Action<string> _log = util.logger;
        JObject _tokenData;

        public class FrameContext
        {
            public string lang { get; set; }
            public string timezone { get; set; }
            public string pid { get; set; }
            public string uid { get; set; }
            public string ts { get; set; }
            public string token { get; set; }
        }

        public Connection(dynamic options)
        {
            _options = options ?? new { };
            _tokenData = dyn.hasProp(_options, "accessToken") ? dyn.ToJObject(new { access_token = _options.accessToken }) : null;
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

        static readonly Regex HttpTestExp = new Regex(@"^https?://", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private async Task<JObject> _request(string url, object data, dynamic options, Restler.Method method)
        {
            options = dyn.exp(options, true);
            if (!HttpTestExp.IsMatch(url))
            {
                var baseUrl = dyn.getProp(_options, "url", config.api_url);
                url = baseUrl + url;
            }
            _log($"Request: {url}");
            Func<bool, Task<JObject>> requestFn = (forceOAuth) => GetOAuthToken(forceOAuth).ContinueWith(x =>
            {
                var promise = new TaskCompletionSource<JObject>();
                if (!x.IsFaulted)
                {
                    var token = x.Result;
                    options.headers = dyn.getObj(options, "headers");
                    options.headers.Authorization = $"Bearer {token["access_token"]}";
                    _rest.request(url, options, method).ContinueWith((Action<Task<object>>)(y =>
                    {
                        if (!y.IsFaulted)
                        {
                            var d = (JObject)y.Result;
                            if ((string)d["success"] == "false" && d["errors"] != null)
                            {
                                _log($"Request failed: {d}");
                                promise.SetException(new InvalidOperationException(d["errors"].ToString()));
                                return;
                            }
                            promise.SetResult(d);
                        }
                        else
                        {
                            var res = y.Exception.InnerExceptions[0] as RestlerOperationException;
                            if (res == null) { promise.SetException(y.Exception.InnerExceptions[0]); return; }
                            var err = (JObject)res.Content;
                            if (err["errors"] != null)
                            {
                                _log($"Request failed: {err}");
                                promise.SetException(res);
                                return;
                            }
                            var statusCode = Math.Floor((int)res.StatusCode / 100M);
                            if (statusCode == 5) promise.SetException(res);
                            else if (statusCode == 4) promise.SetException(res);
                            else promise.SetException(res);
                        }
                    }));
                }
                else
                {
                    //    //var firstError = "Error"; // e.errors[0];
                    //    //throw new InvalidOperationException($"Authentication ({firstError.code}): {firstError.message}");
                    var res = x.Exception;
                    _log("ERROR");
                    promise.SetException(res);
                }
                return promise.Task.Result;
            });
            return await _retry.start(requestFn, this);
        }

        private string GetFrameAuthToken(string url)
        {
            var secret = dyn.getProp<string>(_options, "secret");
            var frameCtx = dyn.getProp<dynamic>(_options, "frameCtx");
            return "SHA512"; // sha512(frameCtx.uid + frameCtx.ts + (url ?? string.Empty) + secret);
        }

        private async Task<JObject> GetOAuthToken(bool force = false)
        {
            if (force || _tokenData == null)
            {
                var frameCtx = dyn.getProp<dynamic>(_options, "frameCtx");
                Func<bool, Task<JObject>> requestFn = (forceOAuth) =>
                {
                    var promise = new TaskCompletionSource<JObject>();
                    var options = new
                    {
                        data = new
                        {
                            grant_type = dyn.getProp(_options, "grantType", (frameCtx != null ? "client_credentials" : "password")),
                            client_id = dyn.getProp(_options, "clientId", (string)null),
                            client_secret = dyn.getProp(_options, "clientSecret", (string)null),
                            username = dyn.getProp(_options, "username", (string)null),
                            password = dyn.getProp(_options, "password", (string)null),
                            // scope= dyn.getProp(_options, "scope", "oob"),
                        },
                        timeout = dyn.getProp(_options, "timeout", 20000),
                    };
                    var baseUrl = dyn.getProp(_options, "url", config.api_url);
                    _rest.post($"{baseUrl}/auth/oauth/v2/token", options).ContinueWith((Action<Task<object>>)(async x =>
                    {
                        if (!x.IsFaulted)
                        {
                            var data = (JObject)x.Result;
                            if (frameCtx != null)
                            {
                                _log($"Got pre-token: {data}");
                                await GetOnBehalfAuthToken(data).ContinueWith(y =>
                                {
                                    if (!y.IsFaulted)
                                    {
                                        var token = (JObject)y.Result;
                                        promise.SetResult(token);
                                    }
                                    else
                                    {
                                        var err = y.Exception;
                                        _log("ERROR");
                                        promise.SetException(err);
                                    }
                                });
                            }
                            else
                            {
                                _log($"Got token: {data}");
                                _tokenData = data;
                                promise.SetResult(data);
                            }
                        }
                        else
                        {
                            var err = x.Exception;
                            _log("ERROR");
                            promise.SetException(err);
                            return;
                        }
                    }));
                    return promise.Task;
                };
                return await _retry.start(requestFn, this);
            }
            else
            {
                _log($"Using existing token: {_tokenData}");
                return _tokenData;
            }
        }

        private async Task<JObject> GetOnBehalfAuthToken(dynamic tokenData)
        {
            var promise = new TaskCompletionSource<JObject>();
            var frameCtx = dyn.getProp(_options, "frameCtx", new { uid = string.Empty });
            Func<bool, Task<JObject>> requestFn = (forceOAuth) =>
            {
                var options = new
                {
                    data = new { memberId = frameCtx.uid },
                    headers = new { Authorization = $"Bearer {tokenData.access_token}" },
                    timeout = dyn.getProp(_options, "timeout", 20000),
                };
                var baseUrl = dyn.getProp(_options, "url", config.api_url);
                _rest.postJson($"{baseUrl}/v1/tokens", options.data, options).ContinueWith(x =>
                {
                    if (!x.IsFaulted)
                    {
                        var data = (JObject)x.Result;
                        _log($"Got token: {data}");
                        _tokenData = data;
                        promise.SetResult(data);
                    }
                    else
                    {
                        var err = x.Exception;
                        _log("ERROR");
                        promise.SetException(err);
                    }
                });
                return promise.Task;
            };
            return await _retry.start(requestFn, this);
        }
    }
}
