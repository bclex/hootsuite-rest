using System;
using System.Threading.Tasks;

namespace Hootsuite.Api
{
    /// <summary>
    /// Class Media.
    /// </summary>
    public class Media
    {
        readonly HootsuiteClient _hootsuite;
        readonly Connection _connection;

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
        /// <param name="options">The options.</param>
        /// <returns>Task&lt;dynamic&gt;.</returns>
        public Task<dynamic> CreateUrl(int sizeBytes, string mimeType = "video/mp4", dynamic options = null)
        {
            var path = util.createPath("media");
            var data = new
            {
                sizeBytes,
                mimeType = mimeType ?? "video/mp4",
            };
            return _connection.postJson(path, data, options);
        }

        /// <summary>
        /// Statuses the by identifier.
        /// </summary>
        /// <param name="mediaId">The media identifier.</param>
        /// <param name="options">The options.</param>
        /// <returns>Task&lt;dynamic&gt;.</returns>
        /// <exception cref="ArgumentNullException">mediaId</exception>
        public Task<dynamic> StatusById(string mediaId, dynamic options = null)
        {
            if (mediaId == null)
                throw new ArgumentNullException(nameof(mediaId));
            var path = util.createPath("media", mediaId);
            return _connection.get(path, options);
        }
    }
}
