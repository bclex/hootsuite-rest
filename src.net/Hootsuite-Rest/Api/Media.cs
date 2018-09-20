using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

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

        /// <summary>
        /// Uploads the media.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileUrl">The file uri.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">fileName
        /// or
        /// uploaded_file</exception>
        public async Task<dynamic> UploadMediaAsync(string fileUrl, string fileName = null)
        {
            if (fileUrl == null)
                throw new ArgumentNullException(nameof(fileUrl));

            var uri = new Uri(fileUrl);

            if (fileName == null)
            {
                fileName = Path.GetFileName(uri.LocalPath);
            }

            byte[] fileBytes;
            string mimeType;

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                fileBytes = client.DownloadData(uri);
                mimeType = client.ResponseHeaders["Content-Type"];
            }

            JObject result = await CreateUrl(fileBytes.Length, mimeType);
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers.Add("Content-Type", mimeType);

                try
                {
                    var uploadUrl = result["data"]["uploadUrl"].ToString();
                    var uploadResult = client.UploadData(uploadUrl, "PUT", fileBytes);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);

                    throw;
                }
            }

            return result;
        }
    }
}
