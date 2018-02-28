using System;
using System.Threading.Tasks;

namespace Hootsuite.Api
{
    /// <summary>
    /// Class Members.
    /// </summary>
    public class Members
    {
        readonly HootsuiteClient _hootsuite;
        readonly Connection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Members" /> class.
        /// </summary>
        /// <param name="hootsuite">The hootsuite.</param>
        /// <param name="connection">The connection.</param>
        public Members(HootsuiteClient hootsuite, Connection connection)
        {
            _hootsuite = hootsuite;
            _connection = connection;
        }

        /// <summary>
        /// Finds the by identifier.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">memberId</exception>
        public Task<dynamic> FindById(string memberId)
        {
            if (memberId == null)
                throw new ArgumentNullException(nameof(memberId));
            var path = util.createPath("members", memberId);
            return _connection.get(path);
        }

        /// <summary>
        /// Creates the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">msg</exception>
        public Task<dynamic> Create(dynamic msg)
        {
            if (msg == null)
                throw new ArgumentNullException(nameof(msg));
            var path = util.createPath("members");
            var data = new
            {
                organizationIds = dyn.getProp<string[]>(msg, "organizationIds"),
                email = dyn.getProp<string>(msg, "email"),
                fullName = dyn.getProp<string>(msg, "fullName"),
                companyName = dyn.getProp<string>(msg, "companyName"),
                bio = dyn.getProp<string>(msg, "bio"),
                timezone = dyn.getProp<string>(msg, "timezone"),
                language = dyn.getProp<string>(msg, "language"),
            };
            return _connection.postJson(path, data);
        }

        /// <summary>
        /// Finds the by identifier orgs.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">memberId</exception>
        public Task<dynamic> FindByIdOrgs(string memberId)
        {
            if (memberId == null)
                throw new ArgumentNullException(nameof(memberId));
            var path = util.createPath("members", memberId, "organizations");
            return _connection.get(path);
        }
    }
}
