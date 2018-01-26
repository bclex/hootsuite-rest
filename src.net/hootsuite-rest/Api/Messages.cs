using Hootsuite.Rest.Require;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

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

        public JToken schedule(dynamic msg)
        {
            var path = util.createPath("messages");
            var data = new
            {
                text = msg.text ?? null,
                socialProfileIds = msg.socialProfileIds ?? null,
                scheduledSendTime = msg.scheduledSendTime != null ? ((DateTime)msg.scheduledSendTime).ToString("o") : null,
                webhookUrls = msg.webhookUrls ?? null,
                tags = msg.tags ?? null,
                targeting = msg.targeting ?? null,
                privacy = msg.privacy ?? null,
                location = msg.location ?? null,
                emailNotification = msg.emailNotification ?? null,
                mediaUrls = msg.mediaUrls ?? null,
                media = msg.media ?? null,
            };
            return _connection.postJson(path, data);
        }

        public JToken find(DateTime startTime, DateTime endTime, string[] socialProfileIds, dynamic opts)
        {
            var path = util.createPath("messages");
            var query = Restler.GetQuery(null, new
            {
                startTime = startTime.ToString("o"),
                endTime = endTime.ToString("o"),
                state = opts.state ?? null,
                limit = opts.limit ?? null,
                cursor = opts.cursor ?? null,
            });
            if (socialProfileIds != null)
                query += string.Join(string.Empty, socialProfileIds.Select(x => $"&socialProfileIds=${Uri.EscapeDataString(x)}").ToArray());
            var options = new
            {
                query
            };
            return _connection.get(path, options);
        }

        public JToken findById(string messageId)
        {
            var path = util.createPath("messages", messageId);
            return _connection.get(path);
        }

        public JToken deleteById(string messageId)
        {
            var path = util.createPath("messages", messageId);
            return _connection.del(path);
        }

        public JToken approveById(string messageId, int sequenceNumber = 0, string reviewerType = null)
        {
            var path = util.createPath("messages", messageId, "approve");
            var data = new
            {
                sequenceNumber,
                reviewerType,
            };
            return _connection.postJson(path, data);
        }

        public JToken rejectById(string messageId, string reason = null, int sequenceNumber = 0, string reviewerType = null)
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

        public JToken findByIdHistory(string messageId)
        {
            var path = util.createPath("messages", messageId, "history");
            return _connection.get(path);
        }
    }
}
