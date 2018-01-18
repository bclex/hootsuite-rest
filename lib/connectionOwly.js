var _ = require('lodash'),
  rest = require('restler'),
  Promise = require('bluebird'),
  Retry = require('./retry'),
  errors = require('./errors'),
  util = require('util'),
  EventEmitter = require('events').EventEmitter,
  config = require('../config/config'),
  log = require('./util').logger(),
  restlerMethodArgCount = {
    get: 2,
    post: 2,
  };

function ConnectionOwly(options) {
  EventEmitter.call(this);
  this._options = options || {};
  this._tokenData = this._options.owlyToken ? { access_token: this._options.owlyToken } : null;
  this._retry = new Retry(this._options.retry || {});
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

ConnectionOwly.prototype._request = function (method) {
  var args = Array.prototype.slice.call(arguments, 1),
    url = _.first(args);
  if (!/^https?:\/\//i.test(url)) {
    var baseUrl = this._options.url || config.apiOwly.url;
    args[0] = baseUrl + url;
  }
  log.debug('Request:', arguments);
  var requestFn = function requestFn(forceOAuth) {
    var defer = Promise.defer(),
      options = _.last(args) || {};
    args.pop();
    args.push(options);
    rest[method].apply(rest, args)
      .on('success', function (data, resp) {
        if (data.success === false && _.has(data, 'errors')) {
          log.debug('Request failed: ', data);
          defer.reject(data);
        } else {
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
  };
  return this._retry.start(requestFn, this);
};

module.exports = ConnectionOwly;