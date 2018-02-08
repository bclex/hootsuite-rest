using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Hootsuite.Api
{
    /// <summary>
    /// Class Scim.
    /// </summary>
    public class Scim
    {
        HootsuiteClient _hootsuite;
        Connection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scim"/> class.
        /// </summary>
        /// <param name="hootsuite">The hootsuite.</param>
        /// <param name="connection">The connection.</param>
        public Scim(HootsuiteClient hootsuite, Connection connection)
        {
            _hootsuite = hootsuite;
            _connection = connection;
        }

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> CreateUser(dynamic msg)
        {
            var path = util.createScimPath("Users");
            var data = new
            {
                schemas = dyn.getProp(msg, "schemas", (string)null),
                userName = dyn.getProp(msg, "userName", (string)null),
                name = dyn.getProp(msg, "name", (string)null),
                emails = dyn.getProp(msg, "emails", (string)null),
                displayName = dyn.getProp(msg, "displayName", (string)null),
                timezone = dyn.getProp(msg, "timezone", (string)null),
                preferredLanguage = dyn.getProp(msg, "preferredLanguage", (string)null),
                groups = dyn.getProp(msg, "groups", (string)null),
                active = dyn.getProp(msg, "active", true),
                //"urn:ietf:params:scim:schemas:extension:Hootsuite:2.0:User" = msg['urn:ietf:params:scim:schemas:extension:Hootsuite:2.0:User'] ?? null,
            };
            return _connection.postJson(path, data);
        }

        /// <summary>
        /// Finds the users.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="count">The count.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> FindUsers(string filter = null, int count = 0, int startIndex = 0)
        {
            var path = util.createScimPath("Users");
            var options = new
            {
                query = new
                {
                    filter,
                    count,
                    startIndex
                },
            };
            return _connection.get(path, options);
        }

        /// <summary>
        /// Finds the user by identifier.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> FindUserById(string memberId)
        {
            var path = util.createScimPath("Users", memberId);
            return _connection.get(path);
        }

        /// <summary>
        /// Replaces the user by identifier.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="msg">The MSG.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> ReplaceUserById(string memberId, dynamic msg)
        {
            var path = util.createScimPath("Users", memberId);
            var data = new
            {
                schemas = dyn.getProp(msg, "schemas", (string)null),
                userName = dyn.getProp(msg, "userName", (string)null),
                name = dyn.getProp(msg, "name", (string)null),
                emails = dyn.getProp(msg, "emails", (string)null),
                displayName = dyn.getProp(msg, "displayName", (string)null),
                timezone = dyn.getProp(msg, "timezone", (string)null),
                preferredLanguage = dyn.getProp(msg, "preferredLanguage", (string)null),
                groups = dyn.getProp(msg, "groups", (string)null),
                active = dyn.getProp(msg, "active", true),
                //'urn:ietf:params:scim:schemas:extension:Hootsuite:2.0:User': msg['urn:ietf:params:scim:schemas:extension:Hootsuite:2.0:User'] || undefined
            };
            return _connection.putJson(path, data);
        }

        /// <summary>
        /// Modifies the user by identifier.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="msg">The MSG.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> ModifyUserById(string memberId, dynamic msg)
        {
            var path = util.createScimPath("Users", memberId);
            var options = new
            {
                //headers = new { "content-type" = "application/json" },
                data = new
                {
                    schemas = dyn.getProp(msg, "schemas", (string)null),
                    Operations = dyn.getProp(msg, "Operations", (string)null),
                },
            };
            return _connection.patch(path, options);
        }

        /// <summary>
        /// Gets the resource types.
        /// </summary>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> GetResourceTypes()
        {
            var path = util.createScimPath("ResourceTypes");
            return _connection.get(path);
        }
    }
}
