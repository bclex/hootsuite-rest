var _ = require('lodash'),
  bunyan = require('bunyan'),
  config = require('../config/config'),
  logger;

var logConfig = {
  name: config.name,
  streams: [
    { level: 'warn', stream: process.stderr },
    { level: 'debug', stream: process.stderr }
  ]
};

module.exports = {
  createPath: function (var_args) {
    var args = Array.prototype.slice.call(arguments);
    args.unshift(config.api.version);
    return args.join('/');
  },
  createScimPath: function (var_args) {
    var args = Array.prototype.slice.call(arguments);
    args.unshift('v2');
    args.unshift('/scim');
    return args.join('/');
  },
  createOwlyPath: function (var_args) {
    var args = Array.prototype.slice.call(arguments);
    args.unshift(config.apiOwly.version);
    return args.join('/');
  },
  logger: function () {
    if (!logger) {
      logger = bunyan.createLogger({ name: config.name });
      if (process.env.APPSETTING_HOOTSUITE_LOGLEVEL) {
        logger.level(bunyan.resolveLevel(process.env.APPSETTING_HOOTSUITE_LOGLEVEL));
      }
    }
    return logger;
  },
};
