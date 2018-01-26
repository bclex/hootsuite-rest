using Newtonsoft.Json.Linq;

namespace Hootsuite.Rest.Api
{
    public class Me
    {
        Hootsuite _hootsuite;
        Connection _connection;

        public Me(Hootsuite hootsuite, Connection connection)
        {
            _hootsuite = hootsuite;
            _connection = connection;
        }

        public JToken get()
        {
            var path = util.createPath("me");
            return _connection.get(path);
        }

        public JToken getOrganizations()
        {
            var path = util.createPath("me", "organizations");
            return _connection.get(path);
        }

        public JToken getSocialProfiles()
        {
            var path = util.createPath("me", "socialProfiles");
            return _connection.get(path);
        }
    }
}
