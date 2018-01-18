var _ = require('lodash'),
  util = require('util'),
  EventEmitter = require('events').EventEmitter,
  Connection = require('./connection'),
  ConnectionOwly = require('./connectionOwly'),
  Me = require('./api/me'),
  Media = require('./api/media'),
  Members = require('./api/members'),
  Messages = require('./api/messages'),
  Organizations = require('./api/organizations'),
  Scim = require('./api/scim'),
  SocialProfiles = require('./api/socialProfiles'),
  Teams = require('./api/teams'),
  Owly = require('./api/owly'),
  HootsuiteStream = require('./stream');

function Hootsuite(options) {
  EventEmitter.call(this);

  var self = this;
  this._connection = new Connection(options)
    .on('apiCall', function (data) {
      self.emit('apiCall', data);
    });
  this.apiCallCount = this._connection.apiCallCount;
  this._connectionOwly = new ConnectionOwly(options);
  //
  this.me = new Me(this, this._connection);
  this.media = new Media(this, this._connection);
  this.members = new Members(this, this._connection);
  this.messages = new Messages(this, this._connection);
  this.organizations = new Organizations(this, this._connection);
  this.scim = new Scim(this, this._connection);
  this.socialProfiles = new SocialProfiles(this, this._connection);
  this.teams = new Teams(this, this._connection);
  this.owly = new Owly(this, this._connectionOwly);
}
util.inherits(Hootsuite, EventEmitter);

Hootsuite.prototype.getFrameAuthToken = function getFrameAuthToken() {
  return this._connection.getFrameAuthToken(url);
};

Hootsuite.prototype.getOAuthToken = function oauthToken() {
  return this._connection.getOAuthToken(true);
};

Hootsuite.streamify = function streamify(resultPromise) {
  return new HootsuiteStream(resultPromise);
};

Hootsuite.about = function about() {
  return { version: '0.1.7' };
}

module.exports = Hootsuite;
