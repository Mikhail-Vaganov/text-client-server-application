using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Jabber.ClientServer;

namespace Jabber.Client
{
    public class TcpWraper : ITcpWraper
    {
        string serverHost;
        int serverPort;
        TcpClient _client;
        INetworkStreamAccessor _nsAccessor;
        public event Action<string> OnStatusChanged;
        int ConnectionAttemptsLimit = 10;
        bool isInitialized;

        public TcpWraper(string serverHost, int serverPort)
        {
            this.serverHost = serverHost;
            this.serverPort = serverPort;

            try
            {
                _client = new TcpClient();

                _client.Connect(serverHost, serverPort);
                if (!_client.Connected)
                    throw new JabberException($"Unable to connect to the server {serverHost}:{serverPort}");
                
                _nsAccessor = new NetworkStreamAccessorUTF8(_client.GetStream());
                isInitialized = true;
            }
            catch (SocketException exc)
            {
                throw new JabberException($"Failed to connect to a server using address {serverHost}:{serverPort}.", exc);
            }
        }

        void TryToReconnect()
        {
            for (int i = 0; i < ConnectionAttemptsLimit; i++)
                try
                {
                    _client = new TcpClient(serverHost, serverPort);
                    _nsAccessor = new NetworkStreamAccessorUTF8(_client.GetStream());
                    OnStatusChanged?.Invoke("The client has successfully reconnected to the server.");
                    return;
                }
                catch (SocketException exc)
                {
                    OnStatusChanged?.Invoke($"Failed to recconect #{i}");
                }

            throw new JabberException($"\nCannot connect to the server: {serverHost}:{serverPort}\nRestart the program.");
        }

        public string ReadText()
        {
            if (!_client.Connected)
                TryToReconnect();

            string text="";
            try
            {
                if (_client.Connected)
                {
                    text = _nsAccessor.ReadText();
                    return text;
                }
            }
            catch (System.IO.IOException exc)
            {
                OnStatusChanged?.Invoke(exc.Message);
                _client.Close();
            }
            catch (SocketException exc)
            {
                OnStatusChanged?.Invoke(exc.Message);
                _client.Close();
            }

            if(string.IsNullOrEmpty(text))
                text = "No data received from established connection";

            return text;
        }

        public bool IsConnected
        {
            get { return _client.Connected; }
        }

        public void WriteText(string message)
        {
            if (isInitialized)
                _nsAccessor.WriteText(message);
        }

        public void Dispose()
        {
            _nsAccessor?.Dispose();
            _client?.Close();
        }
    }

    
}
