using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Hootsuite.Tests
{
    [TestClass, TestCategory("Owly")]
    public class OwlyTest
    {
        readonly HootsuiteClient hootsuite;

        public OwlyTest()
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
        [ExpectedException(typeof(HootsuiteSecurityException), "Insufficient permissions to view member permissions")]
        //[ExpectedException(typeof(HootsuiteException), "")]
        public async Task Organizations_FindMembers_Invalid()
        {
            // Retrieves the members in an organization
            var res = await hootsuite.Organizations.FindMembers("1234");
        }

        [TestMethod]
        [ExpectedException(typeof(HootsuiteSecurityException), "Insufficient permissions to view member permissions")]
        public async Task Organizations_FindMembers()
        {
            // Retrieves the members in an organization
            var res = await hootsuite.Organizations.FindMembers("814984");
            Assert.IsTrue((int)res.data.Count > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(HootsuiteSecurityException), "Insufficient permissions to view member permissions")]
        //[ExpectedException(typeof(HootsuiteException), "")]
        public async Task Messages_RemoveMemberById_Invalid()
        {
            // Removes a member from an organization
            var res = await hootsuite.Organizations.RemoveMemberById("814984", "1234");
        }

        [TestMethod]
        [ExpectedException(typeof(HootsuiteSecurityException), "Insufficient permissions to view member permissions")]
        //[ExpectedException(typeof(HootsuiteException), "")]
        public async Task Messages_FindMemberByIdPermissions_Invalid()
        {
            // Retrieves an organization member’s permissions for the organization
            var res = await hootsuite.Organizations.FindMemberByIdPermissions("814984", "1234");
        }

        [TestMethod]
        [ExpectedException(typeof(HootsuiteSecurityException), "Insufficient permissions to view member permissions")]
        //[ExpectedException(typeof(HootsuiteException), "")]
        public async Task Messages_FindMemberByIdTeams_Invalid()
        {
            // Retrieves the teams an organization member is in
            var res = await hootsuite.Organizations.FindMemberByIdTeams("814984", "1234");
        }

        [TestMethod]
        [ExpectedException(typeof(HootsuiteSecurityException), "Insufficient permissions to view member permissions")]
        //[ExpectedException(typeof(HootsuiteException), "")]
        public async Task Messages_FindMemberByIdSocialProfiles_Invalid()
        {
            // Retrieves the teams an organization member is in
            var res = await hootsuite.Organizations.FindMemberByIdSocialProfiles("814984", "1234");
        }

        [TestMethod]
        [ExpectedException(typeof(HootsuiteException), ": Member does not exist in organization")]
        public async Task Messages_FindSocialProfilesByIdPermissions_Invalid()
        {
            // Retrieves the teams an organization member is in
            var res = await hootsuite.Organizations.FindSocialProfilesByIdPermissions("814984", "1234", "1234");
        }
    }
}
