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
      data: {
        text: msg.text,
        socialProfileIds: msg.socialProfileIds,
        scheduledSendTime: msg.scheduledSendTime,
        webhookUrls: msg.webhookUrls,
        tags: msg.tags,
        targeting: msg.targeting,
        privacy: msg.privacy,
        location: msg.location,
        emailNotification: msg.emailNotification || false,
        mediaUrls: msg.mediaUrls,
        media: msg.media,
      },
    };
    return this._connection.post(path, data);
  },
}

module.exports = Messages;
