var _ = require('lodash'),
  Promise = require('bluebird'),
  util = require('../util'),
  log = util.logger();

function SocialProfiles(hootsuite, connection) {
  this._hootsuite = hootsuite;
  this._connection = connection;
}

SocialProfiles.prototype = {
  all: function () {
    var path = util.createPath('socialProfiles');
    return this._connection.get(path, { _method: 'GET' });
  },
  findById: function (socialProfileId) {
    var path = util.createPath('socialProfiles', socialProfileId);
    return this._connection.get(path, { _method: 'GET' });
  },
  findByIdTeams: function (socialProfileId) {
    var path = util.createPath('socialProfiles', socialProfileId, 'teams');
    return this._connection.get(path, { _method: 'GET' });
  },
}

module.exports = SocialProfiles;
