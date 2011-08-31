using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using SilkroadSecurityApi;

namespace Proxy
{
    class Context
    {
        private Socket _mySocket;
        private Security _mySecurity;
        private TransferBuffer _myTransferBuffer;
        private Security _myRelaySecurity;

        public Context()
        {
            _mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _mySecurity = new Security();
            _myTransferBuffer = new TransferBuffer(8192, 0, 0);
        }

        public Socket MySocket
        {
            get
            {
                return _mySocket;
            }
            set
            {
                _mySocket = value;
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
                _mySecurity = value;
            }
        }

        public TransferBuffer MyTransferBuffer
        {
            get
            {
                return _myTransferBuffer;
            }
            set
            {
                _myTransferBuffer = value;
            }
        }

        public Security MyRelaySecurity
        {
            get
            {
                return _myRelaySecurity;
            }
            set
            {
                _myRelaySecurity = value;
            }
        }
    }
}
