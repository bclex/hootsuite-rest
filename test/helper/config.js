var _ = require('lodash'),
  env = process.env,
  defaults = {
    url: 'https://apis.hootsuite.com',
    // clientId: 'someId',
    // clientSecret: 'someSecret'
    clientId: 'l7xx90af3507c77c43dab3c9a2178782b7da',
    clientSecret: 'dea66393dda148b0addca63a25f061af',
    username: 'smorey@degdigital.com',
    password: 'Deg@dminZ1a'
  },
  credentials;

credentials = {
  url: env.HOOTSUITE_URL || defaults.url,
  clientId: env.HOOTSUITE_CLIENTID || defaults.clientId,
  clientSecret: env.HOOTSUITE_CLIENTSECRET || defaults.clientSecret,
  username: env.HOOTSUITE_USERNAME || defaults.username,
  password: env.HOOTSUITE_PASSWORD || defaults.password
};

module.exports = {
  creds: {
    defaults: defaults,
    computed: credentials
  }
};
