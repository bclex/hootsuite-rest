using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Hootsuite.Rest.Api
{
    public class Members
    {
        Hootsuite _hootsuite;
        Connection _connection;

        public Members(Hootsuite hootsuite, Connection connection)
        {
            _hootsuite = hootsuite;
            _connection = connection;
        }

        public Task<JObject> findById(string memberId)
        {
            var path = util.createPath("members", memberId);
            return _connection.get(path);
        }

        public Task<JObject> create(dynamic msg)
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

        public Task<JObject> findByIdOrgs(string memberId)
        {
            var path = util.createPath("members", memberId, "organizations");
            return _connection.get(path);
        }
    }
}
