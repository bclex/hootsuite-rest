var _ = require('lodash'),
  Promise = require('bluebird'),
  assert = require('assert'),
  errors = require('../lib/errors'),
  Connection = require('../lib/connection'),
  testServer = require('./helper/server');

function getUrl(server, path) {
  return 'http://localhost:' + server.http().address().port + path;
}

function getConnection(server, retry) {
  var options = {
    retry: retry || {
      maxRetries: 3,
      initialDelay: 5,
      maxDelay: 10,
    },
    url: getUrl(server, ''),
    clientId: 'someId',
    clientSecret: 'someSecret'
  };
  return new Connection(options);
}

describe('Retry logic', function () {
  var server;
  beforeEach(function (done) {
    server = testServer.newServer(done);
  });

  it('retries on closed connection', function (done) {
    var connection = getConnection(server),
      path = '/some_path';
    connection.get(path, { headers: { 'connection-abort': 'true' } })
      .catch(function (err) {
        // exhausted retries, the last error is returned
        assert.equal(err.code, 'ECONNRESET');
        assert.equal(server.count(path), 4);
        done();
      })
  });

  it('retries on 5XX errors', function (done) {
    var connection = getConnection(server),
      path = '/some_path';
    connection.get(path, { headers: { 'status-code': 501 } })
      .catch(function (err) {
        assert.equal(err.code, '5XX');
        assert.equal(server.count(path), 4);
        done();
      })
  });

  it('does not retry on unauthorized, or quota reached', function (done) {
    var connection = getConnection(server),
      p1 = '/p1',
      p2 = '/p2';
    connection.get(p1, { headers: { 'hootsuite-error-code': errors.errorCodes.ACCESS_DENIED } })
      .catch(function (err) {
        assert(errors.hasErrorCode(err, errors.errorCodes.ACCESS_DENIED))
        assert.equal(server.count(p1), 1);
      })
      .then(function () {
        return connection.get(p2, {
          headers: { 'hootsuite-error-code': errors.errorCodes.QUOTA_REACHED }
        })
      })
      .catch(function (err) {
        assert(errors.hasErrorCode(err, errors.errorCodes.QUOTA_REACHED))
        assert.equal(server.count(p2), 1);
        done();
      })
  });

  it('tries to get a new oauth token when expired', function (done) {
    var connection = getConnection(server),
      path = '/path';
    connection.get(path, { headers: { 'hootsuite-error-code': errors.errorCodes.TOKEN_EXPIRED } })
      .catch(function (err) {
        assert(errors.hasErrorCode(err, errors.errorCodes.TOKEN_EXPIRED))
        assert.equal(server.count(path), 4);
        assert.equal(server.countPattern('^/identity'), 4);
        done();
      })
  });

  it('drops to max delay on rate limit error', function (done) {
    var connection = getConnection(server, {
      maxRetries: 2,
      initialDelay: 1,
      maxDelay: 50
    }),
      path = '/path';
    connection.get(path, { headers: { 'hootsuite-error-code': errors.errorCodes.RATE_LIMIT_REACHED } })
      .catch(function (err) {
        assert(errors.hasErrorCode(err, errors.errorCodes.RATE_LIMIT_REACHED))
        assert.equal(server.count(path), 3);
        done();
      });
    setTimeout(function () {
      assert.equal(server.count(path), 1);
    }, 50);
  });

});
