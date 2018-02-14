using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace Hootsuite
{
    public class HootsuiteException : Exception
    {
        internal HootsuiteException(HttpStatusCode statusCode, JToken errors)
            : this(statusCode, (int)errors[0]["code"], (string)errors[0]["message"], (string)errors[0]["id"], (object)errors[0]["resource"]) { }
        internal HootsuiteException(HttpStatusCode statusCode, int code, string message, string id, object resource)
            : base(message)
        {
            StatusCode = statusCode;
            Code = code;
            Id = id;
            Resource = resource;
        }

        public HttpStatusCode StatusCode { get; set; }
        public int Code { get; set; }
        public string Id { get; set; }
        public object Resource { get; set; }
    }

    public class HootsuiteSecurityException : HootsuiteException
    {
        internal HootsuiteSecurityException(HttpStatusCode statusCode, JToken errors)
            : base(statusCode, errors) { }
    }
}