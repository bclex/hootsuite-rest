using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Hootsuite.Api
{
    /// <summary>
    /// Class Teams.
    /// </summary>
    public class Teams
    {
        readonly HootsuiteClient _hootsuite;
        readonly Connection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Teams" /> class.
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
        /// <exception cref="ArgumentNullException">organizationId
        /// or
        /// teamName</exception>
        public Task<dynamic> CreateTeam(string organizationId, string teamName, dynamic options = null)
        {
            if (organizationId == null)
                throw new ArgumentNullException(nameof(organizationId));
            if (teamName == null)
                throw new ArgumentNullException(nameof(teamName));
            var path = util.createPath("organizations", organizationId, "teams");
            var data = new
            {
                teamName
            };
            return _connection.postJson(path, data, options);
        }

        /// <summary>
        /// Appends the member by identifier.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="teamId">The team identifier.</param>
        /// <param name="memberId">The member identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">organizationId
        /// or
        /// teamId
        /// or
        /// memberId</exception>
        public Task<dynamic> AppendMemberById(string organizationId, string teamId, string memberId, dynamic options = null)
        {
            if (organizationId == null)
                throw new ArgumentNullException(nameof(organizationId));
            if (teamId == null)
                throw new ArgumentNullException(nameof(teamId));
            if (memberId == null)
                throw new ArgumentNullException(nameof(memberId));
            var path = util.createPath("organizations", organizationId, "teams", teamId, "members", memberId);
            return _connection.post(path, options);
        }

        /// <summary>
        /// Finds the by identifier members.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="teamId">The team identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">organizationId
        /// or
        /// teamId</exception>
        public Task<dynamic> FindByIdMembers(string organizationId, string teamId, dynamic options = null)
        {
            if (organizationId == null)
                throw new ArgumentNullException(nameof(organizationId));
            if (teamId == null)
                throw new ArgumentNullException(nameof(teamId));
            var path = util.createPath("organizations", organizationId, "teams", teamId, "members");
            return _connection.get(path, options);
        }

        /// <summary>
        /// Finds the member by identifier permissions.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="teamId">The team identifier.</param>
        /// <param name="memberId">The member identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">organizationId
        /// or
        /// teamId
        /// or
        /// memberId</exception>
        public Task<dynamic> FindMemberByIdPermissions(string organizationId, string teamId, string memberId, dynamic options = null)
        {
            if (organizationId == null)
                throw new ArgumentNullException(nameof(organizationId));
            if (teamId == null)
                throw new ArgumentNullException(nameof(teamId));
            if (memberId == null)
                throw new ArgumentNullException(nameof(memberId));
            var path = util.createPath("organizations", organizationId, "teams", teamId, "members", memberId, "permissions");
            return _connection.get(path, options);
        }

        /// <summary>
        /// Finds the by identifier social profiles.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="teamId">The team identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">organizationId
        /// or
        /// teamId</exception>
        public Task<dynamic> FindByIdSocialProfiles(string organizationId, string teamId, dynamic options = null)
        {
            if (organizationId == null)
                throw new ArgumentNullException(nameof(organizationId));
            if (teamId == null)
                throw new ArgumentNullException(nameof(teamId));
            var path = util.createPath("organizations", organizationId, "teams", teamId, "socialProfiles");
            return _connection.get(path, options);
        }
    }
}
