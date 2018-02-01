using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hootsuite.Rest
{
    public static class errors
    {
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
            //  hasErrorCode(err, errorCodes.SYSTEM_ERROR);
        }

        public static bool isExpiredToken(Exception err)
        {
            return false;
            //return hasErrorCode(err, errorCodes.TOKEN_ERROR);
        }

        public static bool isRateLimited(Exception err)
        {
            return false;
            //return hasErrorCode(err, errorCodes.RATE_LIMIT_REACHED);
        }
    }
}
