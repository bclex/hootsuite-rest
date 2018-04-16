using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hootsuite.Domain
{
    /// <summary>
    /// Class Member.
    /// </summary>
    public class Member
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
        /// <returns>Member[].</returns>
        public static Member[] FromResults(JObject result) => result != null ? JsonConvert.DeserializeObject<Member[]>(result["data"].ToString(), HootsuiteClient.JsonSerializerSettings) : null;
    }
}
