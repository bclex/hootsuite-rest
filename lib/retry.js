var _ = require('lodash'),
  backoff = require('backoff'),
  Promise = require('bluebird'),
  errors = require('./errors'),
  util = require('./util'),
  log = util.logger();

function Retry(options) {
  this._options = options || {};
}

Retry.prototype = {
  defaultRetryCount: 5,
  defaultInitialDelay: 1000, // 1 second
  defaultMaxDelay: 20000,

  numRetries: function () {
    return this._options.maxRetries || this.defaultRetryCount;
  },

  initialDelay: function () {
    return this._options.initialDelay || this.defaultInitialDelay;
  },

  maxDelay: function () {
    return this._options.maxDelay || this.defaultMaxDelay;
  },

  start: function (fetchFn, context) {
    var self = this,
      defer = Promise.defer(),
      newBackoff = this._newBackoff(),
      forceOAuth = false,
      lastError;

    if (_.isObject(context)) {
      fetchFn = _.bind(fetchFn, context);
    }

    executeFn();
    newBackoff.on('ready', function (number, delay) {
      log.debug('Requesting for url', { number: number, delay: delay });
      executeFn();
    }).on('fail', function () {
      defer.reject(lastError);
    });
    function executeFn() {
      fetchFn(forceOAuth).then(function (result) {
        defer.resolve(result);
      }).catch(function (err) {
        lastError = err;
        if (self.retryableError(err)) {
          if (errors.isRateLimited(err)) {
            setTimeout(function () {
              newBackoff.backoff();
            }, self.maxDelay());
          } else {
            if (errors.isExpiredToken(err)) {
              forceOAuth = true;
            }
            newBackoff.backoff();
          }
        } else {
          defer.reject(err);
        }
      });
    }
    return defer.promise;
  },

  retryableError: function (err) {
    return errors.isNetworkError(err) ||
      errors.isExpiredToken(err) ||
      errors.isServerError(err) ||
      errors.isRateLimited(err) ||
      false;
  },

  _newBackoff: function () {
    var newBackoff = backoff.exponential({
      randomisationFactor: 0,
      initialDelay: this.initialDelay(),
      maxDelay: this.maxDelay()
    });
    newBackoff.failAfter(this.numRetries());
    return newBackoff;
  }
};

module.exports = Retry;
