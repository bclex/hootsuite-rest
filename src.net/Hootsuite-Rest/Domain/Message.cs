using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Hootsuite.Domain
{
    /// <summary>
    /// Class Message.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public MessageState State { get; set; }
        /// <summary>
        /// Gets or sets the scheduled send time.
        /// </summary>
        /// <value>The scheduled send time.</value>
        public DateTime? ScheduledSendTime { get; set; }
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [email notification].
        /// </summary>
        /// <value><c>true</c> if [email notification]; otherwise, <c>false</c>.</value>
        public bool? EmailNotification { get; set; }
        /// <summary>
        /// Gets or sets the social profile.
        /// </summary>
        /// <value>The social profile.</value>
        [JsonConverter(typeof(SocialProfile.ArrayConverter))]
        public SocialProfile[] SocialProfile { get; set; }
        /// <summary>
        /// Gets or sets the post URL.
        /// </summary>
        /// <value>The post URL.</value>
        public string PostUrl { get; set; }
        /// <summary>
        /// Gets or sets the post identifier.
        /// </summary>
        /// <value>The post identifier.</value>
        public string PostId { get; set; }
        /// <summary>
        /// Gets or sets the media.
        /// </summary>
        /// <value>The media.</value>
        public Media[] Media { get; set; }
        /// <summary>
        /// Gets or sets the media urls.
        /// </summary>
        /// <value>The media urls.</value>
        public MediaUrl[] MediaUrls { get; set; }
        /// <summary>
        /// Gets or sets the webhook urls.
        /// </summary>
        /// <value>The webhook urls.</value>
        public string[] WebhookUrls { get; set; }
        /// <summary>
        /// Gets or sets the reviewers.
        /// </summary>
        /// <value>The reviewers.</value>
        public Reviewer[] Reviewers { get; set; }
        /// <summary>
        /// Gets or sets the privacy.
        /// </summary>
        /// <value>The privacy.</value>
        public string[] Privacy { get; set; }
        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public Location Location { get; set; }
        /// <summary>
        /// Gets or sets the targeting.
        /// </summary>
        /// <value>The targeting.</value>
        public string[] Targeting { get; set; }
        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>The tags.</value>
        public string[] Tags { get; set; }
        /// <summary>
        /// Gets or sets the sequence number.
        /// </summary>
        /// <value>The sequence number.</value>
        public int SequenceNumber { get; set; }
        /// <summary>
        /// Gets or sets the created by member.
        /// </summary>
        /// <value>The created by member.</value>
        public Member CreatedByMember { get; set; }

        /// <summary>
        /// Froms the results.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>Message[].</returns>
        public static Message[] FromResults(JObject result) => result != null ? JsonConvert.DeserializeObject<Message[]>(result["data"].ToString(), HootsuiteClient.JsonSerializerSettings) : null;

        /// <summary>
        /// Clones the by social profile.
        /// </summary>
        /// <returns>IEnumerable&lt;Message&gt;.</returns>
        public IEnumerable<Message> CloneBySocialProfile()
        {
            foreach (var socialProfile in SocialProfile)
            {
                var msg = DeepClone();
                msg.SocialProfile = new[] { socialProfile };
                yield return msg;
            }
        }

        /// <summary>
        /// Deeps the clone.
        /// </summary>
        /// <returns>Message.</returns>
        public Message DeepClone()
        {
            var serialized = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<Message>(serialized);
        }

        /// <summary>
        /// Gets the time until send.
        /// </summary>
        /// <value>The time until send.</value>
        public TimeSpan TimeUntilSend { get => (ScheduledSendTime ?? DateTime.UtcNow) - DateTime.UtcNow; }

        /// <summary>
        /// Gets the debug string.
        /// </summary>
        /// <returns>System.String.</returns>
        public string ToDebugString()
        {
            var b = new StringBuilder();
            b.Append($"Message ID[{Id}] ");
            b.Append($"$Seq[{SequenceNumber}] ");
            if (CreatedByMember != null)
                b.Append($"Member[{CreatedByMember?.Id}] ");
            if (SocialProfile != null)
                b.Append($"SocialProfile[{SocialProfile?.FirstOrDefault()?.Id}] ");
            b.Append($"State[{State}] ");
            if (ScheduledSendTime.HasValue)
            {
                b.Append($"Schedule[{ScheduledSendTime:o}] ");
                b.Append($"TimeUntilSend[{TimeUntilSend}] ");
            }
            b.Append($"Characters[{Text?.Length ?? 0}]\n");
            return b.ToString();
        }
    }
}
