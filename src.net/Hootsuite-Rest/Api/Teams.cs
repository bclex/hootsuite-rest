using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Hootsuite.Api
{
    /// <summary>
    /// Class Teams.
    /// </summary>
    public class Teams
    {
        HootsuiteClient _hootsuite;
        Connection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Teams"/> class.
        /// </summary>
        /// <param name="hootsuite">The hootsuite.</param>
        /// <param name="connection">The connection.</param>
        public Teams(HootsuiteClient hootsuite, Connection connection)
        {
            _hootsuite = hootsuite;
            _connection = connection;
        }

        /// <summary>
        /// Creates the team.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="teamName">Name of the team.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<JObject> CreateTeam(string organizationId, string teamName)
        {
            var path = util.createPath("organizations", organizationId, "teams");
            var data = new
            {
                teamName
            };
            return _connection.postJson(path, data);
        }

        /// <summary>
        /// Appends the member by identifier.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="teamId">The team identifier.</param>
        /// <param name="memberId">The member identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<JObject> AppendMemberById(string organizationId, string teamId, string memberId)
        {
            var path = util.createPath("organizations", organizationId, "teams", teamId, "members", memberId);
            return _connection.post(path);
        }

        /// <summary>
        /// Finds the by identifier members.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="teamId">The team identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<JObject> FindByIdMembers(string organizationId, string teamId)
        {
            var path = util.createPath("organizations", organizationId, "teams", teamId, "members");
            return _connection.get(path);
        }

        /// <summary>
        /// Finds the member by identifier permissions.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="teamId">The team identifier.</param>
        /// <param name="memberId">The member identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<JObject> FindMemberByIdPermissions(string organizationId, string teamId, string memberId)
        {
            var path = util.createPath("organizations", organizationId, "teams", teamId, "members", memberId, "permissions");
            return _connection.get(path);
        }

        /// <summary>
        /// Finds the by identifier social profiles.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="teamId">The team identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<JObject> FindByIdSocialProfiles(string organizationId, string teamId)
        {
            var path = util.createPath("organizations", organizationId, "teams", teamId, "socialProfiles");
            return _connection.get(path);
        }
    }
}
