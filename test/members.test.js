var assert = require('assert'),
  _ = require('lodash'),
  hootsuite = require('./helper/connection');

describe('Members', function () {
  describe('#findById', function () {
    it('Retrieves a member', function (done) {
      hootsuite.members.findById('16494879').then(function (res) {
        console.log(res);
        // assert.equal(res.result.length, 1);
        // assert.equal(res.result[0].id, 1001);
        // assert(_.has(res.result[0], 'name'));
        // assert(_.has(res.result[0], 'workspaceName'));
        done();
      }).catch(done);
    });
  });
  describe('#create', function () {
    it('Creates a member in a Hootsuite organization. Requires organization manage members permission', function (done) {
      hootsuite.members.create({
        organizationIds: ['814984'],
        email: 'test@test.com',
        fullName: 'test full'
      }).then(function (res) {
        console.log('2', res);
        // assert.equal(res.result.length, 1);
        // assert.equal(res.result[0].id, 1001);
        // assert(_.has(res.result[0], 'name'));
        // assert(_.has(res.result[0], 'workspaceName'));
        done();
      }).catch(done);
    });
  });
  describe('#findByIdOrgs', function () {
    it('Retrieves the organizations that the member is in', function (done) {
      hootsuite.members.findByIdOrgs('16494879').then(function (res) {
        assert.equal(res.data.length > 0);
        assert.equal(res.data[0].id, '814984');
        assert(_.has(res.data[0], 'id'));
        done();
      }).catch(done);
    });
  });
});
