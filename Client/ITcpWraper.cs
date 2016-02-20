using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jabber.Client
{
    public interface ITcpWraper:IDisposable
    {
        bool IsConnected { get; }
        void WriteText(string message);
        event Action<string> OnStatusChanged;
        string ReadText();
    }
}
