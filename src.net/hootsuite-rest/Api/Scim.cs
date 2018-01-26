using Newtonsoft.Json.Linq;

namespace Hootsuite.Rest.Api
{
    public class Scim
    {
        Hootsuite _hootsuite;
        Connection _connection;

        public Scim(Hootsuite hootsuite, Connection connection)
        {
            _hootsuite = hootsuite;
            _connection = connection;
        }

        public JToken createUser(dynamic msg)
        {
            var path = util.createScimPath("Users");
            var data = new
            {
                schemas = msg.schemas ?? null,
                userName = msg.userName ?? null,
                name = msg.name ?? null,
                emails = msg.emails ?? null,
                displayName = msg.displayName ?? null,
                timezone = msg.timezone ?? null,
                preferredLanguage = msg.preferredLanguage ?? null,
                groups = msg.groups ?? null,
                active = msg.active ?? true,
                //"urn:ietf:params:scim:schemas:extension:Hootsuite:2.0:User" = msg['urn:ietf:params:scim:schemas:extension:Hootsuite:2.0:User'] ?? null,
            };
            return _connection.postJson(path, data);
        }

        public JToken findUsers(string filter = null, int count = 0, int startIndex = 0)
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

        public JToken findUserById(string memberId)
        {
            var path = util.createScimPath("Users", memberId);
            return _connection.get(path);
        }

        public JToken replaceUserById(string memberId, dynamic msg)
        {
            var path = util.createScimPath("Users", memberId);
            var data = new
            {
                schemas = msg.schemas ?? null,
                userName = msg.userName ?? null,
                name = msg.name ?? null,
                emails = msg.emails ?? null,
                displayName = msg.displayName ?? null,
                timezone = msg.timezone ?? null,
                preferredLanguage = msg.preferredLanguage ?? null,
                groups = msg.groups ?? null,
                active = msg.active ?? true,
                //'urn:ietf:params:scim:schemas:extension:Hootsuite:2.0:User': msg['urn:ietf:params:scim:schemas:extension:Hootsuite:2.0:User'] || undefined
            };
            return _connection.putJson(path, data);
        }

        public JToken modifyUserById(string memberId, dynamic msg)
        {
            var path = util.createScimPath("Users", memberId);
            var options = new
            {
                //headers = new { "content-type" = "application/json" },
                data = new
                {
                    schemas = msg.schemas ?? null,
                    Operations = msg.Operations ?? null,
                },
            };
            return _connection.patch(path, options);
        }

        public JToken getResourceTypes()
        {
            var path = util.createScimPath("ResourceTypes");
            return _connection.get(path);
        }
    }
}
