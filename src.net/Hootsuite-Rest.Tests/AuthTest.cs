using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Hootsuite.Tests
{
    [TestClass, TestCategory("Auth")]
    public class AuthTest
    {
        readonly HootsuiteClient hootsuite;

        public AuthTest()
        {
            HootsuiteClient.Logger = (x) => { Console.WriteLine(x); };
            hootsuite = new HootsuiteClient(new
            {
                accessToken = "none",
                clientId = Environment.GetEnvironmentVariable("HOOTSUITE_CLIENTID"),
                clientSecret = Environment.GetEnvironmentVariable("HOOTSUITE_CLIENTSECRET"),
                username = Environment.GetEnvironmentVariable("HOOTSUITE_USERNAME"),
                password = Environment.GetEnvironmentVariable("HOOTSUITE_PASSWORD"),
            });
        }

        [TestMethod]
        public void Auth_GetAndSetAccessToken()
        {
            Assert.AreEqual("none", hootsuite.AccessToken);
            hootsuite.AccessToken = "test";
            Assert.AreEqual("test", hootsuite.AccessToken);
        }

        [TestMethod]
        public async Task Auth_Attempt()
        {
            var authCalls = 0;
            hootsuite.OnAccessToken = x => { authCalls++; };
            hootsuite.AccessToken = "none";
            var me = await hootsuite.Me.Get();
            Assert.IsNotNull(me);
            Assert.AreEqual(1, authCalls);
        }
    }
}
