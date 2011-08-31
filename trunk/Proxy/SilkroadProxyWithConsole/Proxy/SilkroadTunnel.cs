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
        private Socket _localClient;
        private Security _localSecurity;
        private TransferBuffer _localRecvBuffer;
        private Socket _remoteClient;
        private Security _remoteSecurity;
        private TransferBuffer _remoteRecvBuffer;

        private string _remoteServerIP;
        private ushort _remoteServerPort;

        private SilkroadProxy _silkroadProxy;

        private Thread _remote;
        private Thread _local;

        public SilkroadTunnel(SilkroadProxy silkroadProxy)
        {
            _localRecvBuffer = new TransferBuffer(4096, 0, 0);

            _remoteClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _remoteRecvBuffer = new TransferBuffer(4096, 0, 0);

            _remoteServerIP = "123.30.200.6";
            _remoteServerPort = 15779;

            _localSecurity = new Security();
            _localSecurity.GenerateSecurity(true, true, true);

            _silkroadProxy = silkroadProxy;
        }

        public void SetRemoteServerAddress(string ip, ushort port)
        {
            _remoteServerIP = ip;
            _remoteServerPort = port;
        }

        public Socket LocalClient
        {
            get
            {
                return _localClient;
            }
            set
            {
                _localClient = value;
            }
        }

        internal void Start()
        {
            RecvLocalConnection();

            ConnectRemoteServer(_remoteServerIP, _remoteServerPort);
        }

        private void RecvLocalConnection()
        {
            try
            {
                _localClient.BeginReceive(_localRecvBuffer.Buffer, 0, _localRecvBuffer.Buffer.Length,
                                            SocketFlags.None, new AsyncCallback(RecvLocalConnectionCallback), _localRecvBuffer);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void ConnectRemoteServer(string ip, int port)
        {
            try
            {
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

                _remoteClient.BeginConnect(ipEndPoint, new AsyncCallback(ConnectRemoteServerCallback), _remoteClient);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void RecvLocalConnectionCallback(IAsyncResult result)
        {
            try
            {
                TransferBuffer localRecvBuffer = result.AsyncState as TransferBuffer;
                localRecvBuffer.Offset = 0;
                localRecvBuffer.Size = _localClient.EndReceive(result);
                _localSecurity.Recv(localRecvBuffer);

                RecvLocalConnection();

            }
            catch (SocketException socketExceptio)
            {
                Console.WriteLine(socketExceptio.ToString());
                Disconnect();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void Disconnect()
        {
            try
            {
                _localClient.BeginDisconnect(false, new AsyncCallback(LocalDisconnectCallback), null);
                _remoteClient.BeginDisconnect(false, new AsyncCallback(RemoteDisconnectCallback), null);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void LocalDisconnectCallback(IAsyncResult result)
        {
            try
            {
                _local.Abort();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void RemoteDisconnectCallback(IAsyncResult result)
        {
            try
            {
                _remote.Abort();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void ConnectRemoteServerCallback(IAsyncResult result)
        {
            try
            {
                Socket socket = result.AsyncState as Socket;
                socket.EndConnect(result);
                Console.WriteLine("Remote connection has been made ");

                _remoteSecurity = new Security();

                RecvRemoteConnection();

                _remote = new Thread(RemoteThread);
                _remote.Start();

                _local = new Thread(LocalThread);
                _local.Start();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void RecvRemoteConnection()
        {
            try
            {
                _remoteClient.BeginReceive(_remoteRecvBuffer.Buffer, 0, _remoteRecvBuffer.Buffer.Length,
                                            SocketFlags.None, new AsyncCallback(RecvRemoteConnectionCallback), _remoteRecvBuffer);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void RecvRemoteConnectionCallback(IAsyncResult result)
        {
            try
            {
                TransferBuffer gwRemoteRecvBuffer = result.AsyncState as TransferBuffer;
                gwRemoteRecvBuffer.Offset = 0;
                gwRemoteRecvBuffer.Size = _remoteClient.EndReceive(result);
                _remoteSecurity.Recv(gwRemoteRecvBuffer);

                RecvRemoteConnection();

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }


        }

        private void LocalThread()
        {
            try
            {
                while (true)
                {
                    List<Packet> _gwLocalRecvPackets = _localSecurity.TransferIncoming();
                    if (_gwLocalRecvPackets != null)
                    {
                        foreach (Packet packet in _gwLocalRecvPackets)
                        {
                            byte[] packet_bytes = packet.GetBytes();
                            Console.WriteLine("[C->P][{0:X4}][{1} bytes]{2}{3}{4}{5}{6}", packet.Opcode, packet_bytes.Length, packet.Encrypted ? "[Encrypted]" : "", packet.Massive ? "[Massive]" : "", Environment.NewLine, Utility.HexDump(packet_bytes), Environment.NewLine);

                            // Do not pass through these packets.
                            if (packet.Opcode == 0x5000 || packet.Opcode == 0x9000 || packet.Opcode == 0x2001)
                            {
                                continue;
                            }

                            _remoteSecurity.Send(packet);
                        }
                    }

                    List<KeyValuePair<TransferBuffer, Packet>> _gwLocalSendPackets = _localSecurity.TransferOutgoing();
                    if (_gwLocalSendPackets != null)
                    {
                        foreach (var kvp in _gwLocalSendPackets)
                        {
                            Packet packet = kvp.Value;
                            TransferBuffer buffer = kvp.Key;

                            byte[] packet_bytes = packet.GetBytes();
                            Console.WriteLine("[P->C][{0:X4}][{1} bytes]{2}{3}{4}{5}{6}", packet.Opcode, packet_bytes.Length, packet.Encrypted ? "[Encrypted]" : "", packet.Massive ? "[Massive]" : "", Environment.NewLine, Utility.HexDump(packet_bytes), Environment.NewLine);

                            _localClient.BeginSend(buffer.Buffer, 0, buffer.Size, SocketFlags.None, new AsyncCallback(SendLocalConnectionCallback), null);
                        }
                    }

                    Thread.Sleep(1);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void RemoteThread()
        {
            try
            {
                while (true)
                {
                    List<Packet> _gwRemoteRecvPackets = _remoteSecurity.TransferIncoming();
                    if (_gwRemoteRecvPackets != null)
                    {
                        foreach (Packet packet in _gwRemoteRecvPackets)
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

                                    _localSecurity.Send(new_packet);

                                    continue;
                                }
                            }

                            _localSecurity.Send(packet);
                        }
                    }

                    List<KeyValuePair<TransferBuffer, Packet>> _gwRemoteSendPackets = _remoteSecurity.TransferOutgoing();
                    if (_gwRemoteSendPackets != null)
                    {
                        foreach (var kvp in _gwRemoteSendPackets)
                        {
                            Packet packet = kvp.Value;
                            TransferBuffer buffer = kvp.Key;

                            byte[] packet_bytes = packet.GetBytes();
                            Console.WriteLine("[P->S][{0:X4}][{1} bytes]{2}{3}{4}{5}{6}", packet.Opcode, packet_bytes.Length, packet.Encrypted ? "[Encrypted]" : "", packet.Massive ? "[Massive]" : "", Environment.NewLine, Utility.HexDump(packet_bytes), Environment.NewLine);

                            _remoteClient.BeginSend(buffer.Buffer, 0, buffer.Size, SocketFlags.None, new AsyncCallback(SendRemoteConnectionCallback), null);
                        }
                    }

                    Thread.Sleep(1);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void SendRemoteConnectionCallback(IAsyncResult result)
        {
            try
            {
                int numSend = _remoteClient.EndSend(result);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void SendLocalConnectionCallback(IAsyncResult result)
        {
            try
            {
                int numSend = _localClient.EndSend(result);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }
    }
}
