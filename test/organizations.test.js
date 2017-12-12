var assert = require('assert'),
  _ = require('lodash'),
  hootsuite = require('./helper/connection');

describe('Organizations', function () {
  describe('#findMembers', function () {
    it('Retrieves the members in an organization', function (done) {
      hootsuite.organizations.findMembers('814984').then(function (response) {
        console.log(response);
        // assert.equal(response.data.length, 1);
        // assert.equal(response.data[0].id, '1001');
        // assert(_.has(response.data[0], 'name'));
        // assert(_.has(response.data[0], 'workspaceName'));
        done();
      }).catch(done);
    });
  });
  describe('#removeMemberById', function () {
    it('Removes a member from an organization', function (done) {
      hootsuite.organizations.removeMemberById('814984', '1234').then(function (response) {
        console.log(response);
        // assert.equal(response.data.length, 1);
        // assert.equal(response.data[0].id, '1001');
        // assert(_.has(response.data[0], 'name'));
        // assert(_.has(response.data[0], 'workspaceName'));
        done();
      }).catch(done);
    });
  });
  describe('#findMemberByIdPermissions', function () {
    it('Retrieves an organization member’s permissions for the organization', function (done) {
      hootsuite.organizations.findMemberByIdPermissions('814984', '1234').then(function (response) {
        console.log(response);
        // assert.equal(response.data.length, 1);
        // assert.equal(response.data[0].id, '1001');
        // assert(_.has(response.data[0], 'name'));
        // assert(_.has(response.data[0], 'workspaceName'));
        done();
      }).catch(done);
    });
  });
  describe('#findMemberByIdTeams', function () {
    it('Retrieves the teams an organization member is in', function (done) {
      hootsuite.organizations.findMemberByIdTeams('814984', '1234').then(function (response) {
        console.log(response);
        // assert.equal(response.data.length, 1);
        // assert.equal(response.data[0].id, '1001');
        // assert(_.has(response.data[0], 'name'));
        // assert(_.has(response.data[0], 'workspaceName'));
        done();
      }).catch(done);
    });
  });
  describe('#findMemberByIdSocialProfiles', function () {
    it('Retrieves the organization’s social profiles that an organization member can access', function (done) {
      hootsuite.organizations.findMemberByIdSocialProfiles('814984', '1234').then(function (response) {
        console.log(response);
        // assert.equal(response.data.length, 1);
        // assert.equal(response.data[0].id, '1001');
        // assert(_.has(response.data[0], 'name'));
        // assert(_.has(response.data[0], 'workspaceName'));
        done();
      }).catch(done);
    });
  });
  describe('#findSocialProfilesByIdPermissions', function () {
    it('Retrieves an organization member’s permissions for a social profile', function (done) {
      hootsuite.organizations.findSocialProfilesByIdPermissions('814984', '1234', '1234').then(function (response) {
        console.log(response);
        // assert.equal(response.data.length, 1);
        // assert.equal(response.data[0].id, '1001');
        // assert(_.has(response.data[0], 'name'));
        // assert(_.has(response.data[0], 'workspaceName'));
        done();
      }).catch(done);
    });
  });
});
