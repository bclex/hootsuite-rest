using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Hootsuite.Api
{
    /// <summary>
    /// Class Me.
    /// </summary>
    public class Me
    {
        HootsuiteClient _hootsuite;
        Connection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Me"/> class.
        /// </summary>
        /// <param name="hootsuite">The hootsuite.</param>
        /// <param name="connection">The connection.</param>
        public Me(HootsuiteClient hootsuite, Connection connection)
        {
            _hootsuite = hootsuite;
            _connection = connection;
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> Get()
        {
            var path = util.createPath("me");
            return _connection.get(path);
        }

        /// <summary>
        /// Gets the organizations.
        /// </summary>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> GetOrganizations()
        {
            var path = util.createPath("me", "organizations");
            return _connection.get(path);
        }

        /// <summary>
        /// Gets the social profiles.
        /// </summary>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> GetSocialProfiles()
        {
            var path = util.createPath("me", "socialProfiles");
            return _connection.get(path);
        }
    }
}
