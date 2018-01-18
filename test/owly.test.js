var assert = require('assert'),
  _ = require('lodash'),
  hootsuite = require('./helper/connection');

describe('Ow.ly', function () {
  describe('#shortenUrl', function () {
    it('given a full URL, returns an ow.ly short URL', function (done) {
      hootsuite.owly.shortenUrl('http://www.google.com').then(function (response) {
        assert.equal(response.data.id, '16494879');
        assert(_.has(response.data, 'fullName'));
        assert(_.has(response.data, 'language'));
        done();
      }).catch(done);
    });
  });
  describe('#expandUrl', function () {
    it('given an ow.ly URL, returns the original full URL', function (done) {
      hootsuite.owly.expandUrl('http://www.google.com').then(function (response) {
        assert.equal(response.data.length, 1);
        assert.equal(response.data[0].id, '814984');
        assert(_.has(response.data[0], 'id'));
        done();
      }).catch(done);
    });
  });
  describe('#getInfo', function () {
    it('given an ow.ly URL, returns information about the page, including the original URL, the HTML title, total clicks, and the "votes" value for the link (votes may be a positive or negative value)', function (done) {
      hootsuite.owly.getInfo('http://www.google.com').then(function (response) {
        // console.log(response);
        assert.equal(response.data.length, 1);
        assert.equal(response.data[0].id, '120732387');
        assert(_.has(response.data[0], 'id'));
        done();
      }).catch(done);
    });
  });
  describe('#clickStats', function () {
    it('given an ow.ly URL, returns an array of dates and the number of clicks on that date', function (done) {
      hootsuite.owly.getClickStats('http://www.google.com').then(function (response) {
        // console.log(response);
        assert.equal(response.data.length, 1);
        assert.equal(response.data[0].id, '120732387');
        assert(_.has(response.data[0], 'id'));
        done();
      }).catch(done);
    });
  });
  describe('#uploadPhoto', function () {
    it('upload a photo to ow.ly file hosting service', function (done) {
      hootsuite.owly.uploadPhoto('file', 'blob').then(function (response) {
        // console.log(response);
        assert.equal(response.data.length, 1);
        assert.equal(response.data[0].id, '120732387');
        assert(_.has(response.data[0], 'id'));
        done();
      }).catch(done);
    });
  });
  describe('#uploadDoc', function () {
    it('upload a document to ow.ly file hosting service', function (done) {
      hootsuite.owly.uploadDoc('file', 'blob').then(function (response) {
        // console.log(response);
        assert.equal(response.data.length, 1);
        assert.equal(response.data[0].id, '120732387');
        assert(_.has(response.data[0], 'id'));
        done();
      }).catch(done);
    });
  });
});
