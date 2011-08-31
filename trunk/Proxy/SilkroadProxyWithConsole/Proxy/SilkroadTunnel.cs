using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilkroadSecurityApi;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Proxy
{
    class SilkroadTunnel
    {
        private Context _local;
        private Context _remote;

        private string _remoteIP;
        private ushort _remotePort;

        private SilkroadProxy _silkroadProxy;

        private List<Context> _contexts;

        public SilkroadTunnel(SilkroadProxy silkroadProxy)
        {
            _remoteIP = "123.30.200.6";
            _remotePort = 15779;

            _silkroadProxy = silkroadProxy;

            _local = new Context();
            _local.MySecurity.GenerateSecurity(true, true, true);

            _remote = new Context();

            _local.MyRelaySecurity = _remote.MySecurity;
            _remote.MyRelaySecurity = _local.MySecurity;

            _contexts = new List<Context>();
            _contexts.Add(_local);
            _contexts.Add(_remote);
        }

        public void SetRemoteServerAddress(string ip, ushort port)
        {
            _remoteIP = ip;
            _remotePort = port;
        }

        public Socket LocalClient
        {
            set
            {
                _local.MySocket = value;
            }
        }

        internal void Start()
        {
            ConnectRemoteServer(_remoteIP, _remotePort);
        }

        private void ConnectRemoteServer(string ip, int port)
        {
            try
            {
                _remote.MySocket.Connect(ip, port);
                HandleNetworkDataStream();
            }
            catch (Exception exception)
            {
                Disconnect();
                Console.WriteLine(exception.ToString());
            }
        }

        private void Disconnect()
        {
            try
            {
                foreach (Context context in _contexts)
                {
                    context.MySocket.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void HandleNetworkDataStream()
        {
            while (true)
            {
                RecvDataStreamNetwork();

                HandleTransferIncoming();

                HandleTransferOutComing();

                Thread.Sleep(1); // Cycle complete, prevent 100% CPU usage
            }
        }

        private void HandleTransferOutComing()
        {
            foreach (Context context in _contexts) // Network output event processing
            {
                if (context.MySocket.Poll(0, SelectMode.SelectWrite))
                {
                    List<KeyValuePair<TransferBuffer, Packet>> buffers = context.MySecurity.TransferOutgoing();
                    if (buffers != null)
                    {
                        foreach (KeyValuePair<TransferBuffer, Packet> kvp in buffers)
                        {
                            TransferBuffer buffer = kvp.Key;
                            Packet packet = kvp.Value;

                            byte[] packet_bytes = packet.GetBytes();
                            Console.WriteLine("[{0}][{1:X4}][{2} bytes]{3}{4}{6}{5}{6}", context == _local ? "P->C" : "P->S", packet.Opcode, packet_bytes.Length, packet.Encrypted ? "[Encrypted]" : "", packet.Massive ? "[Massive]" : "", Utility.HexDump(packet_bytes), Environment.NewLine);

                            while (true)
                            {
                                int count = context.MySocket.Send(buffer.Buffer, buffer.Offset, buffer.Size, SocketFlags.None);
                                buffer.Offset += count;
                                if (buffer.Offset == buffer.Size)
                                {
                                    break;
                                }
                                Thread.Sleep(1);
                            }
                        }
                    }
                }
            }
        }

        private void HandleTransferIncoming()
        {
            foreach (Context context in _contexts) // Logic event processing
            {
                List<Packet> packets = context.MySecurity.TransferIncoming();

                if (packets != null)
                {
                    foreach (Packet packet in packets)
                    {
                        if (context == _remote)
                        {
                            byte[] packet_bytes = packet.GetBytes();
                            Console.WriteLine("[S->P][{0:X4}][{1} bytes]{2}{3}{4}{5}{6}", packet.Opcode, packet_bytes.Length, packet.Encrypted ? "[Encrypted]" : "", packet.Massive ? "[Massive]" : "", Environment.NewLine, Utility.HexDump(packet_bytes), Environment.NewLine);

                            // Do not pass through these packets.
                            if (packet.Opcode == 0x5000 || packet.Opcode == 0x9000)
                            {
                                continue;
                            }

                            if (packet.Opcode == 0xA102)
                            {
                                byte result = packet.ReadUInt8();
                                if (result == 1)
                                {
                                    uint id = packet.ReadUInt32();
                                    string ip = packet.ReadAscii();
                                    ushort port = packet.ReadUInt16();

                                    lock (_silkroadProxy)
                                    {
                                        _silkroadProxy.AcceptAgentConnection(ip, port);
                                    }

                                    Packet new_packet = new Packet(0xA102, true);
                                    new_packet.WriteUInt8(result);
                                    new_packet.WriteUInt32(id);
                                    new_packet.WriteAscii("127.0.0.1");
                                    new_packet.WriteUInt16(15779);

                                    context.MyRelaySecurity.Send(new_packet);

                                    continue;
                                }
                            }

                            context.MyRelaySecurity.Send(packet);
                        }

                        if (context == _local)
                        {
                            byte[] packet_bytes = packet.GetBytes();
                            Console.WriteLine("[C->P][{0:X4}][{1} bytes]{2}{3}{4}{5}{6}", packet.Opcode, packet_bytes.Length, packet.Encrypted ? "[Encrypted]" : "", packet.Massive ? "[Massive]" : "", Environment.NewLine, Utility.HexDump(packet_bytes), Environment.NewLine);

                            // Do not pass through these packets.
                            if (packet.Opcode == 0x5000 || packet.Opcode == 0x9000 || packet.Opcode == 0x2001)
                            {
                                continue;
                            }

                            context.MyRelaySecurity.Send(packet);
                        }
                    }
                }
            }
        }


        private void RecvDataStreamNetwork()
        {
            foreach (Context context in _contexts) // Network input event processing
            {
                if (context.MySocket.Poll(0, SelectMode.SelectRead))
                {
                    int count = context.MySocket.Receive(context.MyTransferBuffer.Buffer);
                    if (count == 0)
                    {
                        throw new Exception("The remote connection has been lost.");
                    }

                    context.MyTransferBuffer.Offset = 0;
                    context.MyTransferBuffer.Size = count;
                    context.MySecurity.Recv(context.MyTransferBuffer);
                }
            }
        }
    }
}
