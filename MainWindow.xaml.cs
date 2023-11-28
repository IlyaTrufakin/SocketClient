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
        private bool ServerConnect = false;
        private Socket? clientSocket;
        public MainWindow()
        {
            InitializeComponent();
        }

        private static IPEndPoint SocketInit(string ipAddress, string port)
        {
            try
            {
                if (!int.TryParse(port, out int portNumber))
                {
                    throw new ArgumentException("Порт задан неверно (должен быть числом)");
                }

                return new IPEndPoint(IPAddress.Parse(ipAddress), portNumber);
            }
            catch (FormatException ex)
            {
                throw new FormatException("Некорректный формат IP-адреса", ex);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("ошибка аргумента: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private bool StartClient(string ipAddress, string port)
        {
            try
            {
                IPEndPoint? endpoint = SocketInit(ipAddress, port);

                if (endpoint != null)
                {
                    OutputWindow.Text += "IPEndPoint успешно создан.\n";
                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    try
                    {
                        clientSocket.Connect(endpoint);
                        OutputWindow.Text += "Соединение с сервером установлено\n";
                        ScrollTextBlock.ScrollToEnd();
                        return true;
                    }
                    catch (SocketException ex)
                    {
                        // Обработка исключений сокета при установлении соединения
                        OutputWindow.Text += "Ошибка соединения: " + ex.Message + Environment.NewLine;
                    }

                }
                else
                {
                    OutputWindow.Text += "IPEndPoint не создан из-за ошибки в методе SocketInit.\n";
                }
            }

            catch (Exception ex)
            {
                OutputWindow.Text += "Произошла ошибка: " + ex.Message + Environment.NewLine;
            }
            ScrollTextBlock.ScrollToEnd();
            return false;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ServerConnect = StartClient(ipAddress.Text, portNumber.Text);
            if (ServerConnect)
            {
                SendMessageButton.IsEnabled = true;
            }
            else 
            { SendMessageButton.IsEnabled = false; }
        }

  

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SendMessageButton.IsEnabled = false;
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            if (ServerConnect && clientSocket != null && clientSocket.Connected)
            {
                byte[] sendData = Encoding.Unicode.GetBytes(InputWindow.Text);
                clientSocket.Send(sendData);
                OutputWindow.Text += "Отправлено сообщение: " + InputWindow.Text + Environment.NewLine;

                byte[] receiveData = new byte[256];
                StringBuilder receivedString = new StringBuilder();
                int bytesReceived;

                while (true)
                {
                    bytesReceived = clientSocket.Receive(receiveData);
                    receivedString.Append(Encoding.Unicode.GetString(receiveData, 0, bytesReceived));

                    if (bytesReceived < receiveData.Length)
                    {
                        break; // Выход из цикла, если больше данных не ожидается
                    }
                }

                OutputWindow.Text += "Server answer: " + receivedString.ToString() + Environment.NewLine;
            }
            else
            {
                OutputWindow.Text += "Сообщение не отправлено: нет соединения с сервером" + Environment.NewLine;
            }
        }
    }
}