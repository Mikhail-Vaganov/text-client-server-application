using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Jabber.ClientServer;

namespace Jabber.Server
{
    class Program
    {
        static event Action<string, Guid> SendMessageToOneClient;
        static IFilesAccessor fileProcessor = FileProcessor.Instance;

        static void Main(string[] args)
        {
            Console.WriteLine("Server has started");

            TcpListener listener = null;

            try
            {
                int port = PreparePort(args);
                IPAddress localAddres = PrepareIPAddress(args);
               
                Console.WriteLine($"Listening {localAddres}:{port}");

                listener = new TcpListener(localAddres, port);
                listener.Start();
                                
                while (true)
                {
                    Console.WriteLine("Waiting for connection");

                    TcpClient newClient = listener.AcceptTcpClient();
                    Console.WriteLine("New incomming connection");

                    Task newConnection = Task.Factory.StartNew(() =>
                       {
                           Communicator communicator = PrepareUTF8Communicator(newClient);
                           communicator.StartListenInputStream();
                       }
                   );               
                       
                }
            }
            catch (SocketException exception)
            {
                Console.Beep();
                Console.WriteLine("SocketException: {0}", exception.Message);
            }
            catch (Exception exception)
            {
                Console.Beep();
                Console.WriteLine("Unpredicted exeption: {0}", exception.Message);
            }
            finally
            {
                listener?.Stop();
            }

            Console.WriteLine("Server has been stopped");
            Console.WriteLine();
            Console.WriteLine("Press \"Enter\" to close the application");
            Console.ReadLine();
        }

        private static IPAddress PrepareIPAddress(string[] args)
        {
            string serverHost = "127.0.0.1";

            if (args.Length > 0)
                serverHost = args[0];

            IPAddress serverAddres;
            if (serverHost == "localhost")
                serverAddres = IPAddress.Parse("127.0.0.1");
            else
            {
                IPHostEntry server = Dns.GetHostEntry(serverHost);
                serverAddres = server.AddressList[0];
            }

            return serverAddres;
        }

        private static int PreparePort(string[] args)
        {
            int port = 8080;
            int tempPort;
            if (args.Length > 1)
                if (int.TryParse(args[1], out tempPort))
                    port = tempPort;
            return port;
        }

        private static void SendMessageToAllClients(string message, Guid id)
        {
            fileProcessor.AddLineToFile(message);
            SendMessageToOneClient(message, id);
        }

        private static Communicator PrepareUTF8Communicator(TcpClient newClient)
        {
            INetworkStreamAccessor nsAccessor = new NetworkStreamAccessorUTF8(newClient.GetStream());
            var communicator = new Communicator(nsAccessor, fileProcessor);

            communicator.OnNewInputMessage += SendMessageToAllClients;
            SendMessageToOneClient += communicator.SendMessageToOneClient;

            communicator.OnDisposed += () =>
            {
                communicator.OnNewInputMessage -= SendMessageToAllClients;
                SendMessageToOneClient -= communicator.SendMessageToOneClient;
            };
            return communicator;
        }
    }
}
