﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SocketClient
{
    public class ServerCommunication
    {
        private Socket? clientSocket;

        public string ConnectToServer(string ipAddress, string port)
        {
            try
            {
                IPEndPoint endpoint = SocketInit(ipAddress, port);
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(endpoint);
                return "Соединение с сервером установлено";
            }
            catch (Exception ex)
            {
                return "Ошибка соединения с сервером: " + ex.Message;
            }
        }

        public bool IsConnected()
        {
            return clientSocket != null && clientSocket.Connected;
        }

        public string SendMessage(string message)
        {
            try
            {
                byte[] sendData = Encoding.Unicode.GetBytes(message);
                clientSocket.Send(sendData);

                byte[] receiveData = new byte[256];
                StringBuilder receivedString = new StringBuilder();
                int bytesReceived;

                while (true)
                {
                    bytesReceived = clientSocket.Receive(receiveData);
                    receivedString.Append(Encoding.Unicode.GetString(receiveData, 0, bytesReceived));

                    if (bytesReceived < receiveData.Length)
                    {
                        break;
                    }
                }

                return receivedString.ToString();
            }
            catch (Exception ex)
            {
                return "Ошибка отправки сообщения/получения ответа: " + ex.Message;
            }
        }

        private IPEndPoint SocketInit(string ipAddress, string port)
        {
            try
            {
                if (!int.TryParse(port, out int portNumber))
                {
                    throw new ArgumentException("Порт задан неверно (должен быть числом)");
                }

                return new IPEndPoint(IPAddress.Parse(ipAddress), portNumber);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка инициализации сокета: " + ex.Message);
            }
        }
    }
}
