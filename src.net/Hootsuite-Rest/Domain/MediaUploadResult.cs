using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hootsuite.Domain
{
    /// <summary>
    /// Class MediaUrl.
    /// </summary>
    public class MediaUploadResult
    {
        /// <summary>
        /// Froms the results.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>Message[].</returns>
        public static MediaUploadResult FromResults(JObject result) => result != null ? JsonConvert.DeserializeObject<MediaUploadResult>(result["data"].ToString(), HootsuiteClient.JsonSerializerSettings) : null;
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        /// <value>The Id.</value>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the thumbnail URL.
        /// </summary>
        /// <value>The thumbnail URL.</value>
        public int UploadDurationSeconds { get; set; }
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string UploadUrl { get; set; }
    }
}
