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
using System.Windows.Shapes;
using System.Net.Sockets;
namespace Jabber.Client
{
    /// <summary>
    /// Interaction logic for InteractionWindow.xaml
    /// </summary>
    public partial class InteractionWindow : Window
    {
        readonly IJabberProcessor _jabberProcessor;

        public static RoutedCommand SendCommand = new RoutedCommand();
        private void ExecuteSendCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SendMessageToServer(sender,e);
        }
        private void CanExecuteSendCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = OutputSpan.Text.Length > 0;
        }

        public InteractionWindow(IJabberProcessor jabberProcessor)
        {
            this._jabberProcessor = jabberProcessor;
            this.DataContext = jabberProcessor;
            this.Closing += InteractionWindow_Closing;
            InitializeComponent();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            var sendComandBinding = new CommandBinding(SendCommand, ExecuteSendCommand, CanExecuteSendCommand);
            CommandBindings.Add(sendComandBinding);
            var keyBinding = new KeyBinding(SendCommand, Key.Enter, ModifierKeys.Alt);
            InputBindings.Add(keyBinding);
        }

        private void InteractionWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _jabberProcessor.Dispose();
        }

        private void SendMessageToServer(object sender, RoutedEventArgs e)
        {
            try
            {
                _jabberProcessor.SendMessage(OutputSpan.Text);
                OutputSpan.Text = "";
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "An error has occured during sending the message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GetLog(object sender, RoutedEventArgs e)
        {
            try
            {
                _jabberProcessor.SendMessage("/getlog");
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "An error has occured during getting the log file", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
