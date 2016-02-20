using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using Jabber.ClientServer;
namespace Jabber.Client
{
    /// <summary>
    /// Interaction logic for ConnectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : Window
    {
        public ConnectWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ServerAddress.Text.Length == 0)
                    throw new ArgumentException("The server address string is empty!");


                TcpWraper tcpWraper = new TcpWraper(GetServerHost(ServerAddress.Text), GetServerPort(ServerAddress.Text));
                IJabberProcessor jabberProcessor = new TcpJabberProcessor(tcpWraper);

                InteractionWindow interactWind = new InteractionWindow(jabberProcessor);
                interactWind.Show();
                this.Close();
            }
            catch (ArgumentException exc)
            {
                MessageBox.Show(exc.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (JabberException exc)
            {
                MessageBox.Show(exc.Message, "An exception has occured during initialization", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "The service is unaccessible", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        string GetServerHost(string serverAddress)
        {
            string[] parts = serverAddress.Split(':');
            return parts[0];
        }

        int GetServerPort(string serverAddress)
        {

            int port = 80;
            string[] parts = serverAddress.Split(':');
            if (parts.Length == 1)
                return port;
            else
                if (int.TryParse(parts[1], out port))
                return port;
            else
                throw new ArgumentException("Wrong server port");
        }
    }
}
