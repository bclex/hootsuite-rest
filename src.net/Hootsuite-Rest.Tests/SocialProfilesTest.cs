using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Hootsuite.Tests
{
    [TestClass, TestCategory("SocialProfiles")]
    public class SocialProfilesTest
    {
        readonly HootsuiteClient hootsuite;

        public SocialProfilesTest()
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
        public async Task SocialProfiles_Find()
        {
            // Retrieves the social profiles that the authenticated user has access to
            var res = await hootsuite.SocialProfiles.Find();
            Assert.IsTrue((int)res.data.Count > 0);
        }

        [TestMethod]
        public async Task SocialProfiles_FindById()
        {
            // Retrieve a social profile. Requires BASIC_USAGE permission on the social profile
            var res = await hootsuite.SocialProfiles.FindById("120732387");
            Assert.IsTrue((int)res.data.Count > 0);
        }

        [TestMethod]
        public async Task SocialProfiles_FindByIdTeams()
        {
            // Retrieves a list of team IDs with access to a social profile
            var res = await hootsuite.SocialProfiles.FindByIdTeams("120732387");
            Assert.IsTrue((int)res.data.Count > 0);
        }
    }
}
