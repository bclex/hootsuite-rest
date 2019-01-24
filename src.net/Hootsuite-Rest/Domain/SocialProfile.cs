using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hootsuite.Domain
{
    /// <summary>
    /// Class SocialProfile.
    /// </summary>
    public class SocialProfile
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }
        /// <summary>
        /// Gets or sets the social network identifier.
        /// </summary>
        /// <value>The social network identifier.</value>
        public string SocialNetworkId { get; set; }
        /// <summary>
        /// Gets or sets the social network username.
        /// </summary>
        /// <value>The social network username.</value>
        public string SocialNetworkUsername { get; set; }
        /// <summary>
        /// Gets or sets the avatar URL.
        /// </summary>
        /// <value>The avatar URL.</value>
        public string AvatarUrl { get; set; }
        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public string Owner { get; set; }

        /// <summary>
        /// Froms the results.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>SocialProfile[].</returns>
        public static SocialProfile[] FromResults(JObject result) => result != null ? JsonConvert.DeserializeObject<SocialProfile[]>(result["data"].ToString(), HootsuiteClient.JsonSerializerSettings) : null;

        /// <summary>
        /// Froms the results single.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>SocialProfile.</returns>
        public static SocialProfile FromResultsSingle(JObject result) => result != null ? JsonConvert.DeserializeObject<SocialProfile>(result["data"].ToString(), HootsuiteClient.JsonSerializerSettings) : null;
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
