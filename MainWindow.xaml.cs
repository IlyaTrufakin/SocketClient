using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SocketClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
   
        private ServerCommunication serverCommunication;
 
        public MainWindow()
        {
            InitializeComponent();
            serverCommunication = new ServerCommunication();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string connectionResult = serverCommunication.ConnectToServer(ipAddress.Text, portNumber.Text);
            OutputWindow.Text += connectionResult + Environment.NewLine;
            SendMessageButton.IsEnabled = serverCommunication.IsConnected();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SendMessageButton.IsEnabled = false;
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            if (serverCommunication.IsConnected())
            {
                string response = serverCommunication.SendMessage(InputWindow.Text);
                OutputWindow.Text += "Server answer: " + response + Environment.NewLine;
            }
            else
            {
                OutputWindow.Text += "Сообщение не отправлено: нет соединения с сервером" + Environment.NewLine;
            }
        }
    }
}