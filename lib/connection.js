var _ = require('lodash'),
  rest = require('restler'),
  Promise = require('bluebird'),
  Retry = require('./retry'),
  errors = require('./errors'),
  util = require('util'),
  sha512 = require('js-sha512'),
  EventEmitter = require('events').EventEmitter,
  config = require('../config/config'),
  log = require('./util').logger(),
  restlerMethodArgCount = {
    get: 2,
    post: 2,
    put: 2,
    del: 2,
    head: 2,
    patch: 2,
    json: 3,
    postJson: 3,
    putJson: 3
  };

function Connection(options) {
  EventEmitter.call(this);
  this._options = options || {};
  this._tokenData = this._options.accessToken ? { access_token: this._options.accessToken } : null;
  this._retry = new Retry(this._options.retry || {});
  this.apiCallCount = 0;
  _.each(restlerMethodArgCount, function (count, method) {
    this[method] = function (var_args) {
      var args = Array.prototype.slice.call(arguments);
      while (args.length < count) {
        args.push(undefined);
      }
      args.unshift(method);
      return this._request.apply(this, args);
    };
  }.bind(this));
}
util.inherits(Connection, EventEmitter);

function getNextPageFn(conn, method, args) {
  var options = _.clone(_.last(args) || {});
  args = _.clone(args);
  args.pop();
  return function nextPage(nextPageToken) {
    var params = options;
    if (method === 'get') params = options.query = options.query || {};
    else if (method === 'post' || method === 'put') params = options.data = options.data || {};
    params.nextPageToken = nextPageToken;
    return conn._request.apply(conn, _.flatten([method, args, options], true));
  }
}

Connection.prototype._request = function (method) {
  var args = Array.prototype.slice.call(arguments, 1),
    url = _.first(args);
  if (!/^https?:\/\//i.test(url)) {
    var baseUrl = this._options.url || config.api.url;
    args[0] = baseUrl + url;
  }
  this.emit('apiCall', { apiCallCount: ++this.apiCallCount });
  log.debug('Request:', arguments);
  var requestFn = function requestFn(forceOAuth) {
    var iframeCtx = this._options.iframeCtx;
    return this.getOAuthToken(forceOAuth, iframeCtx).then(function (token) {
      var defer = Promise.defer(),
        options = _.last(args) || {},
        nextPageFn = getNextPageFn(this, method, args);
      args.pop();
      options.headers = _.extend({}, options.headers, {
        Authorization: 'Bearer ' + token.access_token
      });
      args.push(options);
      rest[method].apply(rest, args)
        .on('success', function (data, resp) {
          if (data.success === false && _.has(data, 'errors')) {
            log.debug('Request failed: ', data);
            defer.reject(data);
          } else {
            if (_.has(data, 'nextPageToken')) {
              Object.defineProperty(data, 'nextPage', {
                enumerable: false,
                value: _.partial(nextPageFn, data.nextPageToken)
              });
            }
            defer.resolve(data);
          }
        })
        .on('error', function (err, resp) {
          defer.reject(err);
        })
        .on('fail', function (data, resp) {
          if (_.has(data, 'errors')) {
            log.debug('Request failed: ', data);
            return defer.reject(data);
          }
          var statusCode = Math.floor(resp.statusCode / 100);
          if (statusCode === 5) defer.reject(new errors.Http5XXError(data));
          else if (statusCode === 4) defer.reject(new errors.Http4XXError(data));
          else defer.reject(data);
        })
        .on('timeout', function (ms) {
          defer.reject(new errors.TimeoutError(ms));
        });
      return defer.promise;
    }.bind(this), function (e) {
      throw new Error('Authentication (' + e.error + '): ' + e.error_description);
    });
  };
  return this._retry.start(requestFn, this);
};

Connection.prototype.getIframeAuthToken = function (iframeCtx, url) {
  var secret = this._options.secret || '';
  return sha512(iframeCtx.uid.toString() + iframeCtx.ts.toString() + (url || '') + secret);
}

Connection.prototype.getOAuthToken = function (force, iframeCtx) {
  force = force || false;
  if (force || this._tokenData == null) {
    var defer = Promise.defer();
    var requestFn = function () {
      var options = {
        data: {
          grant_type: this.grantType || (iframeCtx ? 'client_credentials' : 'password'),
          client_id: this._options.clientId,
          client_secret: this._options.clientSecret,
          username: this._options.username,
          password: this._options.password,
          // scope: this._options.scope || 'oob',
        },
        timeout: this._options.timeout || 20000
      };
      var baseUrl = this._options.url || config.api.url;
      rest.post(baseUrl + '/auth/oauth/v2/token', options)
        .on('success', function (data, resp) {
          if (iframeCtx) {
            log.debug('Got pre-token: ', data);
            this.getOnBehalfOAuthToken(data, iframeCtx).then(function (token) {
              this._tokenData = token;
              defer.resolve(token);
            }, function () {
              defer.reject();
            });
          }
          else {
            log.debug('Got token: ', data);
            this._tokenData = data;
            defer.resolve(data);
          }
        }.bind(this))
        .on('timeout', function (ms) {
          log.debug('Timeout');
          defer.reject(ms);
        })
        .on('fail', function (data, resp) {
          defer.reject(data);
        })
        .on('error', function (err) {
          defer.reject(err);
        });
      return defer.promise;
    };
    return this._retry.start(requestFn, this);
  } else {
    log.debug('Using existing token: ', this._tokenData);
    return Promise.resolve(this._tokenData);
  }
};

Connection.prototype.getOnBehalfOAuthToken = function (tokenData, iframeCtx) {
  var defer = Promise.defer();
  var requestFn = function () {
    var getOptions = {
      data: { memberId: iframeCtx.uid },
      headers: { Authorization: 'Bearer ' + tokenData },
      timeout: this._options.timeout || 20000
    };
    var baseUrl = this._options.url || config.api.url;
    rest.post(baseUrl + '/v1/tokens', options)
      .on('success', function (data, resp) {
        log.debug('Got token: ', data);
        this._tokenData = data;
        defer.resolve(data);
      }.bind(this))
      .on('timeout', function (ms) {
        log.debug('Timeout');
        defer.reject(ms);
      })
      .on('fail', function (data, resp) {
        defer.reject(data);
      })
      .on('error', function (err) {
        defer.reject(err);
      });
    return defer.promise;
  };
  return this._retry.start(requestFn, this);
};

module.exports = Connection;