using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//https://github.com/danwrong/restler
namespace Hootsuite.Rest.Require
{
    public class Restler
    {
        readonly HttpClient _client = new HttpClient();

        public enum Method
        {
            GET,
            PATCH,
            POST,
            PUT,
            DELETE,
            HEAD,
        }

        public Restler()
        {
            //_client.DefaultRequestHeaders.Accept.Clear();
            //_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            //_client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
        }

        public async Task<object> request(string url, dynamic options, Method method = Method.GET, string contentType = null)
        {
            var uri = new Uri(url);
            // query
            if (dyn.hasProp(options, "query") && string.IsNullOrEmpty(uri.Query))
            {
                var query = !options.query is string ? GetQuery(null, options.query) : options.query;
                var b = new UriBuilder(uri); b.Query = query; uri = b.Uri;
            }
            // data
            HttpContent content = null;
            if (dyn.hasProp(options, "data") && options.data != null)
                content = options.data is string ?
                    (HttpContent)new StringContent(options.data, Encoding.UTF8, contentType) :
                    new FormUrlEncodedContent(dyn.getData(options.data));
            // request
            var req = new HttpRequestMessage(new HttpMethod(method.ToString()), uri) { Content = content };
            // headers
            if (dyn.hasProp(options, "headers"))
                foreach (KeyValuePair<string, string> header in dyn.getData(options.headers))
                    req.Headers.Add(header.Key, header.Value);
            // timeout
            var timeout = dyn.getProp(options, "timeout", 2000);
            // make request
            try
            {
                //HttpResponseMessage res = null;
                //var cancellationToken = new CancellationTokenSource(timeout).Token;
                //switch (method)
                //{
                //    case Method.GET: res = await _client.GetAsync(uri, cancellationToken); break;
                //    case Method.POST: res = await _client.PostAsync(uri, content, cancellationToken); break;
                //    case Method.PUT: res = await _client.PutAsync(uri, content, cancellationToken); break;
                //    case Method.DELETE: res = await _client.DeleteAsync(uri, cancellationToken); break;
                //    case Method.PATCH:
                //    case Method.HEAD:
                //        var req = new HttpRequestMessage(new HttpMethod(method.ToString()), uri) { Content = content };
                //        res = await _client.SendAsync(req, cancellationToken);
                //        break;
                //}
                var res = await _client.SendAsync(req, new CancellationTokenSource(timeout).Token);
                //Console.WriteLine(res);
                var body = await res.Content.ReadAsStringAsync();
                //Console.WriteLine(body);
                var r = res.Content.Headers.ContentType.MediaType == "application/json" ? (object)JObject.Parse(body) : body;
                if (!res.IsSuccessStatusCode)
                    throw new RestlerOperationException(res.StatusCode, r);
                return r;
            }
            catch (TaskCanceledException e) { throw new RestlerOperationException(0, null) { E = e, Timedout = true }; }
        }

        public async Task<object> get(string url, dynamic options) => await request(url, options, Method.GET);
        public async Task<object> patch(string url, dynamic options) => await request(url, options, Method.PATCH);
        public async Task<object> post(string url, dynamic options) => await request(url, options, Method.POST);
        public async Task<object> put(string url, dynamic options) => await request(url, options, Method.PUT);
        public async Task<object> del(string url, dynamic options) => await request(url, options, Method.DELETE);
        public async Task<object> head(string url, dynamic options) => await request(url, options, Method.HEAD);
        public async Task<object> json(string url, object data, dynamic options, Method method = Method.GET)
        {
            options.data = JsonConvert.SerializeObject(data);
            return await request(url, options, method, "application/json");
        }
        public async Task<object> postJson(string url, object data, dynamic options) => await json(url, data, options, Method.POST);
        public async Task<object> putJson(string url, object data, dynamic options) => await json(url, data, options, Method.PUT);
        public async Task<object> patchJson(string url, object data, dynamic options) => await json(url, data, options, Method.PATCH);


        public static string GetQuery(string path, object s)
        {
            var parameters = dyn.getData(s).Select(a =>
                {
                    try { return string.Format("{0}={1}", Uri.EscapeDataString(a.Key), Uri.EscapeDataString(a.Value)); }
                    catch (Exception ex) { throw new InvalidOperationException(string.Format("Failed when processing '{0}'.", a), ex); }
                })
                .Aggregate((a, b) => (string.IsNullOrEmpty(a) ? b : string.Format("{0}&{1}", a, b)));
            return path != null ? string.Format("{0}?{1}", path, parameters) : parameters;
        }
    }
}