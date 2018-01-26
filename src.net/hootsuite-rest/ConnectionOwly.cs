using Hootsuite.Rest.Require;
using Newtonsoft.Json.Linq;

namespace Hootsuite.Rest
{
    public class ConnectionOwly
    {
        readonly Restler rest = new Restler();

        public ConnectionOwly(dynamic options)
        {

        }

        public JToken get(string url, dynamic options = null) => rest.get(url, options);
        public JToken post(string url, dynamic options = null) => rest.post(url, options);
        public JToken put(string url, dynamic options = null) => rest.put(url, options);
        public JToken del(string url, dynamic options = null) => rest.del(url, options);
        public JToken head(string url, dynamic options = null) => rest.head(url, options);
        public JToken patch(string url, dynamic options = null) => rest.patch(url, options);
        public JToken json(string url, object data, dynamic options = null) => rest.json(url, data, options);
        public JToken postJson(string url, object data, dynamic options = null) => rest.postJson(url, data, options);
        public JToken putJson(string url, object data, dynamic options = null) => rest.putJson(url, data, options);
    }
}
