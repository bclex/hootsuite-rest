var _ = require('lodash'),
  util = require('util'),
  retryableNetworkErrorCodes,

  // error codes
  errorCodes = {
    TOKEN_ERROR: 1032,
    SYSTEM_ERROR: 5000,
    RATE_LIMIT_REACHED: 1003,
    ACCESS_DENIED: 5000,
    QUOTA_REACHED: 5000,
  };

// These error codes were pulled from
retryableNetworkErrorCodes = [
  'EADDRINFO',
  'EADDRNOTAVAIL',
  'ECONNRESET',
  'ENOTFOUND',
  'ECONNABORTED',
  'EPROTO',
  'ECONNREFUSED',
  'EHOSTUNREACH',
  'ENETDOWN',
  'ENETUNREACH',
  'ENONET',
  'ENOTCONN',
  'ENOTSOCK',
  'ETIMEDOUT'
];

function Http5XXError(msg) {
  Error.call(this);
  this.message = msg;
  this.code = '5XX';
}
util.inherits(Http5XXError, Error);

function Http4XXError(msg) {
  Error.call(this);
  this.message = msg;
  this.code = '4XX';
}
util.inherits(Http4XXError, Error);

function TimeoutError(timeout) {
  Error.call(this, 'Timed out after ' + timeout + ' ms.');
}
util.inherits(TimeoutError, Error);

function hasErrorCode(err, code) {
  return isError(err) &&
    !!_.find(err.errors, { code: code });
}

function isError(err) {
  var val = _.has(err, 'errors') &&
    _.isArray(err.errors) &&
    err.errors.length > 0;
  return val;
}

module.exports = {
  Http5XXError: Http5XXError,
  Http4XXError: Http4XXError,
  isError: isError,
  hasErrorCode: hasErrorCode,
  errorCodes: errorCodes,

  isNetworkError: function (err) {
    var code = err.code || '';
    return retryableNetworkErrorCodes.indexOf(code) > -1 || (err instanceof TimeoutError);
  },

  isServerError: function (err) {
    return (err instanceof Http5XXError) ||
      // hasErrorCode(err, errorCodes.TIMED_OUT) ||
      // hasErrorCode(err, errorCodes.API_UNAVAILBLE) ||
      hasErrorCode(err, errorCodes.SYSTEM_ERROR);
  },

  isExpiredToken: function (err) {
    return hasErrorCode(err, errorCodes.TOKEN_ERROR);
  },

  isRateLimited: function (err) {
    return hasErrorCode(err, errorCodes.RATE_LIMIT_REACHED);
  }
};
