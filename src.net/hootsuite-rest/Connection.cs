using Hootsuite.Rest.Require;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

//https://stackoverflow.com/questions/38996593/promise-equivalent-in-c-sharp
//https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/console-webapiclient
namespace Hootsuite.Rest
{
    public class Connection
    {
        Options _options;
        object _tokenData;
        //RetryOptions _retry;
        Action<string> _log = util.logger;

        public class FrameContext
        {
            public string lang { get; set; }
            public string timezone { get; set; }
            public string pid { get; set; }
            public string uid { get; set; }
            public string ts { get; set; }
            public string token { get; set; }
        }

        public class Options
        {
            public string accessToken { get; set; }
            public string secret { get; set; }
            public FrameContext frameCtx { get; set; }
            //public Retry.Options retry { get; set; }
        }

        readonly Restler rest = new Restler();

        public Connection(Options options)
        {
            _options = options ?? new Options { };
            //_tokenData = !string.IsNullOrEmpty(_options.accessToken) ? new AccessObj { access_token = _options.accessToken } : null;
            //_retry = new Retry(_options.retry ?? new Retry.Options { });
        }

        public JToken get(string url, dynamic options = null) => rest.get(url, options);
        public JToken post(string url, dynamic options = null) => rest.post(url, options);
        public JToken put(string url, dynamic options = null) => rest.put(url, options);
        public JToken del(string url, dynamic options = null) => rest.del(url, options);
        public JToken head(string url, dynamic options = null) => rest.head(url, options);
        public JToken patch(string url, dynamic options = null) => rest.patch(url, options);
        public JToken json(string url, object data, dynamic options = null) => rest.json(url, data, options);
        public JToken postJson(string url, object data, dynamic options = null) => rest.postJson(url, data, options);
        public JToken putJson(string url, object data, dynamic options = null) => rest.putJson(url, data, options);

        //    private object GetNextPageFn(object conn, object method, object args)
        //    {
        //        var options = _.clone(_.last(args) || { });
        //        args = _.clone(args);
        //        args.pop();
        //        return function nextPage(nextPageToken) {
        //            var params = options;
        //            if (method === 'get') params = options.query = options.query || { };
        //else if (method === 'post' || method === 'put') params = options.data = options.data || { };
        //params.nextPageToken = nextPageToken;
        //            return conn._request.apply(conn, _.flatten([method, args, options], true));
        //        }
        //    }

        //      private void _request(string method)
        //      {
        //          var args = Array.prototype.slice.call(arguments, 1),
        //            url = _.first(args);
        //          if (!/^ https ?:\/\//i.test(url)) {
        //  var baseUrl = this._options.url || config.api.url;
        //          args[0] = baseUrl + url;
        //      }
        //this.emit('apiCall', { apiCallCount: ++this.apiCallCount });
        //log.debug('Request:', arguments);
        //var requestFn = function requestFn(forceOAuth)
        //      {
        //          var frameCtx = this._options.frameCtx || null;
        //          return this.getOAuthToken(forceOAuth).then(function(token) {
        //              var defer = Promise.defer(),
        //                options = _.last(args) || { },
        //      nextPageFn = getNextPageFn(this, method, args);
        //              args.pop();
        //              options.headers = _.extend({ }, options.headers, {
        //                  Authorization: 'Bearer ' + token.access_token
        //    });
        //              args.push(options);
        //              rest[method].apply(rest, args)
        //                .on('success', function(data, resp) {
        //                  if (data.success === false && _.has(data, 'errors'))
        //                  {
        //                      log.debug('Request failed: ', data);
        //                      defer.reject(data);
        //                  }
        //                  else
        //                  {
        //                      if (_.has(data, 'nextPageToken'))
        //                      {
        //                          Object.defineProperty(data, 'nextPage', {
        //                              enumerable: false,
        //              value: _.partial(nextPageFn, data.nextPageToken)
        //                          });
        //                      }
        //                      defer.resolve(data);
        //                  }
        //              })
        //      .on('error', function(err, resp) {
        //                  defer.reject(err);
        //              })
        //      .on('fail', function(data, resp) {
        //                  if (_.has(data, 'errors'))
        //                  {
        //                      log.debug('Request failed: ', data);
        //                      return defer.reject(data);
        //                  }
        //                  var statusCode = Math.floor(resp.statusCode / 100);
        //                  if (statusCode === 5) defer.reject(new errors.Http5XXError(data));
        //                  else if (statusCode === 4) defer.reject(new errors.Http4XXError(data));
        //                  else defer.reject(data);
        //              })
        //      .on('timeout', function(ms) {
        //                  defer.reject(new errors.TimeoutError(ms));
        //              });
        //              return defer.promise;
        //          }.bind(this), function(e) {
        //              let firstError = e.errors[0];
        //              throw new Error('Authentication (' + firstError.code + '): ' + firstError.message);
        //          });
        //      };
        //return this._retry.start(requestFn, this);
        //      }

        private string GetFrameAuthToken(string url)
        {
            var secret = _options.secret ?? string.Empty;
            var frameCtx = _options.frameCtx;
            return "SHA512"; // sha512(frameCtx.uid + frameCtx.ts + (url ?? string.Empty) + secret);
        }

        //      private string GetOAuthToken(bool force = false)
        //      {
        //          if (force || _tokenData == null)
        //          {
        //              var frameCtx = _options.frameCtx;
        //              var promise = new TaskCompletionSource<object>();
        //              var requestFn = () =>
        //              {
        //                  var options = new Rest.Option
        //                  {
        //                      data = new
        //                      {
        //                          grant_type = _options.grantType ?? (frameCtx != null ? "client_credentials" : "password"),
        //                          client_id = _options.clientId,
        //                          client_secret = _options.clientSecret,
        //                          username = _options.username,
        //                          password = _options.password,
        //                          // scope= _options.scope || 'oob',
        //                      },
        //                      timeout = _options.timeout ?? 20000
        //                  };
        //                  var baseUrl = _options.url ?? config.api.url;
        //                  //        rest.Post(baseUrl + "/auth/oauth/v2/token", options)
        //                  //          .on('success', function(data, resp) {
        //                  //            if (frameCtx)
        //                  //            {
        //                  //                log.debug('Got pre-token: ', data);
        //                  //                GetOnBehalfAuthToken(data).then(function(token) {
        //                  //                        promise.SetResult(token);
        //                  //                }).catch (function (data) {
        //                  //            defer.reject(data);
        //                  //        });
        //                  //        }
        //                  //        else {
        //                  //            log.debug("Got token: ", data);
        //                  //            this._tokenData = data;
        //                  //            promise.SetResult(data);
        //                  //        }
        //                  //    }.bind(this))
        //                  //.on('timeout', function(ms) {
        //                  //        log.debug('Timeout');
        //                  //        defer.reject(ms);
        //                  //    })
        //                  //.on('fail', function(data, resp) {
        //                  //        defer.reject(data);
        //                  //    })
        //                  //.on('error', function(err) {
        //                  //        defer.reject(err);
        //                  //    });
        //                  return promise;
        //              };
        //              return _retry.start(requestFn, this);
        //          }
        //          else
        //          {
        //              log.debug('Using existing token: ', _tokenData);
        //              return Promise.resolve(_tokenData);
        //          }
        //      }

        //      private string GetOnBehalfAuthToken(string tokenData)
        //      {
        //          var frameCtx = this._options.frameCtx || { };
        //          var defer = Promise.defer();
        //          var requestFn = function() {
        //              var options = {
        //    data: { memberId: frameCtx.uid },
        //    headers: { Authorization: 'Bearer ' + tokenData.access_token },
        //    timeout: this._options.timeout || 20000
        //               };
        //          var baseUrl = this._options.url || config.api.url;
        //          rest.postJson(baseUrl + '/v1/tokens', options.data, options)
        //            .on('success', function(data, resp) {
        //              log.debug('Got token: ', data);
        //              this._tokenData = data;
        //              defer.resolve(data);
        //          }.bind(this))
        //    .on('timeout', function(ms) {
        //              log.debug('Timeout');
        //              defer.reject(ms);
        //          })
        //    .on('fail', function(data, resp) {
        //              defer.reject(data);
        //          })
        //    .on('error', function(err) {
        //              defer.reject(err);
        //          });
        //          return defer.promise;
        //      };
        //return this._retry.start(requestFn, this);
        //      }
    }
}
