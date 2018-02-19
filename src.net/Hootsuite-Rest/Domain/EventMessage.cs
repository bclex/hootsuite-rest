using System;
using Newtonsoft.Json;

namespace Hootsuite.Domain
{
    public class EventMessage
    {
        [JsonProperty("seq_no")]
        public string SequenceNumber { get; set; }
        public string Type { get; set; }
        public EventMessageData Data { get; set; }
    }

    public class EventMessageData
    {
        public MessageState State { get; set; }
        public Organization Organization { get; set; }
        public Message Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
