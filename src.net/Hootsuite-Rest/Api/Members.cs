using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Hootsuite.Api
{
    /// <summary>
    /// Class Members.
    /// </summary>
    public class Members
    {
        HootsuiteClient _hootsuite;
        Connection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Members"/> class.
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
        public Task<dynamic> FindById(string memberId)
        {
            var path = util.createPath("members", memberId);
            return _connection.get(path);
        }

        /// <summary>
        /// Creates the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> Create(dynamic msg)
        {
            var path = util.createPath("members");
            var data = new
            {
                organizationIds = dyn.getProp(msg, "organizationIds", new string[0]),
                email = dyn.getProp(msg, "email", (string)null),
                fullName = dyn.getProp(msg,"fullName", (string)null),
                companyName = dyn.getProp(msg, "companyName", (string)null),
                bio = dyn.getProp(msg, "bio", (string)null),
                timezone = dyn.getProp(msg, "timezone", (string)null),
                language = dyn.getProp(msg, "language", (string)null),
            };
            return _connection.postJson(path, data);
        }

        /// <summary>
        /// Finds the by identifier orgs.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> FindByIdOrgs(string memberId)
        {
            var path = util.createPath("members", memberId, "organizations");
            return _connection.get(path);
        }
    }
}
