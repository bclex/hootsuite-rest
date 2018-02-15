using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Hootsuite.Tests
{
    [TestClass, TestCategory("Scim")]
    public class ScimTest
    {
        readonly HootsuiteClient hootsuite;

        public ScimTest()
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
        public async Task Scim_CreateUser()
        {
            // Creates a Hootsuite user using the SCIM 2.0 protocol
            var res = await hootsuite.Scim.CreateUser(new
            {
                schemas = new[] {
                    "urn:ietf:params:scim:schemas:core:2.0:User",
                    "urn:ietf:params:scim: schemas: extension: Hootsuite: 2.0:User" },
                userName = "joe.smith@hootsuite.com",
                name = new { givenName = "joe.smith", familyName = "joe.smith" },
                emails = new[] {
                    new { primary = true, value = "joe.smith@hootsuite.com", type = "work" } },
                displayName = "Joe Smith",
                timezone = "America/Vancouver",
                preferredLanguage = "en",
                groups = new object[] { },
                active = true,
                scim__user = new
                {
                    organizationIds = new[] { 38938 },
                    teamIds = new[] { 436875 }
                }
            });
        }

        [TestMethod]
        public async Task Scim_FindUsers()
        {
            // Retrieves Hootsuite users using the SCIM 2.0 protocol. Support equals filtering on username
            var res = await hootsuite.Scim.FindUsers();
            Assert.IsTrue((int)res.Resources.Count >= 0);
        }

        [TestMethod]
        public async Task Scim_FindUserById()
        {
            // Retrieves a Hootsuite user using the SCIM 2.0 protocol
            var res = await hootsuite.Scim.FindUserById("16494879");
            Assert.IsTrue((int)res.data.Count > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(HootsuiteException), "abc")]
        public async Task Scim_ReplaceUserById_Invalid()
        {
            // Updates a Hootsuite user using the SCIM 2.0 protocol
            var res = await hootsuite.Scim.ReplaceUserById("1234", new { });
        }

        [TestMethod]
        [ExpectedException(typeof(HootsuiteException), "abc")]
        public async Task Scim_ModifyUserById_Invalid()
        {
            // Modify one or more attributes of a Hootsuite user
            var res = await hootsuite.Scim.ModifyUserById("1234", new { });
        }

        [TestMethod]
        public async Task Scim_GetResourceTypes()
        {
            // Retrieves the configuration for all supported SCIM resource types
            var res = await hootsuite.Scim.GetResourceTypes();
            Assert.IsTrue((int)res.Resources.Count > 0);
            Assert.AreEqual("User", (string)res.Resources[0].id);
            Assert.IsNotNull((string)res.Resources[0].name);
            Assert.IsNotNull((object)res.Resources[0].meta);
        }
    }
}
