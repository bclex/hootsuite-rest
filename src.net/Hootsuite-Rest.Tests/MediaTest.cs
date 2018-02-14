using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Hootsuite.Tests
{
    [TestClass, TestCategory("Media")]
    public class MediaTest
    {
        readonly HootsuiteClient hootsuite;

        public MediaTest()
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
        public async Task Media_CreateUrl()
        {
            // Create media upload url
            var res = await hootsuite.Media.CreateUrl(50000);
            Assert.IsNotNull((string)res.data.id);
            Assert.IsNotNull((string)res.data.uploadUrl);
        }

        [TestMethod]
        public async Task CurrentUser_StatusById()
        {
            // Retrieves the status of a media upload to Hootsuite
            var res = await hootsuite.Media.StatusById("aHR0cHM6Ly9ob290c3VpdGUtdmlkZW8uczMuYW1hem9uYXdzLmNvbS9wcm9kdWN0aW9uLzE2NDk0ODc5X2E2Yzg4YzY0LTIwYmEtNGE0My05NDlmLTI5N2EzMmM2MDlmMi5tcDQ");
            Assert.IsNotNull((string)res.data.id);
        }
    }
}
