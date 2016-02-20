using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jabber.Client;
using Jabber.ClientServer;
using Jabber.Server;

namespace UnitTests
{
    [TestClass]
    public class CommunicatorUnitTest
    {
        NetworkStreamHelperMock _nsAccessor;
        FileAccessorMock _fAccessor;
        [TestInitialize()]
        public void TestInitialize()
        {
            _nsAccessor = new NetworkStreamHelperMock();
            _fAccessor = new FileAccessorMock();
        }


        [TestMethod]
        public void TestSendMessage()
        {
            Communicator c = new Communicator(_nsAccessor, _fAccessor);
            string message = "try to connect";
            c.SendMessageToOneClient(message, Guid.Empty);

            Assert.AreEqual(message, _nsAccessor.WrittenText);
        }

        [TestMethod]
        public void TestOnDisposedMessage()
        {
            Communicator c = new Communicator(_nsAccessor, _fAccessor);
            string testMessage = "onDisposedTest";
            string reaction = "";
            c.OnDisposed += () => reaction = testMessage;

            Assert.AreNotEqual(reaction, testMessage);
            c.Dispose();
            Assert.AreEqual(reaction, testMessage);
        }

        [TestMethod]
        public void TestReadMessage()
        {
            Communicator c = new Communicator(_nsAccessor, _fAccessor);
            string testMessage = "onInputTextTest";
            string reaction = "";
            c.OnNewInputMessage += (a, b) => reaction = testMessage;

            Assert.AreNotEqual(reaction, testMessage);
            c.StartListenInputStream();
            Assert.AreEqual(reaction, testMessage);
        }

        [TestMethod]
        public void TestGetLogCommand()
        {
            _nsAccessor.TextToRead = "/getlog";
            _nsAccessor.WrittenBytes = new byte[0];
            _fAccessor.BytesToReturn = new byte[] { 1, 2, 3, 4, 5, 6, 7 };

            Communicator c = new Communicator(_nsAccessor, _fAccessor);
            Assert.AreNotEqual(_nsAccessor.WrittenBytes, _fAccessor.BytesToReturn);
            c.StartListenInputStream();
            Assert.AreEqual(_nsAccessor.WrittenBytes, _fAccessor.BytesToReturn);
        }

        [TestMethod]
        public void TestUniquenessOfComs()
        {
            Communicator c1 = new Communicator(_nsAccessor, _fAccessor);
            Communicator c2 = new Communicator(_nsAccessor, _fAccessor);
            Assert.AreNotEqual(c1.Id, c2.Id);
        }
    }
}
