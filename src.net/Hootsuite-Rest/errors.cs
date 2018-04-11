using Hootsuite.Require;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Hootsuite
{
    internal static class errors
    {
        enum errorCodes
        {
            TOKEN_ERROR = 1032,
            SYSTEM_ERROR = 5000,
            RATE_LIMIT_REACHED = 1003,
            ACCESS_DENIED = 5000,
            QUOTA_REACHED = 5000,
        };

        static bool hasPlatformErrorCode(Exception err, int code)
        {
            return isPlatformError(err, out JArray errors) &&
                errors.FirstOrDefault(x => (int)x["code"] == code) != null;
        }

        static bool isPlatformError(Exception err, out JArray errors)
        {
            if (err is HootsuiteException)
            {
                errors = ((HootsuiteException)err).Errors;
                return true;
            }
            // other error
            errors = null;
            JObject res;
            if (!(err is RestlerOperationException) || (res = ((RestlerOperationException)err).Content as JObject) == null)
                return false;
            var val = res["errors"] != null &&
                res["errors"] is JArray &&
                ((JArray)res["errors"]).Count > 0;
            if (val)
                errors = (JArray)res["errors"];
            return val;
        }

        public static bool isNetworkError(Exception err)
        {
            return false;
            //var code = err["code"] || '';
            //return retryableNetworkErrorCodes.indexOf(code) > -1 || (err instanceof TimeoutError);
        }

        public static bool isServerError(Exception err)
        {
            return false;
            //return (err is Http5XXError) ||
            //  // hasErrorCode(err, errorCodes.TIMED_OUT) ||
            //  // hasErrorCode(err, errorCodes.API_UNAVAILBLE) ||
            // hasErrorCode(err, errorCodes.SYSTEM_ERROR);
        }

        public static bool isExpiredToken(Exception err)
        {
            return err.Message.Contains("Token expired") ||
                hasPlatformErrorCode(err, (int)errorCodes.TOKEN_ERROR);
        }

        public static bool isRateLimited(Exception err)
        {
            return hasPlatformErrorCode(err, (int)errorCodes.RATE_LIMIT_REACHED);
        }
    }
}
