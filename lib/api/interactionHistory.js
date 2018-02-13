var _ = require('lodash'),
  Promise = require('bluebird'),
  util = require('../util'),
  log = util.logger();

function InteractionHistory(hootsuite, connection) {
  this._hootsuite = hootsuite;
  this._connection = connection;
}

InteractionHistory.prototype = {
  get: function (socialNetworkType, socialNetworkId, targetSocialNetworkId, cursor, limit) {
    var path = util.createPath('interactions', socialNetworkType);
    var options = {
      query: {
        socialNetworkId,
        targetSocialNetworkId,
        cursor,
        limit
      },
      _method: 'GET'
    };
    return this._connection.get(path, data);
  },
}

module.exports = InteractionHistory;
