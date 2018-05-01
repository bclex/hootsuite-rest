using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hootsuite.Domain
{
    /// <summary>
    /// Class Media.
    /// </summary>
    public class Media
    {
        /// <summary>
        /// Froms the results.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>Message[].</returns>
        public static Media FromResults(JObject result) => result != null ? JsonConvert.DeserializeObject<Media>(result["data"].ToString(), HootsuiteClient.JsonSerializerSettings) : null;
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
        /// <summary>
        /// Gets or sets the thumbnail identifier.
        /// </summary>
        /// <value>The thumbnail identifier.</value>
        public string ThumbnailId { get; set; }
        /// <summary>
        /// Gets or sets the video options identifier.
        /// </summary>
        /// <value>The video options identifier.</value>
        public Options VideoOptions { get; set; }
        
        /// <summary>
        /// Class Options.
        /// </summary>
        public class Options
        {
            /// <summary>
            /// Gets or sets the Facebook identifier.
            /// </summary>
            /// <value>The Facebook identifier.</value>
            public FacebookVideoOptions Facebook { get; set; }
        }

        /// <summary>
        /// Class FacebookVideoOptions.
        /// </summary>
        public class FacebookVideoOptions
        {
            /// <summary>
            /// Gets or sets the category identifier.
            /// </summary>
            /// <value>The category identifier.</value>
            public string Category { get; set; }
            /// <summary>
            /// Gets or sets the title identifier.
            /// </summary>
            /// <value>The title identifier.</value>
            public string Title { get; set; }
        }
    }
}
