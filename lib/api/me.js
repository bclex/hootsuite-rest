var _ = require('lodash'),
  Promise = require('bluebird'),
  util = require('../util'),
  log = util.logger();

function Me(hootsuite, connection) {
  this._hootsuite = hootsuite;
  this._connection = connection;
}

Me.prototype = {
  get: function () {
    var path = util.createPath('me');
    return this._connection.get(path, { _method: 'GET' });
  },

  getOrganizations: function () {
    var path = util.createPath('me', 'organizations');
    return this._connection.get(path, { _method: 'GET' });
  },

  getSocialProfiles: function () {
    var path = util.createPath('me', 'socialProfiles');
    return this._connection.get(path, { _method: 'GET' });
  },
}

module.exports = Me;
