using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    class Server
    {
        private Acceptor _gatewayAcceptor;
        private Acceptor _agentAcceptor;

        private uint _timerInterval;

        private uint _receiveBufferSize;

        private int _id;

        public const string GATEWAY_IP = "127.0.0.1";
        public const uint GATEWAY_PORT = 30000;

        public const string AGENT_IP = "127.0.0.1";
        public const uint AGENT_PORT = 30001;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public uint ReceiveBufferSize
        {
            get { return _receiveBufferSize; }
            set { _receiveBufferSize = value; }
        }

        public uint TimerInterval
        {
            get { return _timerInterval; }
            set { _timerInterval = value; }
        }

        public Server()
        {
            //_mutex = new Mutex();
            _timerInterval = 5000;
            _receiveBufferSize = 4096;
            _id = 0;
        }

        public void Start()
        {
            _gatewayAcceptor = new Acceptor(this);
            //TODO :
            _gatewayAcceptor.Listen(GATEWAY_IP, GATEWAY_PORT);
            _gatewayAcceptor.Accept();

            _agentAcceptor = new Acceptor(this);
            //TODO :
            _agentAcceptor.Listen(AGENT_IP, AGENT_PORT);
            _agentAcceptor.Accept();
        }

    }
}
