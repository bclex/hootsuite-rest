var _ = require('lodash'),
  Promise = require('bluebird'),
  util = require('../util'),
  log = util.logger();

function Scim(hootsuite, connection) {
  this._hootsuite = hootsuite;
  this._connection = connection;
}

Scim.prototype = {

  media: function () {
    var path = util.createPath('media');
    return this._connection.get(path);
  },
}

module.exports = Scim;
