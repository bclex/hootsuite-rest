var assert = require('assert'),
  _ = require('lodash'),
  hootsuite = require('./helper/connection');

describe('SCIM', function () {
  describe('#createUser', function () {
    it('Creates a Hootsuite user using the SCIM 2.0 protocol.', function (done) {
      hootsuite.scim.createUser({}).then(function (response) {
        console.log('1', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#findUsers', function () {
    it('Retrieves Hootsuite users using the SCIM 2.0 protocol. Support equals filtering on username.', function (done) {
      hootsuite.scim.findUsers().then(function (response) {
        console.log('2', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#findUserById', function () {
    it('Retrieves a Hootsuite user using the SCIM 2.0 protocol.', function (done) {
      hootsuite.scim.findUserById('1234').then(function (response) {
        console.log('2', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#replaceUserById', function () {
    it('Updates a Hootsuite user using the SCIM 2.0 protocol.', function (done) {
      hootsuite.scim.replaceUserById('1234', {}).then(function (response) {
        console.log('2', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#modifyUserById', function () {
    it('Modify one or more attributes of a Hootsuite user.', function (done) {
      hootsuite.scim.modifyUserById('1234', {}).then(function (response) {
        console.log('2', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#getResourceTypes', function () {
    it('Retrieves the configuration for all supported SCIM resource types.', function (done) {
      hootsuite.scim.getResourceTypes().then(function (response) {
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
