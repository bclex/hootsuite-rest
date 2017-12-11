var assert = require('assert'),
  _ = require('lodash'),
  hootsuite = require('./helper/connection');

describe('Organizations', function () {
  describe('#findMembers', function () {
    it('Retrieves the members in an organization.', function (done) {
      hootsuite.organizations.findMembers('1234').then(function (response) {
        console.log('1', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#removeMemberById', function () {
    it('Removes a member from an organization.', function (done) {
      hootsuite.organizations.removeMemberById('1234', '1234').then(function (response) {
        console.log('2', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#findMemberByIdPermissions', function () {
    it('Retrieves an organization member’s permissions for the organization.', function (done) {
      hootsuite.organizations.findMemberByIdPermissions('1234', '1234').then(function (response) {
        console.log('1', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#findMemberByIdTeams', function () {
    it('Retrieves the teams an organization member is in.', function (done) {
      hootsuite.organizations.findMemberByIdTeams('1234', '1234').then(function (response) {
        console.log('1', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#findMemberByIdSocialProfiles', function () {
    it('Retrieves the organization’s social profiles that an organization member can access.', function (done) {
      hootsuite.organizations.findMemberByIdSocialProfiles('1234', '1234').then(function (response) {
        console.log('1', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#findSocialProfilesByIdPermissions', function () {
    it('Retrieves an organization member’s permissions for a social profile.', function (done) {
      hootsuite.organizations.findSocialProfilesByIdPermissions('1234', '1234', '1234').then(function (response) {
        console.log('1', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
});
