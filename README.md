# hootsuite-rest

A node client for Hootsuite's REST API.

## Usage

### Installation

```
npm install hootsuite-rest --save
```

### Creating a connection

You will first need to obtain your OAuth information from Hootstuite, they have a [guide out](https://app-directory.s3.amazonaws.com/docs/api/index.html) to get you started. In short, you will need to get the client id and client secret.

```js
var Hootsuite = require('hootsuite-rest');

var hootsuite = new Hootsuite({
  clientId: 'client id',
  clientSecret: 'client secret'
});

hootsuite.me.get()
  .then(function(data) {
  });
```
