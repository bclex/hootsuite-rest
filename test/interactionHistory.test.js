var assert = require('assert'),
  _ = require('lodash'),
  hootsuite = require('./helper/connection');

describe('Interaction History', function () {
  describe('#get', function () {
    it('Retrieves the interactions between two users of a particular social network type such as direct messages, comments, and posts', function (done) {
      hootsuite.interactionHistory.get('twitter', '@handle').then(function (res) {
        console.log(res);
        // assert.equal(res.data.length, 1);
        // assert.equal(res.data[0].id, 1001);
        // assert(_.has(res.data[0], 'name'));
        // assert(_.has(res.data[0], 'workspaceName'));
        done();
      }).catch(done);
    });
  });
});
