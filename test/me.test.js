var assert = require('assert'),
  _ = require('lodash'),
  hootsuite = require('./helper/connection');

describe('Current User', function () {
  describe('#get', function () {
    it('Retrieves authenticated member', function (done) {
      hootsuite.me.get().then(function (res) {
        assert.equal(res.data.id, '16494879');
        assert(_.has(res.data, 'fullName'));
        assert(_.has(res.data, 'language'));
        done();
      }).catch(done);
    });
  });
  describe('#getOrganizations', function () {
    it('Retrieves the organizations that the authenticated member is in', function (done) {
      hootsuite.me.getOrganizations().then(function (res) {
        assert.equal(res.data.length, 1);
        assert.equal(res.data[0].id, '814984');
        assert(_.has(res.data[0], 'id'));
        done();
      }).catch(done);
    });
  });
  describe('#getSocialProfiles', function () {
    it('Retrieves the social media profiles that the authenticated user has BASIC_USAGE permissions on', function (done) {
      hootsuite.me.getSocialProfiles().then(function (res) {
        assert.equal(res.data.length, 1);
        assert.equal(res.data[0].id, '120732387');
        assert(_.has(res.data[0], 'id'));
        done();
      }).catch(done);
    });
  });
});
