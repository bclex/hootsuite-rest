using Hootsuite.Rest.Require;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Hootsuite.Rest
{
    public class ConnectionOwly
    {
        readonly dynamic _options;
        readonly Action<string> _log = util.logger;
        readonly Restler _rest = new Restler();

        public ConnectionOwly(dynamic options)
        {
            _options = options ?? new { };
        }

        public Task<JObject> get(string url, dynamic options = null) => _rest.get(url, options);
        public Task<JObject> post(string url, dynamic options = null) => _rest.post(url, options);
        public Task<JObject> put(string url, dynamic options = null) => _rest.put(url, options);
        public Task<JObject> del(string url, dynamic options = null) => _rest.del(url, options);
        public Task<JObject> head(string url, dynamic options = null) => _rest.head(url, options);
        public Task<JObject> patch(string url, dynamic options = null) => _rest.patch(url, options);
        public Task<JObject> json(string url, object data, dynamic options = null) => _rest.json(url, data, options);
        public Task<JObject> postJson(string url, object data, dynamic options = null) => _rest.postJson(url, data, options);
        public Task<JObject> putJson(string url, object data, dynamic options = null) => _rest.putJson(url, data, options);
    }
}
