using Hootsuite.Require;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter.Net;
using LinqToTwitter.Security;

namespace Hootsuite
{
    /// <summary>
    /// Class ConnectionOwly.
    /// </summary>
    public class ConnectionOwly
    {
        readonly Action<string> _log = util.logger;
        readonly Restler _rest = new Restler();
        readonly dynamic _options;
        readonly Retry _retry;
        string _apiKey;
        const string TwitterVerifyEndpoint = "https://api.twitter.com/1.1/account/verify_credentials.json";
        const string TwitterOAuthVersion = "1.0";
        const string TwitterOAuthSignatureMethod = "HMAC-SHA1";
        const long UnixEpocTicks = 621355968000000000L;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionOwly"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ConnectionOwly(dynamic options)
        {
            _options = options ?? new { };
            _apiKey = dyn.getProp<string>(_options, "owlyApiKey", null);
            _retry = new Retry(dyn.getProp(_options, "retry", new { }));
        }

        /// <summary>
        /// Gets or sets the api key.
        /// </summary>
        /// <value>The api key.</value>
        public string ApiKey
        {
            get => _apiKey;
            set => _apiKey = !string.IsNullOrEmpty(value) ? _apiKey = value : null;
        }
        /// <summary>
        /// Gets the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="options">The options.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>Task&lt;dynamic&gt;.</returns>
        public Task<dynamic> get(string url, dynamic options = null, string contentType = null) => _request(url, null, options, Restler.Method.GET, contentType);
        /// <summary>
        /// Posts the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="options">The options.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>Task&lt;dynamic&gt;.</returns>
        public Task<dynamic> post(string url, dynamic options = null, string contentType = null) => _request(url, null, options, Restler.Method.POST, contentType);
        /// <summary>
        /// Puts the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="options">The options.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>Task&lt;dynamic&gt;.</returns>
        public Task<dynamic> put(string url, dynamic options = null, string contentType = null) => _request(url, null, options, Restler.Method.PUT, contentType);
        /// <summary>
        /// Deletes the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="options">The options.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>Task&lt;dynamic&gt;.</returns>
        public Task<dynamic> del(string url, dynamic options = null, string contentType = null) => _request(url, null, options, Restler.Method.DELETE, contentType);
        /// <summary>
        /// Heads the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="options">The options.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>Task&lt;dynamic&gt;.</returns>
        public Task<dynamic> head(string url, dynamic options = null, string contentType = null) => _request(url, null, options, Restler.Method.HEAD, contentType);
        /// <summary>
        /// Patches the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="options">The options.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>Task&lt;dynamic&gt;.</returns>
        public Task<dynamic> patch(string url, dynamic options = null, string contentType = null) => _request(url, null, options, Restler.Method.PATCH, contentType);
        /// <summary>
        /// Jsons the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        /// <param name="options">The options.</param>
        /// <returns>Task&lt;dynamic&gt;.</returns>
        public Task<dynamic> json(string url, object data, dynamic options = null) => _request(url, data, options, Restler.Method.GET);
        /// <summary>
        /// Posts the json.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        /// <param name="options">The options.</param>
        /// <returns>Task&lt;dynamic&gt;.</returns>
        public Task<dynamic> postJson(string url, object data, dynamic options = null) => _request(url, data, options, Restler.Method.POST);
        /// <summary>
        /// Puts the json.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        /// <param name="options">The options.</param>
        /// <returns>Task&lt;dynamic&gt;.</returns>
        public Task<dynamic> putJson(string url, object data, dynamic options = null) => _request(url, data, options, Restler.Method.PUT);

        async Task<dynamic> _request(string url, object data, dynamic options, Restler.Method method, string contentType = null)
        {
            options = dyn.exp(options, true);

            if (!Connection.HttpTestExp.IsMatch(url))
            {
                var baseUrl = dyn.getProp(_options, "url", config.apiOwly_url);
                url = baseUrl + url;
            }
            _log($"Request: {url}");
            Func<bool, Task<JObject>> requestFn = async (forceOAuth) =>
            {
                if (method == Restler.Method.POST && options.upload != null)
                {
                    var twitter = dyn.getObj(_options, "twitter", null);

                    var parameters = new Dictionary<string, string>();
                    parameters = (Dictionary<string, string>)AddMissingOAuthParameters(parameters, twitter);

                    //Twitter Auth
                    var encodedAndSortedString = BuildEncodedSortedString(parameters, twitter);
                    var signatureBaseString = BuildSignatureBaseString("POST", TwitterVerifyEndpoint, encodedAndSortedString);
                    var signingKey = BuildSigningKey(twitter.consumerSecret, twitter.oAuthTokenSecret);
                    var signature = CalculateSignature(signingKey, signatureBaseString);

                    options.headers = dyn.getObj(options, "headers", new { });
                    // Header breaks photo upload, for some reason
                    //options.headers.Authorization = BuildAuthorizationHeaderString(encodedAndSortedString, signature);

                    var b = new UriBuilder(url) { Query = GetQuery(null, parameters) };
                    url = b.Uri.AbsoluteUri;
                    

                    var fileBytes = DownloadFileBytes(options.upload.fileUrl, out string fileMimeType);
                    var formDataBoundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");

                    var postParameters = new Dictionary<string, object> {
                        { "apiKey", options.upload.apiKey },
                        { "fileName", options.upload.fileName },
                        { "uploaded_file", new FileParameter(fileBytes, options.upload.fileName, fileMimeType) }
                    };
                    
                    var content = GetMultipartFormData(postParameters, formDataBoundary, url);

                    options.content = content;
                }

                try
                {
                    var y = await _rest.request(url, options, method, contentType);
                    var d = (JObject)y;
                    if ((string)d["success"] == "false" && d["errors"] != null)
                    {
                        _log($"Request failed: {d}");
                        throw new InvalidOperationException(d["errors"].ToString());
                    }
                    return (dynamic)d;
                }
                catch (RestlerOperationException res)
                {
                    Console.WriteLine(res);
                    var err = (JObject)res.Content;
                    if (err["errors"] != null || err["error"] != null) { _log($"Request failed: {err}"); throw; }
                    else
                    {
                        var statusCode = Math.Floor((int)res.StatusCode / 100M);
                        if (statusCode == 5) throw res;
                        else if (statusCode == 4) throw res;
                        else throw res;
                    }
                }
            };
            return await _retry.start(requestFn);
        }

        IDictionary<string, string> AddMissingOAuthParameters(IDictionary<string, string> parameters, dynamic twitter)
        {
            if (parameters == null)
                parameters = new Dictionary<string, string>();

            if (!parameters.ContainsKey("oauth_token"))
                parameters.Add("oauth_token", twitter.oAuthToken);

            if (!parameters.ContainsKey("oauth_timestamp"))
                parameters.Add("oauth_timestamp", GetTimestamp());

            if (!parameters.ContainsKey("oauth_nonce"))
                parameters.Add("oauth_nonce", GenerateNonce());

            if (!parameters.ContainsKey("oauth_version"))
                parameters.Add("oauth_version", TwitterOAuthVersion);

            if (!parameters.ContainsKey("oauth_signature_method"))
                parameters.Add("oauth_signature_method", TwitterOAuthSignatureMethod);

            if (!parameters.ContainsKey("oauth_consumer_key"))
                parameters.Add("oauth_consumer_key", dyn.getProp<dynamic>(twitter, "consumerKey", null));

            return parameters;
        }

        string BuildEncodedSortedString(IDictionary<string, string> parameters, dynamic twitter)
        {
            AddMissingOAuthParameters(parameters, twitter);

            return
                string.Join("&",
                    (from parm in parameters
                     orderby parm.Key
                     select parm.Key + "=" + Url.PercentEncode(parameters[parm.Key]))
                    .ToArray());
        }

        string BuildSignatureBaseString(string method, string url, string encodedStringParameters)
        {
            int paramsIndex = url.IndexOf('?');

            string urlWithoutParams = paramsIndex >= 0 ? url.Substring(0, paramsIndex) : url;

            return string.Join("&", method.ToUpper(), Url.PercentEncode(urlWithoutParams), Url.PercentEncode(encodedStringParameters));
        }

        string BuildSigningKey(string consumerSecret, string oAuthTokenSecret)
        {
            return string.Format(
                CultureInfo.InvariantCulture, "{0}&{1}",
                Url.PercentEncode(consumerSecret),
                Url.PercentEncode(oAuthTokenSecret));
        }

        string CalculateSignature(string signingKey, string signatureBaseString)
        {
            byte[] key = Encoding.UTF8.GetBytes(signingKey);
            byte[] msg = Encoding.UTF8.GetBytes(signatureBaseString);

            byte[] hash = new Hmac(new Sha1()).Sign(key, msg);

            return Convert.ToBase64String(hash);
        }

        string BuildAuthorizationHeaderString(string encodedAndSortedString, string signature)
        {
            string[] allParms = (encodedAndSortedString + "&oauth_signature=" + Url.PercentEncode(signature)).Split('&');
            string allParmsString =
                string.Join(", ",
                    (from parm in allParms
                     let keyVal = parm.Split('=')
                     where parm.StartsWith("oauth") || parm.StartsWith("x_auth")
                     orderby keyVal[0]
                     select keyVal[0] + "=\"" + keyVal[1] + "\"")
                    .ToList());
            return "OAuth " + allParmsString;
        }

        string GetTimestamp()
        {
            long ticksSinceUnixEpoc = DateTime.UtcNow.Ticks - UnixEpocTicks;
            double secondsSinceUnixEpoc = new TimeSpan(ticksSinceUnixEpoc).TotalSeconds;
            return Math.Floor(secondsSinceUnixEpoc).ToString(CultureInfo.InvariantCulture);
        }

        internal virtual string GenerateNonce()
        {
            return new Random().Next(111111, 9999999).ToString(CultureInfo.InvariantCulture);
        }

        string GetQuery(string path, Dictionary<string, string> s)
        {
            if (s.Count < 1)
                return null;

            var parameters = s.Select(a =>
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
            Stream formDataStream = new MemoryStream();
            bool needsCLRF = false;

            foreach (var param in postParameters)
            {
                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;

                if (param.Value is FileParameter)
                {
                    FileParameter fileToUpload = (FileParameter)param.Value;
                    string header =
                        $"--{boundary}\r\nContent-Disposition: form-data; name=\"{param.Key}\"; filename=\"{fileToUpload.FileName ?? param.Key}\"\r\nContent-Type: {fileToUpload.ContentType ?? "application/octet-stream"}\r\n\r\n";

                    formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));
                    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                }
                else
                {
                    string postData = $"--{boundary}\r\nContent-Disposition: form-data; name=\"{param.Key}\"\r\n\r\n{param.Value}";
                    formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                }
            }

            string footer = $"\r\n--{boundary}--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            var multiPartContent = new MultipartFormDataContent(boundary);
            var byteArrayContent = new ByteArrayContent(formData);
            multiPartContent.Add(byteArrayContent);

            return multiPartContent;
        }
        byte[] DownloadFileBytes(string imageUrl, out string mimeType)
        {
            var buffer = new byte[4096];

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(imageUrl);

            using (WebResponse wr = webRequest.GetResponse())
            {
                mimeType = wr.ContentType;
                using (Stream responseStream = wr.GetResponseStream())
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        int count;
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

    /// <summary>
    /// Class FileParameter.
    /// </summary>
    public class FileParameter
    {

        /// <summary>
        /// The File.
        /// </summary>
        public byte[] File { get; set; }

        /// <summary>
        /// The FileName.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The ContentType.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The FileParameter.
        /// </summary>
        public FileParameter(byte[] file) : this(file, null)
        {
        }

        /// <summary>
        /// The FileParameter.
        /// </summary>
        public FileParameter(byte[] file, string filename) : this(file, filename, null)
        {
        }

        /// <summary>
        /// The FileParameter.
        /// </summary>
        public FileParameter(byte[] file, string filename, string contenttype)
        {
            File = file;
            FileName = filename;
            ContentType = contenttype;
        }
    }
}
