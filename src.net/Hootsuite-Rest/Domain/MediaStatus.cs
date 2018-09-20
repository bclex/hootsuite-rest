using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hootsuite.Domain
{
    /// <summary>
    /// Class Media.
    /// </summary>
    public class MediaStatus
    {
        /// <summary>
        /// Froms the results.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>Message[].</returns>
        public static MediaStatus FromResults(JObject result) => result != null ? JsonConvert.DeserializeObject<MediaStatus>(result["data"].ToString(), HootsuiteClient.JsonSerializerSettings) : null;
        /// <summary>
        /// Gets or sets the download url.
        /// </summary>
        public string DownloadUrl { get; set; }
        /// <summary>
        /// Gets or sets the download duraction seconds.
        /// </summary>
        public int DownloadUrlDurationSeconds { get; set; }
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public string State { get; set; }
    }
}
