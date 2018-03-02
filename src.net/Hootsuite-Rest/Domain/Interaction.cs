namespace Hootsuite.Domain
{
    public class Interaction
    {
        public long SocialNetworkId { get; set; }
        public long TargetSocialNetworkId { get; set; }
        public string SocialNetworkType { get; set; }
        public long MessageId { get; set; }
        public string Type { get; set; }
    }
}
