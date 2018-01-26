using Newtonsoft.Json.Linq;

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

        public JToken findById(string memberId)
        {
            var path = util.createPath("members", memberId);
            return _connection.get(path);
        }

        public JToken create(dynamic msg)
        {
            var path = util.createPath("members");
            var data = new
            {
                organizationIds = msg.organizationIds ?? new string[0],
                email = msg.email ?? null,
                fullName = msg.fullName ?? null,
                companyName = msg.companyName ?? null,
                bio = msg.bio ?? null,
                timezone = msg.timezone ?? null,
                language = msg.language ?? null,
            };
            return _connection.postJson(path, data);
        }

        public JToken findByIdOrgs(string memberId)
        {
            var path = util.createPath("members", memberId, "organizations");
            return _connection.get(path);
        }
    }
}
