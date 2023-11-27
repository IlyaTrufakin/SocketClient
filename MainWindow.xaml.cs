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

        private void StartClient(string ipAddress, string port)
        {
            try
            {
                IPEndPoint? endpoint = SocketInit(ipAddress, port);

                if (endpoint != null)
                {
                    OutputWindow.Text += "IPEndPoint успешно создан.\n";

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


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartClient(ipAddress.Text, portNumber.Text);
        }
    }
}