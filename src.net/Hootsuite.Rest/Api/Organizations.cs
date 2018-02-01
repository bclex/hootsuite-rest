using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Hootsuite.Rest.Api
{
    public class Organizations
    {
        Hootsuite _hootsuite;
        Connection _connection;

        public Organizations(Hootsuite hootsuite, Connection connection)
        {
            _hootsuite = hootsuite;
            _connection = connection;
        }

        public Task<JObject> findMembers(string organizationId)
        {
            var path = util.createPath("organizations", organizationId, "members");
            return _connection.get(path);
        }

        public Task<JObject> removeMemberById(string organizationId, string memberId)
        {
            var path = util.createPath("organizations", organizationId, "members", memberId);
            return _connection.del(path);
        }

        public Task<JObject> findMemberByIdPermissions(string organizationId, string memberId)
        {
            var path = util.createPath("organizations", organizationId, "members", memberId, "permissions");
            return _connection.get(path);
        }

        public Task<JObject> findMemberByIdTeams(string organizationId, string memberId)
        {
            var path = util.createPath("organizations", organizationId, "members", memberId, "teams");
            return _connection.get(path);
        }

        public Task<JObject> findMemberByIdSocialProfiles(string organizationId, string memberId)
        {
            var path = util.createPath("organizations", organizationId, "members", memberId, "socialProfiles");
            return _connection.get(path);
        }

        public Task<JObject> findSocialProfilesByIdPermissions(string organizationId, string memberId, string socialProfileId)
        {
            var path = util.createPath("organizations", organizationId, "members", memberId, "socialProfiles", socialProfileId, "permissions");
            return _connection.get(path);
        }
    }
}
