using Hootsuite.Rest.Require;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hootsuite.Rest.Api
{
    public class Messages
    {
        Hootsuite _hootsuite;
        Connection _connection;

        public Messages(Hootsuite hootsuite, Connection connection)
        {
            _hootsuite = hootsuite;
            _connection = connection;
        }

        public Task<JObject> schedule(dynamic msg)
        {
            var path = util.createPath("messages");
            var data = new
            {
                text = dyn.getProp(msg, "text", (string)null),
                socialProfileIds = dyn.getProp(msg, "socialProfileIds", (string)null),
                scheduledSendTime = dyn.hasProp(msg, "scheduledSendTime") ? ((DateTime)msg.scheduledSendTime).ToString("o") : null,
                webhookUrls = dyn.getProp(msg, "msg.webhookUrls", (string)null),
                tags = dyn.getProp(msg, "tags", (string)null),
                targeting = dyn.getProp(msg, "targeting", (string)null),
                privacy = dyn.getProp(msg, "privacy", (string)null),
                location = dyn.getProp(msg, "location", (string)null),
                emailNotification = dyn.getProp(msg, "emailNotification", (string)null),
                mediaUrls = dyn.getProp(msg, "mediaUrls", (string)null),
                media = dyn.getProp(msg, "media", (string)null),
            };
            return _connection.postJson(path, data);
        }

        public Task<JObject> find(DateTime startTime, DateTime endTime, string[] socialProfileIds, dynamic opts)
        {
            var path = util.createPath("messages");
            var query = Restler.GetQuery(null, new
            {
                startTime = startTime.ToString("o"),
                endTime = endTime.ToString("o"),
                state = dyn.getProp(opts, "state", (string)null),
                limit = dyn.getProp(opts, "limit", (string)null),
                cursor = dyn.getProp(opts, "cursor", (string)null),
            });
            if (socialProfileIds != null)
                query += string.Join(string.Empty, socialProfileIds.Select(x => $"&socialProfileIds=${Uri.EscapeDataString(x)}").ToArray());
            var options = new
            {
                query
            };
            return _connection.get(path, options);
        }

        public Task<JObject> findById(string messageId)
        {
            var path = util.createPath("messages", messageId);
            return _connection.get(path);
        }

        public Task<JObject> deleteById(string messageId)
        {
            var path = util.createPath("messages", messageId);
            return _connection.del(path);
        }

        public Task<JObject> approveById(string messageId, int sequenceNumber = 0, string reviewerType = null)
        {
            var path = util.createPath("messages", messageId, "approve");
            var data = new
            {
                sequenceNumber,
                reviewerType,
            };
            return _connection.postJson(path, data);
        }

        public Task<JObject> rejectById(string messageId, string reason = null, int sequenceNumber = 0, string reviewerType = null)
        {
            var path = util.createPath("messages", messageId, "reject");
            var data = new
            {
                reason,
                sequenceNumber,
                reviewerType,
            };
            return _connection.postJson(path, data);
        }

        public Task<JObject> findByIdHistory(string messageId)
        {
            var path = util.createPath("messages", messageId, "history");
            return _connection.get(path);
        }
    }
}
