var _ = require('lodash'),
  Promise = require('bluebird'),
  util = require('../util'),
  log = util.logger();

function Media(hootsuite, connection) {
  this._hootsuite = hootsuite;
  this._connection = connection;
}

Media.prototype = {
  createUrl: function (sizeBytes, mimeType) {
    var path = util.createPath('media');
    var data = {
      sizeBytes: sizeBytes || null,
      mimeType: mimeType || 'video/mp4',
    };
    return this._connection.postJson(path, data);
  },
  
  statusById: function (mediaId) {
    var path = util.createPath('media', mediaId);
    return this._connection.get(path, { _method: 'GET' });
  }
}

module.exports = Media;
