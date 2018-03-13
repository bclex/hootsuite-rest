using System.Threading.Tasks;

namespace Hootsuite.Api
{
    /// <summary>
    /// Class Me.
    /// </summary>
    public class Me
    {
        readonly HootsuiteClient _hootsuite;
        readonly Connection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Me" /> class.
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
        /// <param name="options">The options.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> Get(dynamic options = null)
        {
            var path = util.createPath("me");
            return _connection.get(path, options);
        }

        /// <summary>
        /// Gets the organizations.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> GetOrganizations(dynamic options = null)
        {
            var path = util.createPath("me", "organizations");
            return _connection.get(path, options);
        }

        /// <summary>
        /// Gets the social profiles.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> GetSocialProfiles(dynamic options = null)
        {
            var path = util.createPath("me", "socialProfiles");
            return _connection.get(path, options);
        }
    }
}
