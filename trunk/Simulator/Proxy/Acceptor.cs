using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Proxy
{
    class Acceptor
    {
        private Proxy _myProxy;
        private Socket _serverSocket;

        public Acceptor(Proxy proxy)
        {
            // TODO: Complete member initialization
            _myProxy = proxy;
        }

        public Proxy MyProxy
        {
            get
            {
                return _myProxy;
            }
            set
            {
            }
        }

        internal bool Listen(IPEndPoint gatewayProxyEndPoint)
        {
            bool retval = false;
            try
            {
                _serverSocket = new Socket(gatewayProxyEndPoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _serverSocket.Bind(gatewayProxyEndPoint);
                _serverSocket.Listen((int)SocketOptionName.MaxConnections);
                retval = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }

            return retval;
        }

        internal void StartAccept(Connection connection)
        {
            try
            {
                //Console.WriteLine("->->->StartAccept()");
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallbackHandle), connection);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void AcceptCallbackHandle(IAsyncResult result)
        {
            try
            {
                Connection connection = result.AsyncState as Connection;
                //Console.WriteLine("->->->->AcceptCallbackHandle()");
                connection.ConnectionSocket = _serverSocket.EndAccept(result);
                connection.StartTimer();
                lock (_myProxy)
                {
                    _myProxy.AcceptorOnAccept(this, connection);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        internal void Close()
        {
            try
            {
                _serverSocket.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }
    }
}
