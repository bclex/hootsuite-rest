using Newtonsoft.Json.Linq;

namespace Hootsuite.Rest.Api
{
    public class Teams
    {
        Hootsuite _hootsuite;
        Connection _connection;

        public Teams(Hootsuite hootsuite, Connection connection)
        {
            _hootsuite = hootsuite;
            _connection = connection;
        }

        public JToken createTeam(string organizationId, string teamName)
        {
            var path = util.createPath("organizations", organizationId, "teams");
            var data = new
            {
                teamName
            };
            return _connection.postJson(path, data);
        }

        public JToken appendMemberById(string organizationId, string teamId, string memberId)
        {
            var path = util.createPath("organizations", organizationId, "teams", teamId, "members", memberId);
            return _connection.post(path);
        }

        public JToken findByIdMembers(string organizationId, string teamId)
        {
            var path = util.createPath("organizations", organizationId, "teams", teamId, "members");
            return _connection.get(path);
        }

        public JToken findMemberByIdPermissions(string organizationId, string teamId, string memberId)
        {
            var path = util.createPath("organizations", organizationId, "teams", teamId, "members", memberId, "permissions");
            return _connection.get(path);
        }

        public JToken findByIdSocialProfiles(string organizationId, string teamId)
        {
            var path = util.createPath("organizations", organizationId, "teams", teamId, "socialProfiles");
            return _connection.get(path);
        }
    }
}
