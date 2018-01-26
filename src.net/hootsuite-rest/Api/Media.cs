using Newtonsoft.Json.Linq;

namespace Hootsuite.Rest.Api
{
    public class Media
    {
        Hootsuite _hootsuite;
        Connection _connection;

        public Media(Hootsuite hootsuite, Connection connection)
        {
            _hootsuite = hootsuite;
            _connection = connection;
        }

        public JToken createUrl(int sizeBytes, string mimeType)
        {
            var path = util.createPath("media");
            var data = new
            {
                sizeBytes,
                mimeType = mimeType ?? "video/mp4",
            };
            return _connection.postJson(path, data);
        }

        public JToken statusById(string mediaId)
        {
            var path = util.createPath("media", mediaId);
            return _connection.get(path);
        }
    }
}
