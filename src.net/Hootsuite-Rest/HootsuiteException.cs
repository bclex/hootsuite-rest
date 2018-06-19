using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace Hootsuite
{
    /// <summary>
    /// Class HootsuiteException.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class HootsuiteException : Exception
    {
        internal HootsuiteException(HttpStatusCode statusCode, dynamic error)
            : this(statusCode, GetErrorCode((object)(error.code ?? error.statusCode)), (string)(error.message ?? error.error_description), (string)error.id, (object)error.resource) { Error = error; }
        internal HootsuiteException(HttpStatusCode statusCode, int code, string message, string id, object resource)
            : base(message)
        {
            StatusCode = statusCode;
            Code = code;
            Id = id;
            Resource = resource;
        }

        static int GetErrorCode(object obj)
        {
            if (obj == null)
                return 0;

            if (obj is int) {
                return (int) obj;
            }

            return int.TryParse(obj.ToString(), out var i) ? i : 0;
        }
        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>The status code.</value>
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public int Code { get; set; }
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        /// <value>The resource.</value>
        public object Resource { get; set; }
        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>The errors.</value>
        public JToken Error { get; set; }
    }

    /// <summary>
    /// Class HootsuiteSecurityException.
    /// </summary>
    /// <seealso cref="Hootsuite.HootsuiteException" />
    public class HootsuiteSecurityException : HootsuiteException
    {
        internal HootsuiteSecurityException(HttpStatusCode statusCode, JObject error)
            : base(statusCode, error) { }
    }
}