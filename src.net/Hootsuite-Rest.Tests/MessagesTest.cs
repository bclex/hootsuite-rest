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
            Assert.IsTrue((int)res.data.Count > 0);
            Assert.IsNotNull((string)res.data[0].id);
            Assert.AreEqual("SENT", (string)res.data.state);
            Assert.IsNotNull((string)res.data.text);

            // Schedules a message to send on one or more social profiles. Returns an array of uniquely identifiable messages (one per social profile requested)
            var res2 = await hootsuite.Messages.Schedule(new
            {
                text = "sent in future",
                socialProfileIds = new[] { "120732387" },
                scheduledSendTime = DateTime.Parse("2020-01-01"),
            });
            Assert.IsTrue((int)res2.data.Count > 0);
            Assert.IsNotNull((string)res2.data[0].id);
            Assert.AreEqual("SCHEDULED", (string)res2.data.state);
            Assert.IsNotNull((string)res2.data.text);
        }

        [TestMethod]
        public async Task Messages_Find()
        {
            // Retrieve outbound messages
            var res = await hootsuite.Messages.FindAll(DateTime.Parse("2017-12-01"), DateTime.Parse("2017-12-15"));
            Assert.IsTrue((int)res.data.Count > 0);
            Assert.IsNotNull((string)res.data[0].id);
            Assert.AreEqual("SENT", (string)res.data[0].state);
            Assert.IsNotNull((string)res.data[0].text);
        }

        [TestMethod]
        [ExpectedException(typeof(HootsuiteException), "Unable to find the message by id")]
        public async Task Messages_FindById_Invalid()
        {
            // Retrieves a message. A message is always associated with a single social profile. Messages might be unavailable for a brief time during upload to social networks
            var res = await hootsuite.Messages.FindById("1234");
        }

        [TestMethod]
        public async Task Messages_FindById()
        {
            // Retrieves a message. A message is always associated with a single social profile. Messages might be unavailable for a brief time during upload to social networks
            var res = await hootsuite.Messages.FindById("4950332802");
            Assert.IsTrue((int)res.data.Count == 1);
            Assert.AreEqual("4950332802", (string)res.data[0].id);
            Assert.AreEqual("SENT", (string)res.data[0].state);
            Assert.IsNotNull((string)res.data[0].text);
        }

        [TestMethod]
        [ExpectedException(typeof(HootsuiteException), "Unable to find the message by id")]
        public async Task Messages_DeleteId_Invalid()
        {
            // Deletes a message. A message is always associated with a single social profile
            var res = await hootsuite.Messages.DeleteById("1234");
            //Assert.IsTrue((int)res.data.Count == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(HootsuiteException), "Message is not awaiting approval")]
        public async Task Messages_ApproveById_Invalid()
        {
            // Approve a message
            var res = await hootsuite.Messages.ApproveById("4950332802");
            //Assert.IsTrue((int)res.data.Count == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(HootsuiteException), "Message is not awaiting approval")]
        public async Task Messages_RejectById_Invalid()
        {
            // Reject a message
            var res = await hootsuite.Messages.RejectById("4950332802");
            //Assert.IsTrue((int)res.data.Count == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(HootsuiteException), "Message is not awaiting approval")]
        public async Task Messages_FindByIdHistory_Invalid()
        {
            // Gets a message’s prescreening review history
            var res = await hootsuite.Messages.FindByIdHistory("4950332802");
            //Assert.IsTrue((int)res.data.Count == 1);
        }
    }
}
