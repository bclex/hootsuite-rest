var assert = require('assert'),
  _ = require('lodash'),
  hootsuite = require('./helper/connection');

describe('Current User', function () {
  describe('#get', function () {
    it('Retrieves authenticated member.', function (done) {
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
    it('Retrieves the organizations that the authenticated member is in.', function (done) {
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
    it('Retrieves the social media profiles that the authenticated user has BASIC_USAGE permissions on.', function (done) {
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
