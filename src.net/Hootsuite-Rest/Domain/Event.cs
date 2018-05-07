using System;
using Newtonsoft.Json;

namespace Hootsuite.Domain
{
    /// <summary>
    /// Class Event.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Gets or sets the sequence number.
        /// </summary>
        /// <value>The sequence number.</value>
        [JsonProperty("seq_no")]
        public string SequenceNumber { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public EventData Data { get; set; }

        /// <summary>
        /// Froms the results.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <returns>Event[].</returns>
        public static Event[] FromResults(string results) => results != null ? JsonConvert.DeserializeObject<Event[]>(results, HootsuiteClient.JsonSerializerSettings) : null;

        /// <summary>
        /// Class EventMessageData.
        /// </summary>
        public class EventData
        {
            /// <summary>
            /// Gets or sets the state.
            /// </summary>
            /// <value>The state.</value>
            public EventState State { get; set; }
            /// <summary>
            /// Gets or sets the organization.
            /// </summary>
            /// <value>The organization.</value>
            public Organization Organization { get; set; }
            /// <summary>
            /// Gets or sets the message.
            /// </summary>
            /// <value>The message.</value>
            public Message Message { get; set; }
            /// <summary>
            /// Gets or sets the timestamp.
            /// </summary>
            /// <value>The timestamp.</value>
            public DateTime Timestamp { get; set; }
        }

        /// <summary>
        /// Enum EventState
        /// </summary>
        public enum EventState
        {
            /// <summary>
            /// The scheduled
            /// </summary>
            SCHEDULED,
            /// <summary>
            /// The pending approval
            /// </summary>
            PENDING_APPROVAL,
            /// <summary>
            /// The approved
            /// </summary>
            APPROVED,
            /// <summary>
            /// The submitted
            /// </summary>
            SUBMITTED,
            /// <summary>
            /// The sent
            /// </summary>
            SENT,
            /// <summary>
            /// The send failed permanently
            /// </summary>
            SEND_FAILED_PERMANENTLY,
            /// <summary>
            /// The deleted
            /// </summary>
            DELETED,
            /// <summary>
            /// The rejected
            /// </summary>
            REJECTED
        }
    }
}
