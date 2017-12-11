var assert = require('assert'),
  _ = require('lodash'),
  hootsuite = require('./helper/connection');

describe('Media', function () {
  describe('#createUrl', function () {
    it('Create media upload url.', function (done) {
      hootsuite.media.createUrl(100000).then(function (response) {
        console.log('1', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#statusById', function () {
    it('Retrieves the status of a media upload to Hootsuite.', function (done) {
      hootsuite.media.statusById('aHR0cHM6Ly9ob290c3VpdGUtdmlkZW8uczMuYW1hem9uYXdzLmNvbS9wcm9kdWN0aW9uLzE2NDk0ODc5X2E2Yzg4YzY0LTIwYmEtNGE0My05NDlmLTI5N2EzMmM2MDlmMi5tcDQ=').then(function (response) {
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
