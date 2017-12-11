var assert = require('assert'),
  _ = require('lodash'),
  hootsuite = require('./helper/connection');

describe('Members', function () {
  describe('#findById', function () {
    it('Retrieves a member.', function (done) {
      hootsuite.members.findById('16494879').then(function (response) {
        console.log('1', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#create', function () {
    it('Creates a member in a Hootsuite organization. Requires organization manage members permission.', function (done) {
      hootsuite.members.create({}).then(function (response) {
        console.log('2', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#findByIdOrgs', function () {
    it('Retrieves the organizations that the member is in.', function (done) {
      hootsuite.members.findByIdOrgs('16494879').then(function (response) {
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
