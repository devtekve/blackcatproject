using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace Client
{
    class Client
    {
        private Socket _socket;
        private ClientForm _clientForm;
        private byte[] _buffer;
        private string _ip;
        private uint _port;

        public Client(ClientForm clientForm)
        {
            _clientForm = clientForm;
            _buffer = new byte[4096];
            _ip = String.Empty;
            _port = 0;
        }

        private void WriteLogs(string output)
        {
            if (_clientForm != null)
            {
                _clientForm.WriteLogs(output);
            }
        }

        public bool Connect(IPEndPoint ep)
        {
            bool retval = false;

            try
            {
                _socket = new Socket(ep.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _socket.Connect(ep);
                WriteLogs("A connection has made on port " + ep.Port);
                retval = true;
                Receive();
            }
            catch (SocketException socketException)
            {
                WriteLogs(socketException.ToString());
            }
            catch (Exception exception)
            {
                WriteLogs(exception.ToString());
            }

            return retval;
        }

        private void Receive()
        {
            try
            {
                _socket.BeginReceive(_buffer, 0, 4096, SocketFlags.None, new AsyncCallback(ReceiveCallback), _buffer);
            }
            catch (SocketException socketException)
            {
                WriteLogs(socketException.ToString());
            }
            catch (Exception exception)
            {
                WriteLogs(exception.ToString());
            }
        }

        private string Bytes2String(byte[] bytes, int size)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            return encoder.GetString(bytes, 0, size);
        }

        private byte[] String2Bytes(string input)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            return encoder.GetBytes(input);
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                byte[] buffer = result.AsyncState as byte[];
                if (_socket != null)
                {
                    int numRead = _socket.EndReceive(result);
                    string msg = Bytes2String(buffer, numRead);
                    WriteLogs(msg);
                    if (Parser(msg))
                    {
                        _socket.Send(buffer, 0, numRead, SocketFlags.None);
                    }
                    Receive();
                }
                
            }
            catch (SocketException)
            {
                Disconnect();
            }
            catch (Exception exception)
            {
                WriteLogs(exception.ToString());
            }
            
        }

        private bool Parser(string input)
        {
            bool retval = true;
            if (!"pingping".Equals(input))
            {
                if (input.Contains('.'))
                {
                    _ip = input;
                }
                else
                {
                    _port = Convert.ToUInt32(input);
                }

                retval = false;
            }

            if (!_ip.Equals(String.Empty) && _port != 0)
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(_ip), (int)_port);
                Connect(ep);
                _ip = String.Empty;
                _port = 0;
            }
            return retval;
        }

        private void Disconnect()
        {
            try
            {
                _socket.Close();
                _socket = null;
                _clientForm.ChangeVisibleButton();
                WriteLogs("Connection has just ended");
            }
            catch (SocketException socketException)
            {
                WriteLogs(socketException.ToString());
            }
            catch (Exception exception)
            {
                WriteLogs(exception.ToString());
            }
        }

        public void Send(string message)
        {
            try
            {
                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] buffer = encoder.GetBytes(message);
                if (_socket != null)
                {
                    _socket.Send(buffer);
                }
            }
            catch (Exception exception)
            {
                WriteLogs(exception.ToString());
            }
            
        }
    }
}
