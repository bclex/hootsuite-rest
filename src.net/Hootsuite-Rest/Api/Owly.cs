using System;
using System.Threading.Tasks;
using Hootsuite.Require;

namespace Hootsuite.Api
{
    /// <summary>
    /// Class Owly.
    /// </summary>
    public class Owly
    {
        readonly HootsuiteClient _hootsuite;
        readonly ConnectionOwly _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Owly" /> class.
        /// </summary>
        /// <param name="hootsuite">The hootsuite.</param>
        /// <param name="connection">The connection.</param>
        public Owly(HootsuiteClient hootsuite, ConnectionOwly connection)
        {
            _hootsuite = hootsuite;
            _connection = connection;
        }

        /// <summary>
        /// Shortens the URL.
        /// </summary>
        /// <param name="longUrl">The long URL.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">longUrl</exception>
        public Task<dynamic> ShortenUrl(string longUrl)
        {
            if (longUrl == null)
                throw new ArgumentNullException(nameof(longUrl));

            var path = util.createOwlyPath("url", "shorten");
            var query = Restler.GetQuery(null, new
            {
                apiKey = _connection.ApiKey,
                longUrl
            });
            return _connection.get(path, new { query });
        }

        /// <summary>
        /// Expands the URL.
        /// </summary>
        /// <param name="shortUrl">The short URL.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">shortUrl</exception>
        public Task<dynamic> ExpandUrl(string shortUrl)
        {
            if (shortUrl == null)
                throw new ArgumentNullException(nameof(shortUrl));

            var path = util.createOwlyPath("url", "expand");
            var query = Restler.GetQuery(null, new
            {
                apiKey = _connection.ApiKey,
                shortUrl
            });
            return _connection.get(path, new { query });
        }


        /// <summary>
        /// Gets Info.
        /// </summary>
        /// <param name="shortUrl">The short URL.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">shortUrl</exception>
        public Task<dynamic> GetInfo(string shortUrl)
        {
            if (shortUrl == null)
                throw new ArgumentNullException(nameof(shortUrl));
            var path = util.createOwlyPath("url", "info");
            var query = Restler.GetQuery(null, new
            {
                apiKey = _connection.ApiKey,
                shortUrl
            });
            return _connection.get(path, new { query });
        }

        /// <summary>
        /// Gets the click stats.
        /// </summary>
        /// <param name="shortUrl">The short URL.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">shortUrl</exception>
        public Task<dynamic> GetClickStats(string shortUrl, DateTime? from, DateTime? to)
        {
            if (shortUrl == null)
                throw new ArgumentNullException(nameof(shortUrl));
            var path = util.createOwlyPath("url", "clickStats");
            //YYYY-MM-DD HH:mm:SS
            var query = Restler.GetQuery(null, new
            {
                apiKey = _connection.ApiKey,
                shortUrl,
                from = from?.ToString("yyyy-MM-dd HH:mm:ss"),
                to = to?.ToString("yyyy-MM-dd HH:mm:ss")
            });

            return _connection.get(path, new { query });
        }

        /// <summary>
        /// Uploads the photo.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileUrl">The file uri.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">fileName
        /// or
        /// uploaded_file</exception>
        public Task<dynamic> UploadPhoto(string fileUrl, string fileName = null)
        {
            if (fileUrl == null)
                throw new ArgumentNullException(nameof(fileUrl));

            if (fileName == null) {
                var uri = new Uri(fileUrl);
                fileName = System.IO.Path.GetFileName(uri.LocalPath);
            }

            var path = util.createOwlyPath("photo", "upload");
            dynamic options = new
            {
                timeout = 10000,
                upload = new
                {
                    apiKey = _connection.ApiKey,
                    fileName,
                    fileUrl
                }
            };
            return _connection.post(path, options);
        }

        /// <summary>
        /// Uploads the document.
        /// </summary>
        /// <param name="fileUrl">The file url.</param>
        /// <param name="fileName">The file name.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">fileName
        /// or
        /// uploaded_file</exception>
        public Task<dynamic> UploadDoc(string fileUrl, string fileName = null)
        {
            if (fileUrl == null)
                throw new ArgumentNullException(nameof(fileUrl));

            if (fileName == null)
            {
                var uri = new Uri(fileUrl);
                fileName = System.IO.Path.GetFileName(uri.LocalPath);
            }

            var path = util.createOwlyPath("doc", "upload");
            dynamic options = new
            {
                upload = new
                {
                    apiKey = _connection.ApiKey,
                    fileName,
                    fileUrl
                }
            };
            return _connection.post(path, options);
        }
    }
}
