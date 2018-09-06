using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

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
        public static Interaction[] FromResults(JObject result)
        {
            if (result == null) return null;
            var text = result["data"]["interactions"].ToString();
            try { return JsonConvert.DeserializeObject<Interaction[]>(text, HootsuiteClient.JsonSerializerSettings); }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(text);
                throw;
            }
        }
    }
}
