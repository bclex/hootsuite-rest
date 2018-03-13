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
        internal HootsuiteException(HttpStatusCode statusCode, JArray errors)
            : this(statusCode, (int)errors[0]["code"], (string)errors[0]["message"], (string)errors[0]["id"], (object)errors[0]["resource"]) { Errors = errors; }
        internal HootsuiteException(HttpStatusCode statusCode, int code, string message, string id, object resource)
            : base(message)
        {
            StatusCode = statusCode;
            Code = code;
            Id = id;
            Resource = resource;
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
        public JArray Errors { get; set; }
    }

    /// <summary>
    /// Class HootsuiteSecurityException.
    /// </summary>
    /// <seealso cref="Hootsuite.HootsuiteException" />
    public class HootsuiteSecurityException : HootsuiteException
    {
        internal HootsuiteSecurityException(HttpStatusCode statusCode, JArray errors)
            : base(statusCode, errors) { }
    }
}