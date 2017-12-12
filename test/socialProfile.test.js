var assert = require('assert'),
  _ = require('lodash'),
  hootsuite = require('./helper/connection');

describe('Social profiles', function () {
  describe('#find', function () {
    it('Retrieves the social profiles that the authenticated user has access to', function (done) {
      hootsuite.socialProfiles.find().then(function (response) {
        assert.equal(response.data.length, 1);
        assert.equal(response.data[0].id, '120732387');
        assert(_.has(response.data[0], 'type'));
        assert(_.has(response.data[0], 'socialNetworkId'));
        done();
      }).catch(done);
    });
  });
  describe('#findById', function () {
    it('Retrieve a social profile. Requires BASIC_USAGE permission on the social profile', function (done) {
      hootsuite.socialProfiles.findById('120732387').then(function (response) {
        assert.equal(response.data.id, '120732387');
        assert(_.has(response.data, 'type'));
        assert(_.has(response.data, 'socialNetworkId'));
        done();
      }).catch(done);
    });
  });
  describe('#findByIdTeams', function () {
    it('Retrieves a list of team IDs with access to a social profile', function (done) {
      hootsuite.socialProfiles.findByIdTeams('120732387').then(function (response) {
        assert.equal(response.data.length, 0);
        done();
      }).catch(done);
    });
  });
});
