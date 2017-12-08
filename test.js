let Hootsuite = require('./lib/hootsuite.js');

let hootsuite = new Hootsuite({
});

// hootsuite.me.get().then(x => {
//     console.log('me', x);
// }).catch(console.log);


// hootsuite.me.getSocialProfiles().then(x => {
//     console.log('getSocialProfiles', x);
// }).catch(console.log);

hootsuite.socialProfiles.all(1).then(x => {
    console.log('get', x);
}).catch(console.log);
