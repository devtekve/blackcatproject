using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Proxy
{
    class Connection
    {
        private Proxy _myProxy;
        private int _id;
        private IPEndPoint _remoteEndPoint;
        private ConnectionTypes _connectionType;
        private Socket _connectionSocket;
        private Timer _myTimer;
        private object _lockObject;
        private Security _mySecurity;
        private byte[] _buffer;
        private string _ip;
        private int _port;
        private Connection _relayConnection;
        private IPEndPoint _agentProxyEndPoint;

        public Connection(Proxy proxy, int id, IPEndPoint remoteEndPoint, ConnectionTypes connectionType)
        {
            // TODO: Complete member initialization
            _myProxy = proxy;
            _id = id;
            _remoteEndPoint = remoteEndPoint;
            _connectionType = connectionType;
            _mySecurity = new Security();
            _lockObject = new object();
            _buffer = new byte[4096];
            _ip = String.Empty;
            _port = 0;
        }

        public IPEndPoint RemoteEndPoint
        {
            get
            {
                return _remoteEndPoint;
            }
            set
            {

            } 
        }

        public Socket ConnectionSocket
        {
            get
            {
                return _connectionSocket;
            }
            set
            {
                _connectionSocket = value;
            }
        }

        public Proxy MyProxy
        {
            get
            {
                return _myProxy;
            }
            set
            {
                _myProxy = value;
            }
        }

        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
            }
        }

        public Connection RelayConnection
        {
            get
            {
                return _relayConnection;
            }
            set
            {
                _relayConnection = value;
            }
        }

        public Security MySecurity
        {
            get
            {
                return _mySecurity;
            }
            set
            {
            }
        }

        public ConnectionTypes ConnectionType
        {
            get
            {
                return _connectionType;
            }
            set
            {
            }
        }

        internal void StartTimer()
        {
            try
            {
                _myTimer = new Timer(new TimerCallback(TimerCallbackHandle), this, 0, _myProxy.TimeInterval);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void TimerCallbackHandle(object state)
        {
            try
            {
                Connection connection = state as Connection;
                connection.CheckForSend();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        internal void CheckForSend()
        {
            try
            {
                lock (_lockObject)
                {
                    while (_mySecurity.HasPacketToSend())
                    {
                        StartSend(_mySecurity.GetPacketToSend());
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void StartSend(Packet packet)
        {
            try
            {
                ASCIIEncoding encoder = new ASCIIEncoding();
                string msg = encoder.GetString(packet.Buffer, 0, packet.Size);
                _connectionSocket.BeginSend(packet.Buffer, 0, packet.Size, SocketFlags.None, new AsyncCallback(SendCallbackHandle), this);
            }
            catch (SocketException)
            {
                Disconnect();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void SendCallbackHandle(IAsyncResult result)
        {
            try
            {
                Connection connection = result.AsyncState as Connection;
                int numSend = _connectionSocket.EndSend(result);

                connection.CheckForSend();
            }
            catch (SocketException)
            {
                Disconnect();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }    
        }

        internal void StartConnect()
        {
            try
            {
                _connectionSocket = new Socket(_remoteEndPoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _connectionSocket.BeginConnect(_remoteEndPoint, new AsyncCallback(ConnectCallbackHandle), this);
                StartTimer();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void ConnectCallbackHandle(IAsyncResult result)
        {
            try
            {
                Connection connection = result.AsyncState as Connection;
                Connection relayConnection = connection.RelayConnection;
                if (relayConnection != null)
                {
                    lock (relayConnection)
                    {
                        connection.StartRecv();
                        connection.CheckForSend();

                        relayConnection.StartRecv();
                        relayConnection.CheckForSend();
                    }
                }
                else
                {
                    connection.Disconnect();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        internal void Disconnect()
        {
            try
            {
                if (_myTimer != null && _connectionSocket != null)
                {
                    _myTimer.Dispose();
                    _myTimer = null;
                    _connectionSocket.Close();
                    _connectionSocket = null;
                    _relayConnection.Disconnect();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        internal void StartRecv()
        {
            try
            {
                _connectionSocket.BeginReceive(_buffer, 0, 4096, SocketFlags.None, new AsyncCallback(RecvCallbackHandle), _buffer);
            }
            catch (SocketException)
            {
                Disconnect();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void RecvCallbackHandle(IAsyncResult result)
        {
            try
            {
                byte[] buffer = result.AsyncState as byte[];
                int numRecv = _connectionSocket.EndReceive(result);
                List<Packet> incomingList = new List<Packet>();

                lock (_lockObject)
                {
                    Packet packet = new Packet(buffer, numRecv);
                    _mySecurity.Recv(packet);
                    while (_mySecurity.HasPacketToRecv())
                    {
                        incomingList.Insert(0, _mySecurity.GetPacketToRecv());
                    }
                }

                foreach (Packet packet in incomingList)
                {
                    OnPacket(packet);
                }

                StartRecv();
            }
            catch (SocketException)
            {
                Disconnect();
            }
            catch (NullReferenceException)
            {

            }
            catch (ObjectDisposedException)
            {

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void OnPacket(Packet packet)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            if (_connectionType.Equals(ConnectionTypes.ServerType))
            {
                string msg = encoder.GetString(packet.Buffer, 0, packet.Size);
                if ("pingping".Equals(msg))
                {
                    packet.Buffer = encoder.GetBytes(msg + msg);
                    packet.Size = packet.Buffer.Length;
                }
            }
            else if (_connectionType.Equals(ConnectionTypes.ClientType))
            {
                
                string msg = encoder.GetString(packet.Buffer, 0, packet.Size);
                if (!"ping".Equals(msg))
                {
                    if (msg.Contains("."))
                    {
                        _ip = msg;
                        packet.Buffer = encoder.GetBytes("127.0.0.1");
                        packet.Size = packet.Buffer.Length;
                    }
                    else
                    {
                        _port = Convert.ToInt32(msg);
                        packet.Buffer = encoder.GetBytes(_agentProxyEndPoint.Port.ToString());
                        packet.Size = packet.Buffer.Length;
                    }

                    if (!_ip.Equals(String.Empty) && _port != 0)
                    {
                        IPEndPoint ep = new IPEndPoint(IPAddress.Parse(_ip), _port);
                        lock (_myProxy)
                        {
                            _myProxy.AcceptAgentConnection(ep);
                        }
                        _ip = String.Empty;
                        _port = 0;
                    }
                }
                else
                {
                    packet.Buffer = encoder.GetBytes(msg + msg);
                    packet.Size = packet.Buffer.Length;
                }
                
            }

            if (_relayConnection != null)
            {
                lock (_relayConnection)
                {
                    _relayConnection.SendPacket(packet);
                }
            }
            else
            {
                Disconnect();
            }
            
        }

        internal void SendPacket(Packet packet)
        {
            lock (_lockObject)
            {
                _mySecurity.Send(packet);
            }
        }

        internal void SetAgentProxyEndPoint(IPEndPoint agentProxyEndPoint)
        {
            _agentProxyEndPoint = agentProxyEndPoint;
        }
    }
}
