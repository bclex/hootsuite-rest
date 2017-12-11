var _ = require('lodash'),
  Promise = require('bluebird'),
  util = require('../util'),
  log = util.logger();

function Scim(hootsuite, connection) {
  this._hootsuite = hootsuite;
  this._connection = connection;
}

Scim.prototype = {
  createUser: function (msg) {
    var path = util.createScimPath('Users');
    var data = {
      schemas: msg.schemas || undefined,
      userName: msg.userName || undefined,
      name: msg.name || undefined,
      emails: msg.emails || undefined,
      displayName: msg.displayName || undefined,
      timezone: msg.timezone || undefined,
      preferredLanguage: msg.preferredLanguage || undefined,
      groups: msg.groups || undefined,
      active: msg.active || true,
      'urn:ietf:params:scim:schemas:extension:Hootsuite:2.0:User': msg['urn:ietf:params:scim:schemas:extension:Hootsuite:2.0:User'] || undefined
    };
    return this._connection.postJson(path, data);
  },
  findUsers: function (filter, count, startIndex) {
    var path = util.createScimPath('Users');
    var options = {
      query: {
        filter: filter || undefined,
        count: count || undefined,
        startIndex: startIndex || undefined,
      },
      _method: 'GET'
    };
    return this._connection.get(path, options);
  },
  findUserById: function (memberId) {
    var path = util.createScimPath('Users', memberId);
    return this._connection.get(path, { _method: 'GET' });
  },
  replaceUserById: function (memberId, msg) {
    var path = util.createScimPath('Users', memberId);
    var data = {
      schemas: msg.schemas || undefined,
      userName: msg.userName || undefined,
      name: msg.name || undefined,
      emails: msg.emails || undefined,
      displayName: msg.displayName || undefined,
      timezone: msg.timezone || undefined,
      preferredLanguage: msg.preferredLanguage || undefined,
      groups: msg.groups || undefined,
      active: msg.active || true,
      'urn:ietf:params:scim:schemas:extension:Hootsuite:2.0:User': msg['urn:ietf:params:scim:schemas:extension:Hootsuite:2.0:User'] || undefined
    };
    return this._connection.putJson(path, data);
  },
  modifyUserById: function (memberId, msg) {
    var path = util.createScimPath('Users', memberId);
    var options = {
      headers: { 'content-type': 'application/json' },
      data: JSON.stringify({
        schemas: msg.schemas || undefined,
        Operations: msg.Operations || undefined,
      }),
      _method: 'PATCH'
    };
    return this._connection.patch(path, options);
  },
  getResourceTypes: function () {
    var path = util.createScimPath('ResourceTypes');
    return this._connection.get(path, { _method: 'GET' });
  },
}

module.exports = Scim;
