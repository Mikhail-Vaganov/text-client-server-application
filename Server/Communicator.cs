using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Jabber.ClientServer;

namespace Jabber.Server
{
    public class Communicator : IDisposable
    {

        public Guid Id { get; } = Guid.NewGuid();

        readonly INetworkStreamAccessor _networkStreamAccessor;
        IFilesAccessor _fileAcessor;        
        public event Action<string, Guid> OnNewInputMessage;
        public event Action OnDisposed;
        object streamWriteLocker = new object();

        public Communicator(INetworkStreamAccessor networkStreamAccessor, IFilesAccessor fileAcessor)
        {
            this._fileAcessor = fileAcessor;
            this._networkStreamAccessor = networkStreamAccessor;
        }

        public void StartListenInputStream()
        {
            while (true)
            {
                try
                {
                    string message = _networkStreamAccessor.ReadText();

                    if (string.IsNullOrEmpty(message))
                        continue;

                    if (message.Length > 0 && message[0] == '/')
                        ProcessCommand(message);
                    else
                    {
                        if (message.StartsWith(@"\"))
                            message = message.Substring(1);

                        OnNewInputMessage?.Invoke(message, Id);
                    }
                }
                catch(Exception exc)
                {
                    Console.WriteLine($"An error has occurred during listening an input stream: {exc.Message}");
                    return;
                }
            }
        }

        private void ProcessCommand(string commandString)
        {
            if (commandString == "/getlog")
                try
                {

                    byte[] bytes = _fileAcessor.GetBytes();
                    _networkStreamAccessor.WriteBytes(bytes);
                }
                catch (System.IO.IOException exc)
                {
                    Dispose();
                }
        }

        public void SendMessageToOneClient(string message, Guid id)
        {
            if (this.Id != id)
            {
                try
                {
                    _networkStreamAccessor.WriteText(message);
                }
                catch (System.IO.IOException exc)
                {
                    Dispose();
                }
            }
        }

        public void Dispose()
        {
            OnDisposed?.Invoke();
            _networkStreamAccessor?.Dispose();
        }
    }
}
