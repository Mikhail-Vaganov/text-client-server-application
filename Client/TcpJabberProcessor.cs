using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Threading;
using Jabber.ClientServer;

namespace Jabber.Client
{
    public class TcpJabberProcessor : IJabberProcessor
    {

        CancellationTokenSource cts;
        object conversationLocker = new Object();
        ITcpWraper _tcpWraper;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        string _conversation;
        public string Conversation
        {
            get
            {
                lock (conversationLocker)
                {
                    return _conversation;
                }
            }
            set
            {
                lock (conversationLocker)
                {
                    _conversation = value;
                    OnPropertyChanged("Conversation");
                }
            }
        }

        public TcpJabberProcessor(ITcpWraper tcpWraper)
        {
            this._tcpWraper = tcpWraper;
            _tcpWraper.OnStatusChanged += AddTextToConversation;

            cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;
            Task readingInfo = Task.Factory.StartNew(() => StreamReading(ct), ct);

        }
        public void StreamReading(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested == true)
                {
                    AddTextToConversation("The application cancelled reading the input stream");
                    break;
                }

                try
                {
                    string text = _tcpWraper.ReadText();
                    AddTextToConversation(text);
                }
                catch (Exception exc)
                {
                    AddTextToConversation("Unpredicted exception.");
                    AddTextToConversation(exc.Message);
                    AddTextToConversation("Restart the application");
                    return;
                }
            }
        }

        public void SendMessage(string message)
        {
            if (!_tcpWraper.IsConnected)
            {
                AddTextToConversation("The connection was lost!");
                AddTextToConversation("Restart the application");
                return;
            }
            
            _tcpWraper.WriteText(message);
        }

        private void AddTextToConversation(string message)
        {
            Conversation += $"{message}{Environment.NewLine}";
        }

        public void Dispose()
        {
            cts.Cancel();
            _tcpWraper.Dispose();
        }
    }
}
