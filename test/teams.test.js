var assert = require('assert'),
  _ = require('lodash'),
  hootsuite = require('./helper/connection');

describe('Teams', function () {
  describe('#createTeam', function () {
    it('Creates a team in an organization.', function (done) {
      hootsuite.teams.createTeam('name').then(function (response) {
        console.log('1', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#appendMemberById', function () {
    it('Adds a member to a team.', function (done) {
      hootsuite.teams.appendMemberById('1234', '1234', '1234').then(function (response) {
        console.log('2', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#findByIdMembers', function () {
    it('Retrieves the members in a team.', function (done) {
      hootsuite.teams.findByIdMembers('1234', '1234').then(function (response) {
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
    it('Retrieves team member’s team permissions.', function (done) {
      hootsuite.teams.findMemberByIdPermissions('1234', '1234', '1234').then(function (response) {
        console.log('2', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#findMemberByIdSocialProfiles', function () {
    it('Retrieves the organization’s social profiles that an organization team can access.', function (done) {
      hootsuite.teams.findMemberByIdSocialProfiles('1234', '1234', '1234').then(function (response) {
        console.log('2', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
});
