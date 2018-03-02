using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hootsuite.Domain
{
    public class SocialProfile
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string SocialNetworkId { get; set; }
        public string SocialNetworkUsername { get; set; }
        public string AvatarUrl { get; set; }
        public string Owner { get; set; }

        public static T FromResults<T>(JObject result)
        {
            return JsonConvert.DeserializeObject<T>(result["data"].ToString(), HootsuiteClient.JsonSerializerSettings);
        }

        internal class ArrayConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType) { return false; }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.StartArray)
                    return serializer.Deserialize(reader, objectType);
                var item = JObject.Load(reader);
                if (item["id"] != null)
                    return new[] { new SocialProfile { Id = item["id"].ToString() } };
                throw new Exception();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var t = JToken.FromObject(value);
                if (t.Type != JTokenType.Object)
                {
                    t.WriteTo(writer);
                    return;
                }
                var o = (JObject)t;
                var propertyNames = o.Properties().Select(p => p.Name).ToList();
                o.AddFirst(new JProperty("Keys", new JArray(propertyNames)));
                o.WriteTo(writer);
            }
        }
    }
}
