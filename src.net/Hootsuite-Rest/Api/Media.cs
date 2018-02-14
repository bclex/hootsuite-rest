using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Hootsuite.Api
{
    /// <summary>
    /// Class Media.
    /// </summary>
    public class Media
    {
        HootsuiteClient _hootsuite;
        Connection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Media" /> class.
        /// </summary>
        /// <param name="hootsuite">The hootsuite.</param>
        /// <param name="connection">The connection.</param>
        public Media(HootsuiteClient hootsuite, Connection connection)
        {
            _hootsuite = hootsuite;
            _connection = connection;
        }

        /// <summary>
        /// Creates the URL.
        /// </summary>
        /// <param name="sizeBytes">The size bytes.</param>
        /// <param name="mimeType">Type of the MIME.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> CreateUrl(int sizeBytes, string mimeType = "video/mp4")
        {
            var path = util.createPath("media");
            var data = new
            {
                sizeBytes,
                mimeType = mimeType ?? "video/mp4",
            };
            return _connection.postJson(path, data);
        }

        /// <summary>
        /// Statuses the by identifier.
        /// </summary>
        /// <param name="mediaId">The media identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">mediaId</exception>
        public Task<dynamic> StatusById(string mediaId)
        {
            if (mediaId == null)
                throw new ArgumentNullException(nameof(mediaId));
            var path = util.createPath("media", mediaId);
            return _connection.get(path);
        }
    }
}
