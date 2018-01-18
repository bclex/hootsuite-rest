var _ = require('lodash'),
  Promise = require('bluebird'),
  util = require('../util'),
  log = util.logger();

function Owly(hootsuite, connection) {
  this._hootsuite = hootsuite;
  this._connection = connection;
}

Owly.prototype = {
  shortenUrl: function (longUrl) {
    var path = util.createOwlyPath('url', 'shorten');
    return this._connection.get(path, { longUrl, _method: 'GET' });
  },

  expandUrl: function (shortUrl) {
    var path = util.createPath('url', 'expand');
    return this._connection.get(path, { shortUrl, _method: 'GET' });
  },

  getInfo: function (shortUrl) {
    var path = util.createPath('url', 'info');
    return this._connection.get(path, { shortUrl, _method: 'GET' });
  },

  getClickStats: function (shortUrl, from, to) {
    var path = util.createPath('url', 'clickStats');
    return this._connection.get(path, { shortUrl, from, to, _method: 'GET' });
  },

  uploadPhoto: function (fileName, uploaded_file) {
    var path = util.createPath('photo', 'upload');
    return this._connection.post(path, { fileName, uploaded_file, _method: 'POST' });
  },

  uploadDoc: function (fileName, uploaded_file) {
    var path = util.createPath('doc', 'upload');
    return this._connection.post(path, { fileName, uploaded_file, _method: 'POST' });
  },
}

module.exports = Owly;
