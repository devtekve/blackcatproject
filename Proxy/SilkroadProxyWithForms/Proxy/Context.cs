using System.Net.Sockets;
using SilkroadSecurityApi;
using System;

namespace Proxy
{
    class Context : IDisposable
    {
        private Socket _mySocket;
        private Security _mySecurity;
        private TransferBuffer _myTransferBuffer;
        private Security _myRelaySecurity;
        private bool disposed = false;

        public Context()
        {
            _mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _mySecurity = new Security();
            _myTransferBuffer = new TransferBuffer(8192, 0, 0);
        }

        ~Context()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposeManagedResources)
        {
            if (!this.disposed)
            {
                if (disposeManagedResources)
                {
                    if (_mySocket != null)
                    {
                        _mySocket.Dispose();
                        _mySocket = null;
                    }

                    if (_mySecurity != null)
                    {
                        _mySecurity.Dispose();
                        _mySecurity = null;
                    }
                }
                disposed = true;
            }
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
