var _ = require('lodash'),
  env = process.env,
  defaults = {
    url: 'https://apis.hootsuite.com',
    clientId: 'someId',
    clientSecret: 'someSecret',
    username: 'someUsername',
    password: 'somePassword'
  },
  computed;

computed = {
  url: env.HOOTSUITE_URL || defaults.url,
  clientId: env.HOOTSUITE_CLIENTID || defaults.clientId,
  clientSecret: env.HOOTSUITE_CLIENTSECRET || defaults.clientSecret,
  username: env.HOOTSUITE_USERNAME || defaults.username,
  password: env.HOOTSUITE_PASSWORD || defaults.password
};

frameComputed = {
  url: env.HOOTSUITE_URL || defaults.url,
  frameCtx: { uid: '16494879' },
  clientId: env.HOOTSUITE_CLIENTID || defaults.clientId,
  clientSecret: env.HOOTSUITE_CLIENTSECRET || defaults.clientSecret,
};

module.exports = {
  creds: {
    defaults: defaults,
    computed: computed,
    frameComputed: frameComputed,
  }
};
