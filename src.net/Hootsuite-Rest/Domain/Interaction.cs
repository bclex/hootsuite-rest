using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hootsuite.Domain
{
    /// <summary>
    /// Class Interaction.
    /// </summary>
    public class Interaction
    {
        /// <summary>
        /// Gets or sets from social network identifier.
        /// </summary>
        /// <value>From social network identifier.</value>
        public long FromSocialNetworkId { get; set; }
        /// <summary>
        /// Gets or sets to social network identifier.
        /// </summary>
        /// <value>To social network identifier.</value>
        public long ToSocialNetworkId { get; set; }
        /// <summary>
        /// Gets or sets the type of the social network.
        /// </summary>
        /// <value>The type of the social network.</value>
        public string SocialNetworkType { get; set; }
        /// <summary>
        /// Gets or sets the external message identifier.
        /// </summary>
        /// <value>The external message identifier.</value>
        public string ExternalMessageId { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }

        /// <summary>
        /// Froms the results.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>Interaction[].</returns>
        public static Interaction[] FromResults(JObject result) => result != null ? JsonConvert.DeserializeObject<Interaction[]>(result["data"]["interactions"].ToString(), HootsuiteClient.JsonSerializerSettings) : null;
    }
}
