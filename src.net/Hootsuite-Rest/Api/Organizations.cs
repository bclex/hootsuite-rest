using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Hootsuite.Api
{
    /// <summary>
    /// Class Organizations.
    /// </summary>
    public class Organizations
    {
        readonly HootsuiteClient _hootsuite;
        readonly Connection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Organizations" /> class.
        /// </summary>
        /// <param name="hootsuite">The hootsuite.</param>
        /// <param name="connection">The connection.</param>
        public Organizations(HootsuiteClient hootsuite, Connection connection)
        {
            _hootsuite = hootsuite;
            _connection = connection;
        }

        /// <summary>
        /// Finds the members.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">organizationId</exception>
        public Task<dynamic> FindMembers(string organizationId)
        {
            if (organizationId == null)
                throw new ArgumentNullException(nameof(organizationId));
            var path = util.createPath("organizations", organizationId, "members");
            return _connection.get(path);
        }

        /// <summary>
        /// Removes the member by identifier.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="memberId">The member identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">organizationId
        /// or
        /// memberId</exception>
        public Task<dynamic> RemoveMemberById(string organizationId, string memberId)
        {
            if (organizationId == null)
                throw new ArgumentNullException(nameof(organizationId));
            if (memberId == null)
                throw new ArgumentNullException(nameof(memberId));
            var path = util.createPath("organizations", organizationId, "members", memberId);
            return _connection.del(path);
        }

        /// <summary>
        /// Finds the member by identifier permissions.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="memberId">The member identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">organizationId
        /// or
        /// memberId</exception>
        public Task<dynamic> FindMemberByIdPermissions(string organizationId, string memberId)
        {
            if (organizationId == null)
                throw new ArgumentNullException(nameof(organizationId));
            if (memberId == null)
                throw new ArgumentNullException(nameof(memberId));
            var path = util.createPath("organizations", organizationId, "members", memberId, "permissions");
            return _connection.get(path);
        }

        /// <summary>
        /// Finds the member by identifier teams.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="memberId">The member identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">organizationId
        /// or
        /// memberId</exception>
        public Task<dynamic> FindMemberByIdTeams(string organizationId, string memberId)
        {
            if (organizationId == null)
                throw new ArgumentNullException(nameof(organizationId));
            if (memberId == null)
                throw new ArgumentNullException(nameof(memberId));
            var path = util.createPath("organizations", organizationId, "members", memberId, "teams");
            return _connection.get(path);
        }

        /// <summary>
        /// Finds the member by identifier social profiles.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="memberId">The member identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">organizationId
        /// or
        /// memberId</exception>
        public Task<dynamic> FindMemberByIdSocialProfiles(string organizationId, string memberId)
        {
            if (organizationId == null)
                throw new ArgumentNullException(nameof(organizationId));
            if (memberId == null)
                throw new ArgumentNullException(nameof(memberId));
            var path = util.createPath("organizations", organizationId, "members", memberId, "socialProfiles");
            return _connection.get(path);
        }

        /// <summary>
        /// Finds the social profiles by identifier permissions.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="socialProfileId">The social profile identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">organizationId
        /// or
        /// memberId
        /// or
        /// socialProfileId</exception>
        public Task<dynamic> FindSocialProfilesByIdPermissions(string organizationId, string memberId, string socialProfileId)
        {
            if (organizationId == null)
                throw new ArgumentNullException(nameof(organizationId));
            if (memberId == null)
                throw new ArgumentNullException(nameof(memberId));
            if (socialProfileId == null)
                throw new ArgumentNullException(nameof(socialProfileId));
            var path = util.createPath("organizations", organizationId, "members", memberId, "socialProfiles", socialProfileId, "permissions");
            return _connection.get(path);
        }
    }
}
