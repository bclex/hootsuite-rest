using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Hootsuite.Rest.Api
{
    public class SocialProfiles
    {
        Hootsuite _hootsuite;
        Connection _connection;

        public SocialProfiles(Hootsuite hootsuite, Connection connection)
        {
            _hootsuite = hootsuite;
            _connection = connection;
        }

        public Task<JObject> find()
        {
            var path = util.createPath("socialProfiles");
            return _connection.get(path);
        }

        public Task<JObject> findById(string socialProfileId)
        {
            var path = util.createPath("socialProfiles", socialProfileId);
            return this._connection.get(path);
        }

        public Task<JObject> findByIdTeams(string socialProfileId)
        {
            var path = util.createPath("socialProfiles", socialProfileId, "teams");
            return _connection.get(path);
        }
    }
}
