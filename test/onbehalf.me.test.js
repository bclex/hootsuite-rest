var assert = require('assert'),
  _ = require('lodash'),
  hootsuite = require('./helper/frameConnection');

describe('Current User', function () {
  describe('#get', function () {
    it('Retrieves authenticated member', function (done) {
      hootsuite.me.get().then(function (response) {
        assert.equal(response.data.id, '16494879');
        assert(_.has(response.data, 'fullName'));
        assert(_.has(response.data, 'language'));
        done();
      }).catch(done);
    });
  });
  describe('#getOrganizations', function () {
    it('Retrieves the organizations that the authenticated member is in', function (done) {
      hootsuite.me.getOrganizations().then(function (response) {
        assert.equal(response.data.length, 1);
        assert.equal(response.data[0].id, '814984');
        assert(_.has(response.data[0], 'id'));
        done();
      }).catch(done);
    });
  });
  describe('#getSocialProfiles', function () {
    it('Retrieves the social media profiles that the authenticated user has BASIC_USAGE permissions on', function (done) {
      hootsuite.me.getSocialProfiles().then(function (response) {
        // console.log(response);
        assert.equal(response.data.length, 1);
        assert.equal(response.data[0].id, '120732387');
        assert(_.has(response.data[0], 'id'));
        done();
      }).catch(done);
    });
  });
});
