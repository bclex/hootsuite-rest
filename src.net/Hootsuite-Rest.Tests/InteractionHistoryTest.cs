using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Hootsuite.Tests
{
    [TestClass, TestCategory("InteractionHistory")]
    public class InteractionHistoryTest
    {
        readonly HootsuiteClient hootsuite;

        public InteractionHistoryTest()
        {
            HootsuiteClient.Logger = (x) => { Console.WriteLine(x); };
            hootsuite = new HootsuiteClient(new
            {
                clientId = Environment.GetEnvironmentVariable("HOOTSUITE_CLIENTID"),
                clientSecret = Environment.GetEnvironmentVariable("HOOTSUITE_CLIENTSECRET"),
                username = Environment.GetEnvironmentVariable("HOOTSUITE_USERNAME"),
                password = Environment.GetEnvironmentVariable("HOOTSUITE_PASSWORD"),
            });
        }

        [TestMethod]
        public async Task InteractionHistory_Find()
        {
            // Retrieves the social profiles that the authenticated user has access to
            var res = await hootsuite.InteractionHistory.FindAll("twitter", "120732387", "121181894");
            Assert.IsTrue((int)res.data.Count > 0);
        }
    }
}
