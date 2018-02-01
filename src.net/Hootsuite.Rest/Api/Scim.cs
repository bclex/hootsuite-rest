using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

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

        public Task<JObject> createUser(dynamic msg)
        {
            var path = util.createScimPath("Users");
            var data = new
            {
                schemas = dyn.getProp(msg, "schemas", (string)null),
                userName = dyn.getProp(msg, "userName", (string)null),
                name = dyn.getProp(msg, "name", (string)null),
                emails = dyn.getProp(msg, "emails", (string)null),
                displayName = dyn.getProp(msg, "displayName", (string)null),
                timezone = dyn.getProp(msg, "timezone", (string)null),
                preferredLanguage = dyn.getProp(msg, "preferredLanguage", (string)null),
                groups = dyn.getProp(msg, "groups", (string)null),
                active = dyn.getProp(msg, "active", true),
                //"urn:ietf:params:scim:schemas:extension:Hootsuite:2.0:User" = msg['urn:ietf:params:scim:schemas:extension:Hootsuite:2.0:User'] ?? null,
            };
            return _connection.postJson(path, data);
        }

        public Task<JObject> findUsers(string filter = null, int count = 0, int startIndex = 0)
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

        public Task<JObject> findUserById(string memberId)
        {
            var path = util.createScimPath("Users", memberId);
            return _connection.get(path);
        }

        public Task<JObject> replaceUserById(string memberId, dynamic msg)
        {
            var path = util.createScimPath("Users", memberId);
            var data = new
            {
                schemas = dyn.getProp(msg, "schemas", (string)null),
                userName = dyn.getProp(msg, "userName", (string)null),
                name = dyn.getProp(msg, "name", (string)null),
                emails = dyn.getProp(msg, "emails", (string)null),
                displayName = dyn.getProp(msg, "displayName", (string)null),
                timezone = dyn.getProp(msg, "timezone", (string)null),
                preferredLanguage = dyn.getProp(msg, "preferredLanguage", (string)null),
                groups = dyn.getProp(msg, "groups", (string)null),
                active = dyn.getProp(msg, "active", true),
                //'urn:ietf:params:scim:schemas:extension:Hootsuite:2.0:User': msg['urn:ietf:params:scim:schemas:extension:Hootsuite:2.0:User'] || undefined
            };
            return _connection.putJson(path, data);
        }

        public Task<JObject> modifyUserById(string memberId, dynamic msg)
        {
            var path = util.createScimPath("Users", memberId);
            var options = new
            {
                //headers = new { "content-type" = "application/json" },
                data = new
                {
                    schemas = dyn.getProp(msg, "schemas", (string)null),
                    Operations = dyn.getProp(msg, "Operations", (string)null),
                },
            };
            return _connection.patch(path, options);
        }

        public Task<JObject> getResourceTypes()
        {
            var path = util.createScimPath("ResourceTypes");
            return _connection.get(path);
        }
    }
}
