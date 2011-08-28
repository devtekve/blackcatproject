using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class Connection
    {
        private Server _server;
        private int _id;
        private Socket _connectionSocket;
        private byte[] _buffer;
        private Timer _timer;
        private DateTime _lastTime;
        private string _ip;
        private uint _port;

        public DateTime LastTime
        {
            get { return _lastTime; }
            set { _lastTime = value; }
        }

        public Socket ConnectionSocket
        {
            get { return _connectionSocket; }
            set { _connectionSocket = value; }
        }

        public byte[] Buffer
        {
            get { return _buffer; }
            set { _buffer = value; }
        }

        public Connection(Server server, int id, string ip, uint port)
        {
            _server = server;
            _id = id;
            _ip = ip;
            _port = port;
        }

        public void Receive()
        {
            try
            {
                _buffer = new byte[_server.ReceiveBufferSize];
                _connectionSocket.BeginReceive(_buffer, 0, (int)_server.ReceiveBufferSize, SocketFlags.None,
                                              new AsyncCallback(RecieveCallback), _buffer);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
            
        }

        private string Bytes2String(byte[] bytes, int size)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            return encoder.GetString(bytes, 0, size);
        }

        private void RecieveCallback(IAsyncResult result)
        {
            try
            {
                byte[] buffer = result.AsyncState as byte[];
                int numRead = _connectionSocket.EndReceive(result);
                if (numRead > 0)
                {
                    string msg = Bytes2String(buffer, numRead);
                    Console.WriteLine("Connection " + _id + " : " + msg);
                    Receive();
                }
                else
                {
                    Disconnect();
                }
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

        private void Disconnect()
        {
            lock (this)
            {
                _server.Id--;
            }

            try
            {
                _connectionSocket.Close();
                _connectionSocket = null;
                Console.WriteLine("Connection " + _id + " has just left");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        public void OnTimer()
        {
            try
            {
                TimerCallback timerCallback = new TimerCallback(PingClient);
                _lastTime = DateTime.Now;
                _timer = new Timer(timerCallback, this, 1000, _server.TimerInterval);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private byte[] String2Bytes(string input)
        {
            byte[] buffer;
            ASCIIEncoding encoder = new ASCIIEncoding();
            buffer = encoder.GetBytes(input);
            return buffer;
        }

        private void PingClient(object state)
        {
            try
            {
                Connection connection = state as Connection;
                Socket socket = connection.ConnectionSocket;
                if (connection.ConnectionSocket != null)
                {
                    long dueTime = DateTime.Now.Ticks - connection._lastTime.Ticks;
                    //Console.WriteLine("dueTime = " + dueTime);
                    byte[] buffer;
                    if (dueTime > 11e7 && dueTime < 15e7 && _port != Server.AGENT_PORT)
                    {
                        //string agentAddressString = "<ip>"+ Server.AGENT_IP + "</ip><port>" + 
                        //                            Server.AGENT_PORT.ToString() + "</port>";
                        buffer = String2Bytes(Server.AGENT_IP);
                        socket.Send(buffer);
                        buffer = String2Bytes(Server.AGENT_PORT.ToString());
                    }
                    else
                    {
                        buffer = String2Bytes("ping");
                        //socket.Send(buffer);
                    }
                    socket.Send(buffer);
                    
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }
    }
}
