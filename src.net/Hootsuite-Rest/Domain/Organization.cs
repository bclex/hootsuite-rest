using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hootsuite.Domain
{
    /// <summary>
    /// Class Organization.
    /// </summary>
    public class Organization
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// Froms the results.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>Organization[].</returns>
        public static Organization[] FromResults(JObject result) => result != null ? JsonConvert.DeserializeObject<Organization[]>(result["data"].ToString(), HootsuiteClient.JsonSerializerSettings) : null;
    }
}
