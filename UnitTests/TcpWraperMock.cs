using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jabber.Client;
using Jabber.ClientServer;
namespace UnitTests
{
    class TcpWraperMock : ITcpWraper
    {
        public string TextToRead;
        public string WrittenText;
        public bool ConnectionStatus=true;
        public bool IsDisposed = false;
        public bool IsConnected
        {
            get
            {
                return ConnectionStatus;
            }
        }

        public event Action<string> OnStatusChanged;

        public void Dispose()
        {
            IsDisposed = true;
        }

        public string ReadText()
        {
            return TextToRead;
        }

        public void WriteText(string message)
        {
            WrittenText = message;
        }
    }
}
