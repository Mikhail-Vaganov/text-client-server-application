using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jabber.Client;

namespace UnitTests
{
    /// <summary>
    /// Summary description for TcpJabberProcessorUnitTests
    /// </summary>
    [TestClass]
    public class TcpJabberProcessorUnitTest
    {

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        public void TestSendMEssageToStream()
        {
            TcpWraperMock _twMock = new TcpWraperMock();
            TcpJabberProcessor tcpProcessor = new TcpJabberProcessor(_twMock);
            string messageToSend = "testMessage";
            Assert.AreNotEqual(_twMock.WrittenText, messageToSend);
            tcpProcessor.SendMessage(messageToSend);
            Assert.AreEqual(_twMock.WrittenText, messageToSend);
        }

        [TestMethod]
        public void TestTcpWraperDisposed()
        {
            TcpWraperMock _twMock = new TcpWraperMock();
            TcpJabberProcessor tcpProcessor = new TcpJabberProcessor(_twMock);
            tcpProcessor.Dispose();
            Assert.IsTrue(_twMock.IsDisposed);
        }
    }
}
