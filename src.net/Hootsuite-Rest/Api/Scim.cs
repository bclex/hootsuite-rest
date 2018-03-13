using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Hootsuite.Api
{
    /// <summary>
    /// Class Scim.
    /// </summary>
    public class Scim
    {
        readonly HootsuiteClient _hootsuite;
        readonly Connection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scim" /> class.
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
        /// <param name="options">The options.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">msg</exception>
        public Task<dynamic> CreateUser(dynamic msg, dynamic options = null)
        {
            if (msg == null)
                throw new ArgumentNullException(nameof(msg));
            var path = util.createScimPath("Users");
            var data = new
            {
                schemas = dyn.getProp<string[]>(msg, "schemas"),
                userName = dyn.getProp<string>(msg, "userName"),
                name = dyn.getProp<object>(msg, "name"),
                emails = dyn.getProp<object[]>(msg, "emails"),
                displayName = dyn.getProp<string>(msg, "displayName"),
                timezone = dyn.getProp<string>(msg, "timezone"),
                preferredLanguage = dyn.getProp<string>(msg, "preferredLanguage"),
                groups = dyn.getProp<object[]>(msg, "groups"),
                active = dyn.getProp<bool>(msg, "active", true),
                scim__user = dyn.getProp<object>(msg, "scim__user"),
            };
            return _connection.postJson(path, data, options);
        }

        /// <summary>
        /// Finds the users.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="count">The count.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="options">The options.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> FindUsers(string filter = null, int count = 0, int startIndex = 0, dynamic options = null)
        {
            var path = util.createScimPath("Users");
            options = dyn.exp(options);
            options.query = new
            {
                filter,
                count,
                startIndex
            };
            return _connection.get(path, options);
        }

        /// <summary>
        /// Finds the user by identifier.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="options">The options.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">memberId</exception>
        public Task<dynamic> FindUserById(string memberId, dynamic options = null)
        {
            if (memberId == null)
                throw new ArgumentNullException(nameof(memberId));
            var path = util.createScimPath("Users", memberId);
            return _connection.get(path, options);
        }

        /// <summary>
        /// Replaces the user by identifier.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="msg">The MSG.</param>
        /// <param name="options">The options.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">memberId
        /// or
        /// msg</exception>
        public Task<dynamic> ReplaceUserById(string memberId, dynamic msg, dynamic options = null)
        {
            if (memberId == null)
                throw new ArgumentNullException(nameof(memberId));
            if (msg == null)
                throw new ArgumentNullException(nameof(msg));
            var path = util.createScimPath("Users", memberId);
            var data = new
            {
                schemas = dyn.getProp<string>(msg, "schemas"),
                userName = dyn.getProp<string>(msg, "userName"),
                name = dyn.getProp<string>(msg, "name"),
                emails = dyn.getProp<string>(msg, "emails"),
                displayName = dyn.getProp<string>(msg, "displayName"),
                timezone = dyn.getProp<string>(msg, "timezone"),
                preferredLanguage = dyn.getProp<string>(msg, "preferredLanguage"),
                groups = dyn.getProp<string>(msg, "groups"),
                active = dyn.getProp<bool>(msg, "active", true),
                scim__user = dyn.getProp<string>(msg, "scim__user"),
            };
            return _connection.putJson(path, data, options);
        }

        /// <summary>
        /// Modifies the user by identifier.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="msg">The MSG.</param>
        /// <param name="options">The options.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">memberId
        /// or
        /// msg</exception>
        public Task<dynamic> ModifyUserById(string memberId, dynamic msg, dynamic options = null)
        {
            if (memberId == null)
                throw new ArgumentNullException(nameof(memberId));
            if (msg == null)
                throw new ArgumentNullException(nameof(msg));
            var path = util.createScimPath("Users", memberId);
            options = dyn.exp(options);
            options.data = new
            {
                schemas = dyn.getProp<string[]>(msg, "schemas"),
                Operations = dyn.getProp<object[]>(msg, "Operations"),
            };
            return _connection.patch(path, options);
        }

        /// <summary>
        /// Gets the resource types.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> GetResourceTypes(dynamic options = null)
        {
            var path = util.createScimPath("ResourceTypes");
            return _connection.get(path, options);
        }
    }
}
