using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jabber.ClientServer;
using Jabber.Server;

namespace UnitTests
{
    class NetworkStreamHelperMock : INetworkStreamAccessor
    {
        public string WrittenText;
        public string TextToRead="test";
        private int NumberOfRead = 0;
        private const int MaxNumberOfRead = 1;
        public byte[] WrittenBytes;

        public void Dispose()
        {
            return;
        }

        public string ReadText()
        {
            if (NumberOfRead >= MaxNumberOfRead)
                throw new JabberException("StopReading");
            NumberOfRead++;
            
                return TextToRead;
        }

        public void WriteBytes(byte[] info)
        {
            WrittenBytes = info;
            return;
        }

        public void WriteText(string message)
        {
            WrittenText = message;
            return;
        }
    }
}
