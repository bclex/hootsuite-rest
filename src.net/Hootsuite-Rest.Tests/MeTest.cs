using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Hootsuite.Tests
{
    [TestClass, TestCategory("Me")]
    public class MeTest
    {
        readonly HootsuiteClient hootsuite;

        public MeTest()
        {
            HootsuiteClient.Logger = (x) => { Console.WriteLine(x); };
            hootsuite = new HootsuiteClient(new
            {
                accessToken = "71f08d13-d442-439f-a6b0-fdc20f216f67",
                clientId = Environment.GetEnvironmentVariable("HOOTSUITE_CLIENTID"),
                clientSecret = Environment.GetEnvironmentVariable("HOOTSUITE_CLIENTSECRET"),
                username = Environment.GetEnvironmentVariable("HOOTSUITE_USERNAME"),
                password = Environment.GetEnvironmentVariable("HOOTSUITE_PASSWORD"),
            });
        }

        [TestMethod]
        public async Task CurrentUser_Get()
        {
            // Retrieves authenticated member
            var res = await hootsuite.Me.Get();
            Assert.AreEqual("16494879", (string)res.data.id);
            Assert.IsNotNull((string)res.data.fullName);
            Assert.IsNotNull((string)res.data.language);
        }

        [TestMethod]
        public async Task CurrentUser_GetOrganizations()
        {
            // Retrieves the organizations that the authenticated member is in
            var res = await hootsuite.Me.GetOrganizations();
            Assert.IsTrue(res.data.Count >= 1);
            Assert.AreEqual("814984", (string)res.data[0].id);
            Assert.IsNotNull((string)res.data[0].id);
        }

        [TestMethod]
        public async Task CurrentUser_GetSocialProfiles()
        {
            // Retrieves the social media profiles that the authenticated user has BASIC_USAGE permissions on
            var res = await hootsuite.Me.GetSocialProfiles();
            Assert.IsTrue(res.data.Count >= 1);
            Assert.AreEqual("120732387", (string)res.data[0].id);
            Assert.IsNotNull((string)res.data[0].id);
        }
    }
}
