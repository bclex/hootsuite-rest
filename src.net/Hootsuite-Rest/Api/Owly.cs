using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;

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
            return _connection.get(path, new { longUrl });
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
            var path = util.createPath("url", "expand");
            return _connection.get(path, new { shortUrl });
        }

        /// <summary>
        /// Gets the information.
        /// </summary>
        /// <param name="shortUrl">The short URL.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">shortUrl</exception>
        public Task<dynamic> GetInfo(string shortUrl)
        {
            if (shortUrl == null)
                throw new ArgumentNullException(nameof(shortUrl));
            var path = util.createPath("url", "info");
            return _connection.get(path, new { shortUrl });
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
            var path = util.createPath("url", "clickStats");
            return _connection.get(path, new { shortUrl, from, to });
        }

        /// <summary>
        /// Uploads the photo.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="uploaded_file">The uploaded file.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">fileName
        /// or
        /// uploaded_file</exception>
        public Task<dynamic> UploadPhoto(string fileName, Stream uploaded_file)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));
            if (uploaded_file == null)
                throw new ArgumentNullException(nameof(uploaded_file));
            var path = util.createPath("photo", "upload");
            return _connection.post(path, new { fileName, uploaded_file });
        }

        /// <summary>
        /// Uploads the document.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="uploaded_file">The uploaded file.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">fileName
        /// or
        /// uploaded_file</exception>
        public Task<dynamic> UploadDoc(string fileName, Stream uploaded_file)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));
            if (uploaded_file == null)
                throw new ArgumentNullException(nameof(uploaded_file));
            var path = util.createPath("doc", "upload");
            return _connection.post(path, new { fileName, uploaded_file });
        }
    }
}
