namespace Hootsuite.Domain
{
    /// <summary>
    /// Class Media.
    /// </summary>
    public class Media
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }
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
