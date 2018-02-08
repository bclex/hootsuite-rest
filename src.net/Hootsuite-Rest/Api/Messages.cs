using Hootsuite.Require;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hootsuite.Api
{
    /// <summary>
    /// Class Messages.
    /// </summary>
    public class Messages
    {
        HootsuiteClient _hootsuite;
        Connection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Messages"/> class.
        /// </summary>
        /// <param name="hootsuite">The hootsuite.</param>
        /// <param name="connection">The connection.</param>
        public Messages(HootsuiteClient hootsuite, Connection connection)
        {
            _hootsuite = hootsuite;
            _connection = connection;
        }

        /// <summary>
        /// Schedules the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> Schedule(dynamic msg)
        {
            var path = util.createPath("messages");
            var data = new
            {
                text = dyn.getProp(msg, "text", (string)null),
                socialProfileIds = dyn.getProp(msg, "socialProfileIds", (string[])null),
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

        /// <summary>
        /// Finds the specified start time.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="socialProfileIds">The social profile ids.</param>
        /// <param name="opts">The opts.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> Find(DateTime startTime, DateTime endTime, string[] socialProfileIds, dynamic opts)
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

        /// <summary>
        /// Finds the by identifier.
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> FindById(string messageId)
        {
            var path = util.createPath("messages", messageId);
            return _connection.get(path);
        }

        /// <summary>
        /// Deletes the by identifier.
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> DeleteById(string messageId)
        {
            var path = util.createPath("messages", messageId);
            return _connection.del(path);
        }

        /// <summary>
        /// Approves the by identifier.
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="sequenceNumber">The sequence number.</param>
        /// <param name="reviewerType">Type of the reviewer.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> ApproveById(string messageId, int sequenceNumber = 0, string reviewerType = null)
        {
            var path = util.createPath("messages", messageId, "approve");
            var data = new
            {
                sequenceNumber,
                reviewerType,
            };
            return _connection.postJson(path, data);
        }

        /// <summary>
        /// Rejects the by identifier.
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="sequenceNumber">The sequence number.</param>
        /// <param name="reviewerType">Type of the reviewer.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> RejectById(string messageId, string reason = null, int sequenceNumber = 0, string reviewerType = null)
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

        /// <summary>
        /// Finds the by identifier history.
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        public Task<dynamic> FindByIdHistory(string messageId)
        {
            var path = util.createPath("messages", messageId, "history");
            return _connection.get(path);
        }
    }
}
