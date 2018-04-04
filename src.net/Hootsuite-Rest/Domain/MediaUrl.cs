namespace Hootsuite.Domain
{
    /// <summary>
    /// Class MediaUrl.
    /// </summary>
    public class MediaUrl
    {
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }
        /// <summary>
        /// Gets or sets the thumbnail URL.
        /// </summary>
        /// <value>The thumbnail URL.</value>
        public string ThumbnailUrl { get; set; }
        /// <summary>
        /// Gets or sets the video options identifier.
        /// </summary>
        /// <value>The video options identifier.</value>
        public Options VideoOptions { get; set; }
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
        }
        /// <summary>
        /// Class VideoOptions.
        /// </summary>
        public class Options
        {
            /// <summary>
            /// Gets or sets the Facebook identifier.
            /// </summary>
            /// <value>The Facebook identifier.</value>
            public FacebookVideoOptions Facebook { get; set; }
        }
    }
}
