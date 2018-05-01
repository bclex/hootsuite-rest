using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hootsuite.Domain
{
    /// <summary>
    /// Class Owly.
    /// </summary>
    public class Owly
    {
        /// <summary>
        /// Class Upload
        /// </summary>
        public class Upload
        {
            /// <summary>
            /// Froms the results.
            /// </summary>
            /// <param name="result"></param>
            /// <returns></returns>
            public static Result FromResults(JObject result) => result != null ? JsonConvert.DeserializeObject<Result>(result["results"].ToString(), HootsuiteClient.JsonSerializerSettings) : null;

            /// <summary>
            /// 
            /// </summary>
            public class Result
            {
                /// <summary>
                /// 
                /// </summary>
                public string Hash { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string Caption { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string Url { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string Score { get; set; }
            }
        }

        /// <summary>
        /// Class Expand
        /// </summary>
        public class Expand
        {
            /// <summary>
            /// Froms the results.
            /// </summary>
            /// <param name="result"></param>
            /// <returns></returns>
            public static Result FromResults(JObject result) => result != null ? JsonConvert.DeserializeObject<Result>(result["results"].ToString(), HootsuiteClient.JsonSerializerSettings) : null;

            /// <summary>
            /// Class ExpandResult
            /// </summary>
            public class Result
            {
                /// <summary>
                /// 
                /// </summary>
                public string Hash { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string ShortUrl { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string LongUrl { get; set; }
            }
        }

        /// <summary>
        /// Class Shorten
        /// </summary>
        public class Shorten
        {
            /// <summary>
            /// Froms the results.
            /// </summary>
            /// <param name="result"></param>
            /// <returns></returns>
            public static Result FromResults(JObject result) =>
                result != null ? JsonConvert.DeserializeObject<Result>(result["results"].ToString(), HootsuiteClient.JsonSerializerSettings) : null;

            /// <summary>
            /// Class ExpandResult
            /// </summary>
            public class Result
            {
                /// <summary>
                /// 
                /// </summary>
                public string Hash { get; set; }

                /// <summary>
                /// 
                /// </summary>
                public string ShortUrl { get; set; }

                /// <summary>
                /// 
                /// </summary>
                public string LongUrl { get; set; }
            }
        }

        /// <summary>
        /// Class Expand
        /// </summary>
        public class Info
        {
            /// <summary>
            /// Froms the results.
            /// </summary>
            /// <param name="result"></param>
            /// <returns></returns>
            public static Result FromResults(JObject result) => result != null ? JsonConvert.DeserializeObject<Result>(result["results"].ToString(), HootsuiteClient.JsonSerializerSettings) : null;

            /// <summary>
            /// Class ExpandResult
            /// </summary>
            public class Result
            {
                /// <summary>
                /// 
                /// </summary>
                public string Hash { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string ShortUrl { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string LongUrl { get; set; }

                /// <summary>
                /// 
                /// </summary>
                public int Votes { get; set; }

                /// <summary>
                /// 
                /// </summary>
                public int TotalClicks { get; set; }
            }
        }

        /// <summary>
        /// Class ClickStats
        /// </summary>
        public class ClickStats
        {
            /// <summary>
            /// Froms the results.
            /// </summary>
            /// <param name="result"></param>
            /// <returns></returns>
            public static Result[] FromResults(JObject result) => result != null ? JsonConvert.DeserializeObject<Result[]>(result["results"].ToString(), HootsuiteClient.JsonSerializerSettings) : null;

            /// <summary>
            /// Class ExpandResult
            /// </summary>
            public class Result
            {
                /// <summary>
                /// 
                /// </summary>
                public DateTime Date { get; set; }
                
                /// <summary>
                /// 
                /// </summary>
                public int ClickCount { get; set; }
            }
        }
    }
}
