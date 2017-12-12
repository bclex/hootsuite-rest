var _ = require('lodash'),
  Promise = require('bluebird'),
  util = require('../util'),
  log = util.logger();

function Members(hootsuite, connection) {
  this._hootsuite = hootsuite;
  this._connection = connection;
}

Members.prototype = {
  findById: function (memberId) {
    var path = util.createPath('members', memberId);
    return this._connection.get(path, { _method: 'GET' });
  },
  create: function (msg) {
    var path = util.createPath('members');
    var data = {
      organizationIds: msg.organizationIds || [],
      email: msg.email || null,
      fullName: msg.fullName || undefined,
      companyName: msg.companyName || undefined,
      bio: msg.bio || undefined,
      timezone: msg.timezone || undefined,
      language: msg.language || undefined,
    };
    return this._connection.postJson(path, data);
  },
  findByIdOrgs: function (memberId) {
    var path = util.createPath('members', memberId, 'organizations');
    return this._connection.get(path, { _method: 'GET' });
  },
}

module.exports = Members;
