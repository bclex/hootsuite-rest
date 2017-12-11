var assert = require('assert'),
  _ = require('lodash'),
  hootsuite = require('./helper/connection');

describe('Messages', function () {
  describe('#schedule', function () {
    it('Schedules a message to send on one or more social profiles. Returns an array of uniquely identifiable messages (one per social profile requested).', function (done) {
      hootsuite.messages.schedule({ text: 'sample' }).then(function (response) {
        console.log('1', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#find', function () {
    it('Retrieve outbound messages.', function (done) {
      hootsuite.messages.find('2017-12-01', '2017-12-10').then(function (response) {
        console.log('2', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#findById', function () {
    it('Retrieves a message. A message is always associated with a single social profile. Messages might be unavailable for a brief time during upload to social networks.', function (done) {
      hootsuite.messages.findById('1234').then(function (response) {
        console.log('3', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#deleteById', function () {
    it('Deletes a message. A message is always associated with a single social profile.', function (done) {
      hootsuite.messages.deleteById('1234').then(function (response) {
        console.log('3', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#approveById', function () {
    it('Approve a message.', function (done) {
      hootsuite.messages.approveById('1234', 1).then(function (response) {
        console.log('3', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#rejectById', function () {
    it('Reject a message.', function (done) {
      hootsuite.messages.rejectById('1234', 'reason', 1).then(function (response) {
        console.log('3', response);
        // assert.equal(response.result.length, 1);
        // assert.equal(response.result[0].id, 1001);
        // assert(_.has(response.result[0], 'name'));
        // assert(_.has(response.result[0], 'workspaceName'));
        done();
      });
    });
  });
  describe('#findByIdHistory', function () {
    it('Gets a message’ prescreening review history.', function (done) {
      hootsuite.messages.findByIdHistory('1234').then(function (response) {
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
