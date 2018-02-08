using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Hootsuite.Api
{
    /// <summary>
    /// Class SocialProfiles.
    /// </summary>
    public class SocialProfiles
    {
        HootsuiteClient _hootsuite;
        Connection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialProfiles"/> class.
        /// </summary>
        /// <param name="hootsuite">The hootsuite.</param>
        /// <param name="connection">The connection.</param>
        public SocialProfiles(HootsuiteClient hootsuite, Connection connection)
        {
            _hootsuite = hootsuite;
            _connection = connection;
        }

        /// <summary>
        /// Finds this instance.
        /// </summary>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> Find()
        {
            var path = util.createPath("socialProfiles");
            return _connection.get(path);
        }

        /// <summary>
        /// Finds the by identifier.
        /// </summary>
        /// <param name="socialProfileId">The social profile identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> FindById(string socialProfileId)
        {
            var path = util.createPath("socialProfiles", socialProfileId);
            return _connection.get(path);
        }

        /// <summary>
        /// Finds the by identifier teams.
        /// </summary>
        /// <param name="socialProfileId">The social profile identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> FindByIdTeams(string socialProfileId)
        {
            var path = util.createPath("socialProfiles", socialProfileId, "teams");
            return _connection.get(path);
        }
    }
}
