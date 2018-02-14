using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Hootsuite.Tests
{
    [TestClass, TestCategory("Members")]
    public class MembersTest
    {
        readonly HootsuiteClient hootsuite;

        public MembersTest()
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
        [ExpectedException(typeof(HootsuiteSecurityException), "Not authorized to view member")]
        public async Task Members_FindById_Invalid()
        {
            // Retrieves a member
            var res = await hootsuite.Members.FindById("1234");
        }

        [TestMethod, Ignore("Throws Security Error")]
        public async Task Members_FindById()
        {
            // Retrieves a member
            var res = await hootsuite.Members.FindById("16494879");
            Assert.IsNotNull((string)res.data.id);
            //Assert.IsNotNull((string)res.data.uploadUrl);
        }

        [TestMethod]
        [ExpectedException(typeof(HootsuiteSecurityException), "Not authorized to make changes to organization")]
        public async Task Members_Create()
        {
            // Creates a member in a Hootsuite organization. Requires organization manage members permission
            var res = await hootsuite.Members.Create(new
            {
                organizationIds = new[] { "814984" },
                email = "test@test.com",
                fullName = "test full"
            });
            //Assert.IsNotNull((string)res.data.id);
        }

        [TestMethod]
        public async Task Members_FindByIdOrgs()
        {
            // Retrieves the organizations that the member is in
            var res = await hootsuite.Members.FindByIdOrgs("16494879");
            Assert.IsTrue((int)res.data.Count > 0);
            Assert.IsNotNull((string)res.data[0].id);
        }
    }
}
