using Hootsuite.Require;
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
        /// Initializes a new instance of the <see cref="Messages" /> class.
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
        /// <exception cref="ArgumentNullException">msg</exception>
        public Task<dynamic> Schedule(dynamic msg)
        {
            if (msg == null)
                throw new ArgumentNullException(nameof(msg));
            var path = util.createPath("messages");
            var data = new
            {
                text = dyn.getProp<string>(msg, "text"),
                socialProfileIds = dyn.getProp<string[]>(msg, "socialProfileIds"),
                scheduledSendTime = dyn.hasProp(msg, "scheduledSendTime") ? dyn.getProp<DateTime>(msg, "scheduledSendTime").ToString("o") : null,
                webhookUrls = dyn.getProp<string[]>(msg, "webhookUrls"),
                tags = dyn.getProp<string[]>(msg, "tags"),
                targeting = dyn.getProp<string>(msg, "targeting"),
                privacy = dyn.getProp<string>(msg, "privacy"),
                location = dyn.getProp<string>(msg, "location"),
                emailNotification = dyn.getProp<bool>(msg, "emailNotification"),
                mediaUrls = dyn.getProp<string>(msg, "mediaUrls"),
                media = dyn.getProp<string>(msg, "media"),
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
        public Task<dynamic> Find(DateTime startTime, DateTime endTime, string[] socialProfileIds = null, dynamic opts = null)
        {
            var path = util.createPath("messages");
            var query = Restler.GetQuery(null, new
            {
                startTime = startTime.ToString("o"),
                endTime = endTime.ToString("o"),
                state = dyn.getProp<string>(opts, "state"),
                limit = dyn.getProp<string>(opts, "limit"),
                cursor = dyn.getProp<string>(opts, "cursor"),
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
        /// <exception cref="ArgumentNullException">messageId</exception>
        public Task<dynamic> FindById(string messageId)
        {
            if (messageId == null)
                throw new ArgumentNullException(nameof(messageId));
            var path = util.createPath("messages", messageId);
            return _connection.get(path);
        }

        /// <summary>
        /// Deletes the by identifier.
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <returns>Task&lt;JObject&gt;.</returns>
        /// <exception cref="ArgumentNullException">messageId</exception>
        public Task<dynamic> DeleteById(string messageId)
        {
            if (messageId == null)
                throw new ArgumentNullException(nameof(messageId));
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
        /// <exception cref="ArgumentNullException">messageId</exception>
        public Task<dynamic> ApproveById(string messageId, int sequenceNumber = 0, string reviewerType = null)
        {
            if (messageId == null)
                throw new ArgumentNullException(nameof(messageId));
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
        /// <exception cref="ArgumentNullException">messageId</exception>
        public Task<dynamic> RejectById(string messageId, string reason = null, int sequenceNumber = 0, string reviewerType = null)
        {
            if (messageId == null)
                throw new ArgumentNullException(nameof(messageId));
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
        /// <exception cref="ArgumentNullException">messageId</exception>
        public Task<dynamic> FindByIdHistory(string messageId)
        {
            if (messageId == null)
                throw new ArgumentNullException(nameof(messageId));
            var path = util.createPath("messages", messageId, "history");
            return _connection.get(path);
        }
    }
}
