using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Jabber.ClientServer
{
    public class NetworkStreamAccessorUTF8 : INetworkStreamAccessor
    {
        UTF8Encoding encoder = new UTF8Encoding();
        NetworkStream _stream;
        byte[] buffer = new byte[1024];
        object streamWriteLocker = new object();

        public NetworkStreamAccessorUTF8(NetworkStream stream)
        {
            _stream = stream;
        }
        public string ReadText()
        {
            int i;
            string data = "";
            
            while (_stream.CanRead  && (i = _stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                data = encoder.GetString(buffer, 0, i);
                return data;
            }

            if (!_stream.DataAvailable)
                throw new JabberException("Data from the stream is not available!");

            return data;
        }

        public void WriteText(string message)
        {
            byte[] bytes = encoder.GetBytes(message);
            WriteBytes(bytes);
        }

        public void WriteBytes(byte[] bytes)
        {
            lock (streamWriteLocker)
            {
                _stream.Write(bytes, 0, bytes.Length);
            }
        }

        public void Dispose()
        {
            _stream?.Dispose();
        }
    }
}
