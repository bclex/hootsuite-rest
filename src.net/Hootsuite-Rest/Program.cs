using System;
using System.Threading.Tasks;

namespace Hootsuite
{
    public static class Program
    {
        public static void Main(params string[] args)
        {
            //HootsuiteClient.Logger = (x) => { Console.WriteLine(x); };
            var hootsuite = new HootsuiteClient(new
            {
                //accessToken = "ebc624a5-a5bf-446e-bacd-115968b1237e",
                clientId = Environment.GetEnvironmentVariable("HOOTSUITE_CLIENTID"),
                clientSecret = Environment.GetEnvironmentVariable("HOOTSUITE_CLIENTSECRET"),
                username = Environment.GetEnvironmentVariable("HOOTSUITE_USERNAME"),
                password = Environment.GetEnvironmentVariable("HOOTSUITE_PASSWORD"),
            });

            //var res = hootsuite.Me.Get().Result;
            var res = hootsuite.Me.GetOrganizations().Result;
            //var res = hootsuite.Me.GetSocialProfiles().Result;
            Console.WriteLine($"{res}");

            //Task.Run(() => MainAsync(hootsuite)).Wait();
        }

        async static void MainAsync(HootsuiteClient hootsuite)
        {
            var res = await hootsuite.Me.Get();
            Console.WriteLine($"{res}");
        }
    }
}
