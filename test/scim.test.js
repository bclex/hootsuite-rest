var assert = require('assert'),
  _ = require('lodash'),
  hootsuite = require('./helper/connection');

describe('SCIM', function () {
  describe('#createUser', function () {
    it('Creates a Hootsuite user using the SCIM 2.0 protocol', function (done) {
      hootsuite.scim.createUser({
        schemas: [],
        userName: 'testUser',
        name: 'testUser',
        emails: 'testUser',
        'urn:ietf:params:scim:schemas:extension:Hootsuite:2.0:User': { organizationIds: [], teamIds: [] }
      }).then(function (response) {
        console.log(response);
        assert.equal(response.Resources.length, 1);
        assert.equal(response.Resources[0].id, '16494879');
        assert(_.has(response.Resources[0], 'name'));
        assert(_.has(response.Resources[0], 'displayName'));
        done();
      }).catch(x => { done(); throw(JSON.stringify(x)); });
    });
  });
  describe('#findUsers', function () {
    it('Retrieves Hootsuite users using the SCIM 2.0 protocol. Support equals filtering on username', function (done) {
      hootsuite.scim.findUsers().then(function (response) {
        assert.equal(response.Resources.length, 1);
        assert.equal(response.Resources[0].id, '16494879');
        assert(_.has(response.Resources[0], 'name'));
        assert(_.has(response.Resources[0], 'displayName'));
        done();
      }).catch(done);
    });
  });
  describe('#findUserById', function () {
    it('Retrieves a Hootsuite user using the SCIM 2.0 protocol', function (done) {
      hootsuite.scim.findUserById('16494879').then(function (response) {
        console.log(response);
        // assert.equal(response.data.length, 1);
        // assert.equal(response.data[0].id, 1001);
        // assert(_.has(response.data[0], 'name'));
        // assert(_.has(response.data[0], 'workspaceName'));
        done();
      }).catch(x => { done(); throw(JSON.stringify(x)); });
    });
  });
  describe('#replaceUserById', function () {
    it('Updates a Hootsuite user using the SCIM 2.0 protocol', function (done) {
      hootsuite.scim.replaceUserById('1234', {}).then(function (response) {
        console.log(response);
        // assert.equal(response.data.length, 1);
        // assert.equal(response.data[0].id, 1001);
        // assert(_.has(response.data[0], 'name'));
        // assert(_.has(response.data[0], 'workspaceName'));
        done();
      }).catch(x => { done(); throw(JSON.stringify(x)); });
    });
  });
  describe('#modifyUserById', function () {
    it('Modify one or more attributes of a Hootsuite user', function (done) {
      hootsuite.scim.modifyUserById('1234', {}).then(function (response) {
        console.log(response);
        // assert.equal(response.data.length, 1);
        // assert.equal(response.data[0].id, 1001);
        // assert(_.has(response.data[0], 'name'));
        // assert(_.has(response.data[0], 'workspaceName'));
        done();
      }).catch(x => { done(); throw(JSON.stringify(x)); });
    });
  });
  describe('#getResourceTypes', function () {
    it('Retrieves the configuration for all supported SCIM resource types', function (done) {
      hootsuite.scim.getResourceTypes().then(function (response) {
        assert.equal(response.Resources.length, 1);
        assert.equal(response.Resources[0].id, 'User');
        assert(_.has(response.Resources[0], 'name'));
        assert(_.has(response.Resources[0], 'meta'));
        done();
      }).catch(done);
    });
  });
});
