using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hootsuite.Domain
{
    public class Interaction
    {
        public long FromSocialNetworkId { get; set; }
        public long ToSocialNetworkId { get; set; }
        public string SocialNetworkType { get; set; }
        public long ExternalMessageId { get; set; }
        public string Type { get; set; }

        public static Interaction[] FromResults(JObject result) => result != null ? JsonConvert.DeserializeObject<Interaction[]>(result["data"]["interactions"].ToString(), HootsuiteClient.JsonSerializerSettings) : null;
    }
}
