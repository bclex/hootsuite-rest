using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

//https://github.com/danwrong/restler
namespace Hootsuite.Rest.Require
{
    public class Restler
    {
        readonly HttpClient client = new HttpClient();

        public Restler()
        {
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
        }

        public async Task<JToken> request(string url, dynamic options, string method)
        {
            var stringTask = client.GetStringAsync("https://api.github.com/orgs/dotnet/repos");

            var msg = await stringTask;
            Console.Write(msg);

            return null;
        }

        public async Task<JToken> get(string url, dynamic options) => await request(url, options, "GET");
        public async Task<JToken> patch(string url, dynamic options) => await request(url, options, "PATCH");
        public async Task<JToken> post(string url, dynamic options) => await request(url, options, "POST");
        public async Task<JToken> put(string url, dynamic options) => await request(url, options, "PUT");
        public async Task<JToken> del(string url, dynamic options) => await request(url, options, "DELETE");
        public async Task<JToken> head(string url, dynamic options) => await request(url, options, "HEAD");
        public async Task<JToken> json(string url, object data, dynamic options, string method = "GET")
        {
            //options.headers = options.headers || { };
            //options.headers['content-type'] = 'application/json';
            //options.data = JSON.stringify(data || { });
            return await request(url, options, method);
        }
        public async Task<JToken> postJson(string url, object data, dynamic options) => await json(url, data, options, "POST");
        public async Task<JToken> putJson(string url, object data, dynamic options) => await json(url, data, options, "PUT");
        public async Task<JToken> patchJson(string url, object data, dynamic options) => await json(url, data, options, "PATCH");

        public static string GetQuery(string path, object request)
        {
            var requestParameters = request.GetType().GetProperties()
                .Where(x => x.CanRead && x.GetValue(request, null) != null)
                .Select(x => new Tuple<string, string>(x.Name, x.GetValue(request, null).ToString()))
                .ToList();
            var parameters = requestParameters
                .Select(a =>
                {
                    try { return string.Format("{0}={1}", Uri.EscapeDataString(a.Item1), Uri.EscapeDataString(a.Item2)); }
                    catch (Exception ex) { throw new InvalidOperationException(string.Format("Failed when processing '{0}'.", a), ex); }
                })
                .Aggregate((a, b) => (string.IsNullOrEmpty(a) ? b : string.Format("{0}&{1}", a, b)));
            return path != null ? string.Format("{0}?{1}", path, parameters) : parameters;
        }
    }
}