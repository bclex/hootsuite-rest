var _ = require('lodash'),
  Promise = require('bluebird'),
  util = require('../util'),
  log = util.logger();

function Teams(hootsuite, connection) {
  this._hootsuite = hootsuite;
  this._connection = connection;
}

Teams.prototype = {

  media: function () {
    var path = util.createPath('teams');
    return this._connection.get(path);
  },
}

module.exports = Teams;
