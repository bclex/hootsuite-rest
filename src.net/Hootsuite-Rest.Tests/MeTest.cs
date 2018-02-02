using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;

namespace Hootsuite.Tests
{
    [TestClass]
    public class MeTest
    {
        readonly HootsuiteClient hootsuite;

        public MeTest()
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
        public async void CurrentUser_get()
        {
            // Retrieves authenticated member
            var res = await hootsuite.Me.Get();
            Assert.Equals(res["data"]["id"], "16494879");
            Assert.IsNotNull(res["data"]["fullName"]);
            Assert.IsNotNull(res["data"]["language"]);
        }

        [TestMethod]
        public async void CurrentUser_getOrganizations()
        {
            // Retrieves the organizations that the authenticated member is in
            var res = await hootsuite.Me.GetOrganizations();
            Assert.Equals(((JArray)res["data"]).Count, 1);
            Assert.Equals(res["data"][0]["id"], "814984");
            Assert.IsNotNull(res["data"][0], "id");
        }

        [TestMethod]
        public async void CurrentUser_getSocialProfiles()
        {
            // Retrieves the social media profiles that the authenticated user has BASIC_USAGE permissions on
            var res = await hootsuite.Me.GetSocialProfiles();
            Assert.Equals(((JArray)res["data"]).Count, 1);
            Assert.Equals(res["data"][0]["id"], "120732387");
            Assert.IsNotNull(res["data"][0], "id");
        }
    }
}
