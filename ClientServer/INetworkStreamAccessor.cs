using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Jabber.ClientServer
{
    public interface INetworkStreamAccessor: IDisposable
    {        
        string ReadText();
        void WriteText(string message);
        void WriteBytes(byte[] info);
    }
}
