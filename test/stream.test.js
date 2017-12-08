var _ = require('lodash'),
  Hootsuite = require('../lib/hootsuite'),
  Promise = require('bluebird'),
  assert = require('assert');

function getResultPromise(count, hasNext, opt_isError) {
  var result = { result: _.range(count) };
  if (hasNext) {
    result.nextPageToken = 'some_token';
    result.nextPage = function () {
      return getResultPromise(count, false);
    };
  }
  if (opt_isError) {
    result.success = false;
    result.errors = [{
      message: 'error',
      code: '605'
    }];
  } else {
    result.success = true;
  }
  if (opt_isError) return Promise.reject(result);
  else return Promise.resolve(result);
}

describe('Stream', function () {
  it('streams normal result without pagination', function (done) {
    var EXPECTED_COUNT = 10,
      count = 0;
    Hootsuite.streamify(getResultPromise(EXPECTED_COUNT, false))
      .on('data', function () {
        ++count;
      })
      .on('end', function () {
        assert.equal(count, EXPECTED_COUNT);
        done();
      });
  });

  it('streams result with pagination', function (done) {
    var PAGE_COUNT = 10,
      count = 0;
    Hootsuite.streamify(getResultPromise(PAGE_COUNT, true))
      .on('data', function () {
        ++count;
      })
      .on('end', function () {
        assert.equal(count, PAGE_COUNT * 2);
        done();
      });
  });

  it('ends the stream if there is an error', function (done) {
    var count = 0;
    Hootsuite.streamify(getResultPromise(10, true, true))
      .on('data', function () {
        assert(false, 'We should not get data on error');
      })
      .on('error', function (err) {
        assert.notEqual(err.errors.length, 0);
      })
      .on('end', function () {
        assert.equal(count, 0);
        done();
      });
  });
});
