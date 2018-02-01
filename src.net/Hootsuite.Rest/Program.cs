using System;

namespace Hootsuite.Rest
{
    public static class Program
    {
        public static void Main(params string[] args)
        {
            util.logger = (x) => { Console.WriteLine(x); };
            var hootsuite = new Hootsuite(new
            {
                accessToken = "ebc624a5-a5bf-446e-bacd-115968b1237e",
                clientId = Environment.GetEnvironmentVariable("HOOTSUITE_CLIENTID"),
                clientSecret = Environment.GetEnvironmentVariable("HOOTSUITE_CLIENTSECRET"),
                username = Environment.GetEnvironmentVariable("HOOTSUITE_USERNAME"),
                password = Environment.GetEnvironmentVariable("HOOTSUITE_PASSWORD"),
            });
            try
            {
                var a = hootsuite.me.get().ContinueWith(x =>
                {
                    Console.WriteLine(x.Result);
                    return "OK";
                });
                var b = a.Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            //hootsuite.me.get().then(function(response) {
            //    assert.equal(response.data.id, '16494879');
            //    assert(_.has(response.data, 'fullName'));
            //    assert(_.has(response.data, 'language'));
            //    done();
            //}).catch (done);
        }
    }
}
