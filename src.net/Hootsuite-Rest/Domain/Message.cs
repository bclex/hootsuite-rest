using Newtonsoft.Json;
using System;

namespace Hootsuite.Domain
{
    public class Message
    {
        public long Id { get; set; }
        public MessageState State { get; set; }
        public DateTime ScheduledSendTime { get; set; }
        public string Text { get; set; }
        public bool EmailNotification { get; set; }
        [JsonConverter(typeof(SocialProfile.ArrayConverter))]
        public SocialProfile[] SocialProfile { get; set; }
        public string PostUrl { get; set; }
        public string PostId { get; set; }
        public Media[] Media { get; set; }
        public MediaUrl[] MediaUrls { get; set; }
        public string[] WebhookUrls { get; set; }
        public Reviewer[] Reviewers { get; set; }
        public string[] Privacy { get; set; }
        public Location Location { get; set; }
        public string[] Targeting { get; set; }
        public string[] Tags { get; set; }
        public int SequenceNumber { get; set; }
        public Member CreatedByMember { get; set; }
    }
}
