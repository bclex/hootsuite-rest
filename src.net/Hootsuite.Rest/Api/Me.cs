using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

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

        public Task<JObject> get()
        {
            var path = util.createPath("me");
            return _connection.get(path);
        }

        public Task<JObject> getOrganizations()
        {
            var path = util.createPath("me", "organizations");
            return _connection.get(path);
        }

        public Task<JObject> getSocialProfiles()
        {
            var path = util.createPath("me", "socialProfiles");
            return _connection.get(path);
        }
    }
}
