var _ = require('lodash'),
  Promise = require('bluebird'),
  util = require('../util'),
  log = util.logger();

function Teams(hootsuite, connection) {
  this._hootsuite = hootsuite;
  this._connection = connection;
}

Teams.prototype = {
  createTeam: function (organizationId, teamName) {
    var path = util.createPath('organizations', organizationId, 'teams');
    var data = {
      teamName: teamName
    };
    return this._connection.postJson(path, data);
  },
  appendMemberById: function (organizationId, teamId, memberId) {
    var path = util.createPath('organizations', organizationId, 'teams', teamId, 'members', memberId);
    return this._connection.post(path, { _method: 'POST' });
  },
  findByIdMembers: function (organizationId, teamId) {
    var path = util.createPath('organizations', organizationId, 'teams', teamId, 'members');
    return this._connection.get(path, { _method: 'GET' });
  },
  findMemberByIdPermissions: function (organizationId, teamId, memberId) {
    var path = util.createPath('organizations', organizationId, 'teams', teamId, 'members', memberId, 'permissions');
    return this._connection.get(path, { _method: 'GET' });
  },
  findByIdSocialProfiles: function (organizationId, teamId) {
    var path = util.createPath('organizations', organizationId, 'teams', teamId, 'socialProfiles');
    return this._connection.get(path, { _method: 'GET' });
  },
}

module.exports = Teams;
