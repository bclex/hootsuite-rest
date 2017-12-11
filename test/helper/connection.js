var _ = require('lodash'),
    Replay = require('replay'),
    config = require('./config'),
    Hootsuite = require('../../lib/hootsuite');

// Remove authorization from the header comparison
Replay.headers = _.filter(Replay.headers, function(header) {
  console.log(header);
  return !(header.toString() === '/^authorization/');
});

module.exports = new Hootsuite(config.creds.computed);
