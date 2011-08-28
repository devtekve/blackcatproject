using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Server
{
    class Acceptor
    {
        private Server _server;
        private Socket _serverSocket;
        private string _ip;
        private uint _port;

        public Acceptor(Server server)
        {
            _server = server;
            //_serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Listen(string ip, uint port)
        {
            try
            {
                _ip = ip;
                _port = port;
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(_ip), (int)_port);
                _serverSocket = new Socket(ep.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _serverSocket.Bind(ep);
                _serverSocket.Listen((int)SocketOptionName.MaxConnections);
                Console.WriteLine("Started listening on ip : " + ip + " and port " + port);
            }
            catch (SocketException socketException)
            {
                Console.WriteLine(socketException.ToString());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        public void Accept()
        {
            try
            {
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
            catch (SocketException socketException)
            {
                Console.WriteLine(socketException.ToString());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }

        }

        private void AcceptCallback(IAsyncResult result)
        {
            try
            {
                int id;
                lock (this)
                {
                    id = ++_server.Id;
                }

                Console.WriteLine("Connection " + _server.Id + " has just connected on " + _ip + " - " + _port);
                Connection connection = new Connection(_server, id, _ip, _port);
                connection.ConnectionSocket = _serverSocket.EndAccept(result);
                StartTimer(connection);
                connection.Receive();
                Accept();
            }
            catch (SocketException socketException)
            {
                Console.WriteLine(socketException.ToString());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void StartTimer(Connection connection)
        {
            connection.OnTimer();
        }
    }
}
