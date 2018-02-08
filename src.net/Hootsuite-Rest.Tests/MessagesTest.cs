using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Hootsuite.Tests
{
    [TestClass, TestCategory("Messages")]
    public class MessagesTest
    {
        readonly HootsuiteClient hootsuite;

        public MessagesTest()
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
        public async Task Messages_Schedule()
        {
            // Schedules a message to send on one or more social profiles. Returns an array of uniquely identifiable messages (one per social profile requested)
            var res = await hootsuite.Messages.Schedule(new
            {
                text = "sent now",
                socialProfileIds = new[] { "120732387" },
            });
            Assert.AreEqual(1, (int)res.data.Length);
            //Assert.AreEqual("16494879", (string)res.data.Length);
            //Assert.IsNotNull((string)res.data.fullName);
            //Assert.IsNotNull((string)res.data.language);

            // Schedules a message to send on one or more social profiles. Returns an array of uniquely identifiable messages (one per social profile requested)
            var res2 = await hootsuite.Messages.Schedule(new
            {
                text = "sent in future",
                socialProfileIds = new[] { "120732387" },
                scheduledSendTime = DateTime.Parse("2020-01-01"),
            });
            Assert.AreEqual(1, (int)res.data.Length);
            //Assert.AreEqual("16494879", (string)res.data.Length);
            //Assert.IsNotNull((string)res.data.fullName);
            //Assert.IsNotNull((string)res.data.language);
        }
    }
}
