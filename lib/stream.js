var _ = require('lodash'),
  Promise = require('bluebird'),
  Readable = require('stream').Readable,
  util = require('util');

function HootsuiteStream(resultPromise, options) {
  options = _.defaults(options || {}, { objectMode: true });
  Readable.call(this, options);
  this._setData(resultPromise || Promise.resolve({}));
}
util.inherits(HootsuiteStream, Readable);

HootsuiteStream.prototype._setData = function (dataPromise) {
  this._ready = false;
  var self = this;
  this._dataPromise = dataPromise;
  return dataPromise.then(
    function (data) {
      if (data.result && data.result.length > 0) {
        self._data = data;
        self._resultIndex = 0;
        self._ready = true;
        self._pushNext();
      }
      else self.push(null);
    },
    function (err) {
      self.emit('error', err);
      self.push(null);
    });
};

HootsuiteStream.prototype._pushNext = function () {
  var result = this._data.result;
  if (!result) {
    return false;
  }
  if (this._resultIndex < result.length) {
    var record = result[this._resultIndex++]
    this.push(record);
    return true;
  }
};

HootsuiteStream.prototype._read = function () {
  if (!this._ready) {
    return;
  }
  if (this._data.result) {
    if (this._pushNext()) return;
    else if (this._data.moreResult || this._data.nextPageToken) this._setData(this._data.nextPage())
    else this.push(null);
  }
  else this.push(null);
};

HootsuiteStream.prototype.endStream = function () {
  this.push(null);
}

module.exports = HootsuiteStream;
