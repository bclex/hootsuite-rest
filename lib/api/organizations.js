var _ = require('lodash'),
  Promise = require('bluebird'),
  util = require('../util'),
  log = util.logger();

function Organizations(hootsuite, connection) {
  this._hootsuite = hootsuite;
  this._connection = connection;
}

Organizations.prototype = {

  media: function () {
    var path = util.createPath('media');
    return this._connection.get(path);
  },
}

module.exports = Organizations;
