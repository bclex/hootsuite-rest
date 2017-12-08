var assert = require('assert'),
  _ = require('lodash'),
  hootsuite = require('./helper/connection');

describe('Me', function () {
  describe('#get', function () {
    it('returns the current user information', function (done) {
      hootsuite.me.get().then(function (response) {
        console.log('1', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#getOrganizations', function () {
    it('returns the current users organizations', function (done) {
      hootsuite.me.getOrganizations().then(function (response) {
        console.log('2', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#getSocialProfiles', function () {
    it('returns the current users social profiles', function (done) {
      hootsuite.me.getSocialProfiles().then(function (response) {
        console.log('3', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
});
