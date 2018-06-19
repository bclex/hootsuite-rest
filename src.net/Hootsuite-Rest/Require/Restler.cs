using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//https://github.com/danwrong/restler
namespace Hootsuite.Require
{
    /// <summary>
    /// Class Restler.
    /// </summary>
    public class Restler
    {
        readonly HttpClient _client = new HttpClient();

        /// <summary>
        /// Enum Method
        /// </summary>
        public enum Method
        {
            /// <summary>
            /// The get
            /// </summary>
            GET,
            /// <summary>
            /// The patch
            /// </summary>
            PATCH,
            /// <summary>
            /// The post
            /// </summary>
            POST,
            /// <summary>
            /// The put
            /// </summary>
            PUT,
            /// <summary>
            /// The delete
            /// </summary>
            DELETE,
            /// <summary>
            /// The head
            /// </summary>
            HEAD,
        }

        /// <summary>
        /// Requests the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="options">The options.</param>
        /// <param name="method">The method.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="onResponse">The on response.</param>
        /// <returns>Task&lt;System.Object&gt;.</returns>
        /// <exception cref="RestlerOperationException">
        /// 0 - Operation Timed Out
        /// </exception>
        public async Task<object> request(string url, dynamic options, Method method = Method.GET, string contentType = null, Action<HttpResponseMessage, string> onResponse = null)
        {
            var uri = new Uri(url);
            // query
            if (dyn.hasProp(options, "query") && string.IsNullOrEmpty(uri.Query))
            {
                var query = options.query is string ? options.query : GetQuery(null, options.query);
                var b = new UriBuilder(uri) { Query = query };
                uri = b.Uri;
            }
            // data
            HttpContent content = null;
            if (dyn.hasProp(options, "data") && options.data != null)
                content = options.data is string ?
                    (HttpContent)new StringContent(options.data, Encoding.UTF8, contentType) :
                    new FormUrlEncodedContent(dyn.getDataAsString(options.data));
            else if (dyn.hasProp(options, "content") && options.content != null)
                content = dyn.getProp<HttpContent>(options, "content");
            // request
            var req = new HttpRequestMessage(new HttpMethod(method.ToString()), uri) { Content = content };
            // headers
            if (dyn.hasProp(options, "headers"))
                foreach (KeyValuePair<string, string> header in dyn.getDataAsString(options.headers))
                    req.Headers.Add(header.Key, header.Value);
            // timeout
            var timeout = dyn.getProp(options, "timeout", 5000);
            // make request
            try
            {
                var res = await _client.SendAsync(req, new CancellationTokenSource(timeout).Token);
                var body = await res.Content.ReadAsStringAsync();
                onResponse?.Invoke(res, body);
                var r = res.Content.Headers.ContentType.MediaType == "application/json" ? (object)JObject.Parse(body) : body;
                if (!res.IsSuccessStatusCode)
                    throw new RestlerOperationException(res.StatusCode, r);
                return r;
            }
            catch (TaskCanceledException e) { throw new RestlerOperationException(0, "Operation Timed Out") { Timedout = true, E = e }; }
        }

        /// <summary>
        /// Gets the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="options">The options.</param>
        /// <param name="onResponse">The on response.</param>
        /// <returns>Task&lt;System.Object&gt;.</returns>
        public async Task<object> get(string url, dynamic options, Action<HttpResponseMessage, string> onResponse = null) => await request(url, options, Method.GET, onResponse: onResponse);
        /// <summary>
        /// Patches the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="options">The options.</param>
        /// <param name="onResponse">The on response.</param>
        /// <returns>Task&lt;System.Object&gt;.</returns>
        public async Task<object> patch(string url, dynamic options, Action<HttpResponseMessage, string> onResponse = null) => await request(url, options, Method.PATCH, onResponse: onResponse);
        /// <summary>
        /// Posts the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="options">The options.</param>
        /// <param name="onResponse">The on response.</param>
        /// <returns>Task&lt;System.Object&gt;.</returns>
        public async Task<object> post(string url, dynamic options, Action<HttpResponseMessage, string> onResponse = null) => await request(url, options, Method.POST, onResponse: onResponse);
        /// <summary>
        /// Puts the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="options">The options.</param>
        /// <param name="onResponse">The on response.</param>
        /// <returns>Task&lt;System.Object&gt;.</returns>
        public async Task<object> put(string url, dynamic options, Action<HttpResponseMessage, string> onResponse = null) => await request(url, options, Method.PUT, onResponse: onResponse);
        /// <summary>
        /// Deletes the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="options">The options.</param>
        /// <param name="onResponse">The on response.</param>
        /// <returns>Task&lt;System.Object&gt;.</returns>
        public async Task<object> del(string url, dynamic options, Action<HttpResponseMessage, string> onResponse = null) => await request(url, options, Method.DELETE, onResponse: onResponse);
        /// <summary>
        /// Heads the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="options">The options.</param>
        /// <param name="onResponse">The on response.</param>
        /// <returns>Task&lt;System.Object&gt;.</returns>
        public async Task<object> head(string url, dynamic options, Action<HttpResponseMessage, string> onResponse = null) => await request(url, options, Method.HEAD, onResponse: onResponse);
        /// <summary>
        /// Jsons the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        /// <param name="options">The options.</param>
        /// <param name="method">The method.</param>
        /// <param name="onResponse">The on response.</param>
        /// <param name="fixup">The fixup.</param>
        /// <returns>Task&lt;System.Object&gt;.</returns>
        public async Task<object> json(string url, object data, dynamic options, Method method = Method.GET, Action<HttpResponseMessage, string> onResponse = null, Func<string, string> fixup = null)
        {
            var dataAsJson = JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }
            });
            if (fixup != null) dataAsJson = fixup(dataAsJson);
            options = dyn.exp(options);
            options.data = dataAsJson;
            return await request(url, options, method, "application/json", onResponse: onResponse);
        }
        /// <summary>
        /// Posts the json.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        /// <param name="options">The options.</param>
        /// <param name="onResponse">The on response.</param>
        /// <param name="fixup">The fixup.</param>
        /// <returns>Task&lt;System.Object&gt;.</returns>
        public async Task<object> postJson(string url, object data, dynamic options, Action<HttpResponseMessage, string> onResponse = null, Func<string, string> fixup = null) => await json(url, data, options, Method.POST, onResponse: onResponse, fixup: fixup);
        /// <summary>
        /// Puts the json.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        /// <param name="options">The options.</param>
        /// <param name="onResponse">The on response.</param>
        /// <param name="fixup">The fixup.</param>
        /// <returns>Task&lt;System.Object&gt;.</returns>
        public async Task<object> putJson(string url, object data, dynamic options, Action<HttpResponseMessage, string> onResponse = null, Func<string, string> fixup = null) => await json(url, data, options, Method.PUT, onResponse: onResponse, fixup: fixup);
        /// <summary>
        /// Patches the json.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        /// <param name="options">The options.</param>
        /// <param name="onResponse">The on response.</param>
        /// <param name="fixup">The fixup.</param>
        /// <returns>Task&lt;System.Object&gt;.</returns>
        public async Task<object> patchJson(string url, object data, dynamic options, Action<HttpResponseMessage, string> onResponse = null, Func<string, string> fixup = null) => await json(url, data, options, Method.PATCH, onResponse: onResponse, fixup: fixup);

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="s">The s.</param>
        /// <returns>System.String.</returns>
        public static string GetQuery(string path, object s)
        {
            var parameters = dyn.getDataAsString(s).Select(a =>
            {
                try { return $"{Uri.EscapeDataString(a.Key)}={Uri.EscapeDataString(a.Value)}"; }
                catch (Exception ex) { throw new InvalidOperationException($"Failed when processing '{a}'.", ex); }
            })
            .Aggregate((a, b) => (string.IsNullOrEmpty(a) ? b : $"{a}&{b}"));
            return path != null ? $"{path}?{parameters}" : parameters;
        }

        static MultipartFormDataContent GetMultipartFormData(Dictionary<string, object> postParameters, string boundary, string url)
        {
            var encoding = Encoding.UTF8;
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;

            foreach (var param in postParameters)
            {
                // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
                // Skip it on the first parameter, add it to subsequent parameters.
                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;

                if (param.Value is FileParameter)
                {
                    FileParameter fileToUpload = (FileParameter)param.Value;

                    // Add just the first part of this param, since we will write the file data directly to the Stream
                    var header =
                        $"--{boundary}\r\nContent-Disposition: form-data; name=\"{param.Key}\"; filename=\"{fileToUpload.FileName ?? param.Key}\"\r\nContent-Type: {fileToUpload.ContentType ?? "application/octet-stream"}\r\n\r\n";

                    formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                    // Write the file data directly to the Stream, rather than serializing it to a string.
                    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                }
                else
                {
                    var postData = $"--{boundary}\r\nContent-Disposition: form-data; name=\"{param.Key}\"\r\n\r\n{param.Value}";
                    formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                }
            }

            // Add the end of the request.  Start with a newline
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            var multiPartContent = new MultipartFormDataContent(boundary);
            var byteArrayContent = new ByteArrayContent(formData);
            multiPartContent.Add(byteArrayContent);

            return multiPartContent;
        }
        internal byte[] DownloadFileBytes(string imageUrl, out string mimeType)
        {
            var buffer = new byte[4096];

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(imageUrl);

            using (var wr = webRequest.GetResponse()) {
                mimeType = wr.ContentType;
                using (var responseStream = wr.GetResponseStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        int count = 0;
                        do
                        {
                            count = responseStream.Read(buffer, 0, buffer.Length);
                            memoryStream.Write(buffer, 0, count);
                        } while (count != 0);
                        return memoryStream.ToArray();
                    }
                }
            }
        }
    }
}