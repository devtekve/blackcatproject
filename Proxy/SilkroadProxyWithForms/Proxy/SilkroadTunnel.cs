using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using SilkroadSecurityApi;
using System.Windows.Forms;

namespace Proxy
{
    class SilkroadTunnel : IDisposable
    {
        #region don't care
        private Context _localContext;
        private Context _remoteContext;

        private string _remoteIP;
        private ushort _remotePort;

        private bool disposed = false;

        private SilkroadProxy _silkroadProxy;

        private List<Context> _contexts;

        private List<SilkroadTunnel> _tunnels;

        private List<KeyValuePair<TransferBuffer, Packet>> _outgoingPackets = null;

        private List<Packet> _incomingPackets = null;

        public SilkroadTunnel(SilkroadProxy silkroadProxy, List<SilkroadTunnel> tunnels)
        {
            _remoteIP = "123.30.200.6";
            _remotePort = 15779;

            _silkroadProxy = silkroadProxy;

            _localContext = new Context();
            _localContext.MySecurity.GenerateSecurity(true, true, true);

            _remoteContext = new Context();

            _localContext.MyRelaySecurity = _remoteContext.MySecurity;
            _remoteContext.MyRelaySecurity = _localContext.MySecurity;

            _contexts = new List<Context>();
            _contexts.Add(_localContext);
            _contexts.Add(_remoteContext);

            _tunnels = tunnels;
        }

        ~SilkroadTunnel()
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
                    // dispose managed resources
                    if (_localContext != null)
                    {
                        _localContext.Dispose();
                        _localContext = null;
                    }

                    if (_remoteContext != null)
                    {
                        _remoteContext.Dispose();
                        _remoteContext = null;
                    }

                    if (_outgoingPackets != null)
                    {
                        foreach (var kvp in _outgoingPackets)
                        {
                            kvp.Value.Dispose();
                        }
                        _outgoingPackets = null;
                    }

                    if (_incomingPackets != null)
                    {
                        foreach (var packet in _incomingPackets)
                        {
                            packet.Dispose();
                        }
                        _incomingPackets = null;
                    }
                }
                // dispose unmanaged resources
                disposed = true;
            }
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
                _localContext.MySocket = value;
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
                _remoteContext.MySocket.Connect(ip, port);
                HandleNetworkDataStream();
            }
            catch (Exception)
            {
                

                _silkroadProxy.UpdateNotify("A connection has just left !");
            }
            finally
            {

                try
                {
                    lock (_tunnels)
                    {
                        _tunnels.Remove(this);
                        _silkroadProxy.UpdateLabelStartGameButton(_silkroadProxy.HasConnectedClient());
                        Dispose();
                    }
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
                    _outgoingPackets = context.MySecurity.TransferOutgoing();
                    if (_outgoingPackets != null)
                    {
                        foreach (KeyValuePair<TransferBuffer, Packet> kvp in _outgoingPackets)
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
                _incomingPackets = context.MySecurity.TransferIncoming();

                if (_incomingPackets != null)
                {
                    foreach (Packet packet in _incomingPackets)
                    {
                        if (context == _remoteContext)
                        {
                            //Print(packet, "S->P");

                            if (ServerPacketHandler(context, packet))
                            {
                                continue;
                            }

                            context.MyRelaySecurity.Send(packet);
                        }

                        if (context == _localContext)
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

                            //CA2000 don't care
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
