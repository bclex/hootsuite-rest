var _ = require('lodash'),
  Promise = require('bluebird'),
  util = require('../util'),
  log = util.logger();

function Organizations(hootsuite, connection) {
  this._hootsuite = hootsuite;
  this._connection = connection;
}

Organizations.prototype = {
  findMembers: function (organizationId) {
    var path = util.createPath('organizations', organizationId, 'members');
    return this._connection.get(path, { _method: 'GET' });
  },
  removeMemberById: function (organizationId, memberId) {
    var path = util.createPath('organizations', organizationId, 'members', memberId);
    return this._connection.del(path, { _method: 'DELETE' });
  },
  findMemberByIdPermissions: function (organizationId, memberId) {
    var path = util.createPath('organizations', organizationId, 'members', memberId, 'permissions');
    return this._connection.get(path, { _method: 'GET' });
  },
  findMemberByIdTeams: function (organizationId, memberId) {
    var path = util.createPath('organizations', organizationId, 'members', memberId, 'teams');
    return this._connection.get(path, { _method: 'GET' });
  },
  findMemberByIdSocialProfiles: function (organizationId, memberId) {
    var path = util.createPath('organizations', organizationId, 'members', memberId, 'socialProfiles');
    return this._connection.get(path, { _method: 'GET' });
  },
  findSocialProfilesByIdPermissions: function (organizationId, memberId, socialProfileId) {
    var path = util.createPath('organizations', organizationId, 'members', memberId, 'socialProfiles', socialProfileId, 'permissions');
    return this._connection.get(path, { _method: 'GET' });
  },
}

module.exports = Organizations;
