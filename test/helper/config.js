var _ = require('lodash'),
  env = process.env,
  defaults = {
    url: 'https://apis.hootsuite.com',
    clientId: 'someId',
    clientSecret: 'someSecret',
    username: 'someUsername',
    password: 'somePassword'
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
