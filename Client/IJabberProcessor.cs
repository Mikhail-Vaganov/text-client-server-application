using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Jabber.Client
{
    public interface IJabberProcessor : IDisposable, INotifyPropertyChanged
    {
        string Conversation { get; }
        void SendMessage(string message);
    }
}
