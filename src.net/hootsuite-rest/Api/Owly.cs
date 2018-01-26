using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Hootsuite.Rest.Api
{
    public class Owly
    {
        Hootsuite _hootsuite;
        ConnectionOwly _connection;

        public Owly(Hootsuite hootsuite, ConnectionOwly connection)
        {
            _hootsuite = hootsuite;
            _connection = connection;
        }

        public JToken shortenUrl(string longUrl)
        {
            var path = util.createOwlyPath("url", "shorten");
            return _connection.get(path, new { longUrl });
        }

        public JToken expandUrl(string shortUrl)
        {
            var path = util.createPath("url", "expand");
            return _connection.get(path, new { shortUrl });
        }

        public JToken getInfo(string shortUrl)
        {
            var path = util.createPath("url", "info");
            return _connection.get(path, new { shortUrl });
        }

        public JToken getClickStats(string shortUrl, DateTime? from, DateTime? to)
        {
            var path = util.createPath("url", "clickStats");
            return _connection.get(path, new { shortUrl, from, to });
        }

        public JToken uploadPhoto(string fileName, Stream uploaded_file)
        {
            var path = util.createPath("photo", "upload");
            return _connection.post(path, new { fileName, uploaded_file });
        }

        public JToken uploadDoc(string fileName, Stream uploaded_file)
        {
            var path = util.createPath("doc", "upload");
            return _connection.post(path, new { fileName, uploaded_file });
        }
    }
}
