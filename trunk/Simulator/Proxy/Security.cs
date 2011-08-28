using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proxy
{
    class Security
    {
        private List<Packet> _packetToSendList;
        private List<Packet> _packetToRecvList;
        private object _lockObject;

        public Security()
        {
            _packetToSendList = new List<Packet>();
            _packetToRecvList = new List<Packet>();
            _lockObject = new object();
        }

        public bool HasPacketToSend()
        {
            lock (_lockObject)
            {
                if (_packetToSendList.Count > 0)
                {
                    return true;
                }
                //Console.WriteLine("no more packet to send !");
                return false;
            }
        }

        public Packet GetPacketToSend()
        {
            lock (_lockObject)
            {
                Packet packet = _packetToSendList[0];
                _packetToSendList.RemoveAt(0);
                return packet;
            }
        }

        public bool HasPacketToRecv()
        {
            lock (_lockObject)
            {
                if (_packetToRecvList.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public Packet GetPacketToRecv()
        {
            lock (_lockObject)
            {
                Packet packet = _packetToRecvList[0];
                _packetToRecvList.RemoveAt(0);
                return packet;
            }
        }

        internal void Recv(Packet packet)
        {
            lock (_lockObject)
            {
                _packetToRecvList.Add(packet);
            }
        }

        internal void Send(Packet packet)
        {
            lock (_lockObject)
            {
                _packetToSendList.Add(packet);
            }
        }

        
    }
}
