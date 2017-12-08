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
    return this._connection.post(path);
  },
  get: function (mediaId) {
    var path = util.createPath('media', mediaId);
    return this._connection.get(path);
  }
}

module.exports = Media;
