var _ = require('lodash'),
  Promise = require('bluebird'),
  util = require('../util'),
  moment = require('moment'),
  qs = require('qs'),
  log = util.logger();

function Messages(hootsuite, connection) {
  this._hootsuite = hootsuite;
  this._connection = connection;
}

Messages.prototype = {
  schedule: function (msg) {
    var path = util.createPath('messages');
    var data = {
      text: msg.text || null,
      socialProfileIds: msg.socialProfileIds || [],
      scheduledSendTime: msg.scheduledSendTime ? moment(msg.scheduledSendTime).toISOString() : undefined,
      webhookUrls: msg.webhookUrls || undefined,
      tags: msg.tags || undefined,
      targeting: msg.targeting || undefined,
      privacy: msg.privacy || undefined,
      location: msg.location || undefined,
      emailNotification: msg.emailNotification || undefined,
      mediaUrls: msg.mediaUrls || undefined,
      media: msg.media || undefined,
    };
    return this._connection.postJson(path, data);
  },

  find: function (startTime, endTime, socialProfileIds, opts) {
    var path = util.createPath('messages');
    opts = opts || {};
    var query = qs.stringify({
      startTime: moment(startTime).toISOString(),
      endTime: moment(endTime).toISOString(),
      state: opts.state || undefined,
      limit: opts.limit || undefined,
      cursor: opts.cursor || undefined,
    });
    query += (!_.isArray(socialProfileIds) ? (socialProfileIds ? [socialProfileIds] : []) : socialProfileIds)
      .map(function (x) { return '&socialProfileIds=' + encodeURIComponent(x); }).join('');
    var options = {
      query: query,
      _method: 'GET'
    };
    return this._connection.get(path, options);
  },

  findById: function (messageId) {
    var path = util.createPath('messages', messageId);
    return this._connection.get(path, { _method: 'GET' });
  },

  deleteById: function (messageId) {
    var path = util.createPath('messages', messageId);
    return this._connection.del(path, { _method: 'DELETE' });
  },

  approveById: function (messageId, sequenceNumber, reviewerType) {
    var path = util.createPath('messages', messageId, 'approve');
    var data = {
      sequenceNumber: sequenceNumber || 0,
      reviewerType: reviewerType || undefined,
    };
    return this._connection.postJson(path, data);
  },

  rejectById: function (messageId, reason, sequenceNumber, reviewerType) {
    var path = util.createPath('messages', messageId, 'reject');
    var data = {
      reason: reason || null,
      sequenceNumber: sequenceNumber || 0,
      reviewerType: reviewerType || undefined,
    };
    return this._connection.postJson(path, data);
  },
  
  findByIdHistory: function (messageId) {
    var path = util.createPath('messages', messageId, 'history');
    return this._connection.get(path, { _method: 'GET' });
  },
}

module.exports = Messages;
