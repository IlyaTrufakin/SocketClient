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
        private static int port = 8005;
        private static string IPAdress = "127.0.0.1";
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private static void SocketInit()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(IPAdress), port);
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            clientSocket.Connect(ipPoint);

            Console.WriteLine("Enter data to send: ");
            string message = Console.ReadLine();
            byte[] sendData = Encoding.Unicode.GetBytes(message);
            clientSocket.Send(sendData);

            StringBuilder recievedString = new StringBuilder();
            int recievedBytes = 0;
            byte[] receiveData = new byte[4096];

            do
            {
                recievedBytes = clientSocket.Receive(receiveData, receiveData.Length, 0);
                recievedString.Append(Encoding.Unicode.GetString(receiveData, 0, recievedBytes));
            } while (clientSocket.Available > 0);

            Console.WriteLine("Server answer: " + recievedString.ToString());
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            Console.WriteLine("Socket close...");
        }
    }
}