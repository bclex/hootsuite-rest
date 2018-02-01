using System;
using System.Net;

namespace Hootsuite.Rest.Require
{
    public class RestlerOperationException : Exception
    {
        public RestlerOperationException(HttpStatusCode statusCode, object content) : base("Restler exception")
        {
            StatusCode = statusCode;
            Content = content;
        }

        public HttpStatusCode StatusCode { get; set; }
        public object Content { get; set; }
        public Exception E { get; set; }
        public bool Timedout { get; set; }
    }
}