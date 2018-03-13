using System;
using Newtonsoft.Json;

namespace Hootsuite.Domain
{
    /// <summary>
    /// Class EventMessage.
    /// </summary>
    public class EventMessage
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
        public EventMessageData Data { get; set; }
    }

    /// <summary>
    /// Class EventMessageData.
    /// </summary>
    public class EventMessageData
    {
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public MessageState State { get; set; }
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
}
