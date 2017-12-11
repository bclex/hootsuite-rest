var _ = require('lodash'),
  Promise = require('bluebird'),
  util = require('../util'),
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
      scheduledSendTime: msg.scheduledSendTime || null,
      webhookUrls: msg.webhookUrls || [],
      tags: msg.tags || [],
      targeting: msg.targeting || null,
      privacy: msg.privacy || null,
      location: msg.location || null,
      emailNotification: msg.emailNotification || false,
      mediaUrls: msg.mediaUrls || [],
      media: msg.media || [],
    };
    return this._connection.postJson(path, data);
  },
}

module.exports = Messages;
