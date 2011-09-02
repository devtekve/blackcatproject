using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using SilkroadSecurityApi;
using System.Windows.Forms;

namespace Proxy
{
    class SilkroadTunnel
    {
        #region don't care
        private Context _local;
        private Context _remote;

        private string _remoteIP;
        private ushort _remotePort;

        private SilkroadProxy _silkroadProxy;

        private List<Context> _contexts;

        private List<SilkroadTunnel> _tunnels;

        public SilkroadTunnel(SilkroadProxy silkroadProxy, List<SilkroadTunnel> tunnels)
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

            _tunnels = tunnels;
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
            catch (Exception)
            {
                lock (_tunnels)
                {
                    _tunnels.Remove(this);
                    _silkroadProxy.UpdateLabelStartGameButton(_silkroadProxy.HasConnectedClient());
                }
                
                _silkroadProxy.UpdateNotify("A connection has just left !");
            }
            finally
            {

                try
                {
                    foreach (Context context in _contexts)
                    {
                        context.MySocket.Close();
                        context.MySocket = null;
                        context.MyRelaySecurity = null;
                        context.MySecurity = null;
                        context.MyTransferBuffer = null;
                    }
                    _contexts.Clear();
                    _contexts = null;
                    _local = null;
                    _remote = null;
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
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
                            //Print(packet, context == _local ? "P->C" : "P->S");
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

        //private void Print(Packet packet, string direct)
        //{
        //    byte[] packet_bytes = packet.GetBytes();
        //    Console.WriteLine("[" + direct + "][{0:X4}][{1} bytes]{2}{3}{4}{5}{6}",
        //        packet.Opcode, packet_bytes.Length, packet.Encrypted ? "[Encrypted]" : "",
        //        packet.Massive ? "[Massive]" : "", Environment.NewLine,
        //        Utility.HexDump(packet_bytes), Environment.NewLine);
        //}

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
                            //Print(packet, "S->P");

                            if (ServerPacketHandler(context, packet))
                            {
                                continue;
                            }

                            context.MyRelaySecurity.Send(packet);
                        }

                        if (context == _local)
                        {
                            //Print(packet, "C->P");

                            if (ClientPacketHandler(context, packet))
                            {
                                continue;
                            }

                            context.MyRelaySecurity.Send(packet);
                        }
                    }
                }
            }
        }

        #endregion

        private bool ServerPacketHandler(Context context, Packet packet)
        {
            bool retval = false;
            switch (packet.Opcode)
            {
                case 0x5000:
                case 0x9000:
                    {
                        retval = true;
                    }
                    break;
                case 0xA102:
                    #region handle opcode A102
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
                            retval = true;
                        }

                    }
                    #endregion
                    break;
                case 0x6100:
                    {
                        //Print(packet, "S->P");
                    }
                    break;
                default:
                    break;
            }
            return retval;
        }

        private bool ClientPacketHandler(Context context, Packet packet)
        {
            bool retval = false;
            switch (packet.Opcode)
            {
                case 0x2001:
                case 0x5000:
                case 0x9000:
                    {
                        retval = true;
                    }
                    break;
                case 0x6100:
                    {
                        //Print(packet, "C->P");
                    }
                    break;
                case 0x6103:
                    {
                        //Print(packet, "C->P");
                    }
                    break;
                default:
                    break;
            }
            return retval;
        }

    }
}
