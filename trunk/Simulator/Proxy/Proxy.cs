using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;

namespace Proxy
{
    class Proxy
    {
        private Acceptor _gatewayAcceptor;
        private Acceptor _agentAcceptor;
        private IPEndPoint _gatewayServerEndPoint;
        private int _connectionCount;
        private int _timeInterval;
        private object _lockObject;
        private IPEndPoint _agentProxyEndPoint;

        public Proxy()
        {
            _lockObject = new object();
        }

        public int TimeInterval
        {
            get
            {
                return _timeInterval;
            }
            set
            {
                _timeInterval = value;
            }
        }
    
        internal void Setup(IPEndPoint gatewayProxyEndPoint, IPEndPoint agentProxyEndPoint, IPEndPoint gatewayServerEndPoint)
        {
            _gatewayServerEndPoint = gatewayServerEndPoint;
            _agentProxyEndPoint = agentProxyEndPoint;

            _gatewayAcceptor = new Acceptor(this);
            if (_gatewayAcceptor.Listen(gatewayProxyEndPoint))
            {
                Console.WriteLine("Gateway Acceptor is listening !");
            }
            //Console.WriteLine("Setup()");
            AcceptGatewayConnection();

            _agentAcceptor = new Acceptor(this);
            if (_agentAcceptor.Listen(agentProxyEndPoint))
            {
                Console.WriteLine("Agent Acceptor is listening !");
            }
        }

        private void AcceptGatewayConnection()
        {
            //Console.WriteLine("->AcceptGatewayConnection()");
            AcceptRemoteConnection(_gatewayAcceptor, _gatewayServerEndPoint);
        }

        private void AcceptRemoteConnection(Acceptor acceptor, IPEndPoint endPoint)
        {
            int id = Interlocked.Increment(ref _connectionCount);

            Connection connection = new Connection(acceptor.MyProxy, id, endPoint, ConnectionTypes.ServerType);
            acceptor.StartAccept(connection);

        }

        internal void AcceptorOnAccept(Acceptor acceptor, Connection connection)
        {
            if (acceptor.Equals(_gatewayAcceptor))
            {
                AcceptGatewayConnection();
                ConnectGatewayConnection(connection);
            }
            else if (acceptor.Equals(_agentAcceptor))
            {
                ConnectAgentConnection(connection);
            }
        }

        private void ConnectAgentConnection(Connection connection)
        {
            Connection newConnection = new Connection(_agentAcceptor.MyProxy, connection.ID, connection.RemoteEndPoint, ConnectionTypes.ClientType);
            ConnectRemoteConnection(connection, newConnection);
        }

        private void ConnectGatewayConnection(Connection connection)
        {
            Connection newConnection = new Connection(_gatewayAcceptor.MyProxy, connection.ID, connection.RemoteEndPoint, ConnectionTypes.ClientType);
            newConnection.SetAgentProxyEndPoint(_agentProxyEndPoint);
            ConnectRemoteConnection(connection, newConnection);
        }

        private void ConnectRemoteConnection(Connection connection, Connection newConnection)
        {
            connection.RelayConnection = newConnection;
            newConnection.RelayConnection = connection;

            newConnection.StartConnect();
        }   

        internal void AcceptAgentConnection(IPEndPoint ep)
        {
            AcceptRemoteConnection(_agentAcceptor, ep);
        }

        internal void Exit()
        {
            _gatewayAcceptor.Close();
            _agentAcceptor.Close();
        }
    }
}
