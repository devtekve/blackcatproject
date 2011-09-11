using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using SilkroadSecurityApi;
using System.Windows.Forms;
using Decapcha;
using SilkroadProxyWithForms;
using Proxy;
using System.IO;
using System.Text;

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

        private MainForm _mainForm;

        private Charracter _charracter;

        public SilkroadTunnel(SilkroadProxy silkroadProxy, List<SilkroadTunnel> tunnels, MainForm mainForm)
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

            _mainForm = mainForm;

            if (_mainForm != null)
            {
                _charracter = new Charracter(_mainForm);
            }
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
            catch (SocketException)
            {
                _silkroadProxy.UpdateNotify("A connection has just left !");
            }
            catch (Exception e)
            {
                _mainForm.UpdateLog(e.ToString());
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
                            if (packet.Opcode == 0x6103)
                            {
                                Print(packet, context == _localContext ? "P->C" : "P->S");
                            }
                            
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

        private void Print(Packet packet, string direct)
        {
            byte[] packet_bytes = packet.GetBytes();

            StringBuilder output = new StringBuilder();
            output.AppendFormat("[" + direct + "][{0:X4}][{1} bytes]{2}{3}{4}{5}{6}",
                packet.Opcode, packet_bytes.Length, packet.Encrypted ? "[Encrypted]" : "",
                packet.Massive ? "[Massive]" : "", Environment.NewLine,
                Utility.HexDump(packet_bytes), Environment.NewLine);

            _mainForm.UpdateLog(output.ToString());

        }

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

                            if (packet.Opcode == 0x9000 || packet.Opcode == 0x5000)
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

                                    //CA2000 don't care
                                    Packet new_packet = new Packet(0xA102, true);
                                    new_packet.WriteUInt8(result);
                                    new_packet.WriteUInt32(id);
                                    new_packet.WriteAscii("127.0.0.1");
                                    new_packet.WriteUInt16(15779);

                                    context.MyRelaySecurity.Send(new_packet);
                                    continue;
                                }
                            }

                            if (packet.Opcode == 0x2322)
                            {
                                CapchaKiller killer = new CapchaKiller(packet.GetBytes());
                                string result = killer.GetStringCapcha();

                                Packet new_packet = new Packet(0x6323, false);
                                new_packet.WriteAscii(result, 20127);
                                context.MySecurity.Send(new_packet);
                                continue;
                            }

                            if (ServerPacketHandler(context, packet))
                            {
                                continue;
                            }

                            context.MyRelaySecurity.Send(packet);
                        }

                        if (context == _localContext)
                        {
                            if (packet.Opcode == 0x9000 || packet.Opcode == 0x5000 || packet.Opcode == 0x2001)
                            {
                                continue;
                            }

                            if (ClientPacketHandler(context, packet))
                            {
                                continue;
                            }

                            context.MyRelaySecurity.Send(packet);
                        }

                        GC.Collect();
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
                case 0x303D:
                    {
                        packet.SeekRead(24, SeekOrigin.Begin);
                        uint maxHP = packet.ReadUInt32();
                        uint maxMP = packet.ReadUInt32();
                        _charracter.MaxHP = maxHP;
                        _charracter.MaxMP = maxMP;
                        if (_charracter.CurrentHP > 0)
                        {
                            _mainForm.UpdateProgressBarValue(_charracter.MaxHP, _charracter.MaxHP, _charracter.MaxMP, _charracter.MaxMP);
                        }

                    }
                    break;
                case 0x3020:
                    {
                        _charracter.Id = packet.ReadUInt32();
                        Packet packet3013 = _charracter.Opcode3013;
                        string id = BitConverter.ToString(BitConverter.GetBytes(_charracter.Id)).Replace("-", "");
                        string tempPacket = BitConverter.ToString(packet3013.GetBytes()).Replace("-", "");
                        int index = tempPacket.IndexOf(id, 0);

                        if (index > 0)
                        {
                            float X = 0, Y = 0;
                            byte XX = 0, YY = 0;
                            index = index / 2 + 4;
                            packet3013.SeekRead(index, SeekOrigin.Begin);
                            ushort zonetype = packet3013.ReadUInt16();
                            XX = BitConverter.GetBytes(zonetype)[0];
                            YY = BitConverter.GetBytes(zonetype)[1];
                            if (zonetype >= 32769 && zonetype <= 32775)
                            {
                                X = packet3013.ReadSingle();
                                packet3013.SeekRead(index + 14, SeekOrigin.Begin);
                                Y = packet3013.ReadSingle();
                            }
                            else
                            {
                                X = packet3013.ReadSingle();
                                packet3013.SeekRead(index + 10, SeekOrigin.Begin);
                                Y = packet3013.ReadSingle();
                            }
                            _charracter.XCoord = (int)Math.Floor((XX - 135) * 192 + X / 10);
                            _charracter.YCoord = (int)Math.Floor((YY - 92) * 192 + Y / 10);
                            _mainForm.UpdateCoord("X : " + _charracter.XCoord + "   " + "Y : " + _charracter.YCoord);
                        }

                        packet3013.Dispose();
                        _charracter.Opcode3013 = null;
                    }
                    break;
                case 0x3057:
                    {
                        uint tempID = packet.ReadUInt32();
                        packet.SeekRead(6, SeekOrigin.Begin);

                        if (tempID == _charracter.Id)
                        {
                            byte check = packet.ReadUInt8();
                            if (check == 1)
                            {
                                _charracter.CurrentHP = packet.ReadUInt32();
                            }
                            else if (check == 2)
                            {
                                _charracter.CurrentMP = packet.ReadUInt32();
                            }
                        }
                        _mainForm.UpdateProgressBarValue(_charracter.MaxHP, _charracter.CurrentHP, _charracter.MaxMP, _charracter.CurrentMP);
                    }
                    break;
                case 0x3013:
                    {
                        //Print(packet, "S->P");
                        _charracter.Opcode3013 = packet;
                        packet.SeekRead(42, SeekOrigin.Begin);
                        _charracter.CurrentHP = packet.ReadUInt32();
                        _charracter.CurrentMP = packet.ReadUInt32();
                        //_mainForm.UpdateProgressBarValue(_charracter.MaxHP, _charracter.CurrentHP, _charracter.MaxMP, _charracter.CurrentMP);
                    }
                    break;
                case 0xB021:
                    {
                        byte XX = 0, YY = 0;
                        ushort X = 0, Y = 0;
                        uint uintObjectID = packet.ReadUInt32();
                        byte byteDes = packet.ReadUInt8();
                        XX = packet.ReadUInt8();
                        YY = packet.ReadUInt8();
                        packet.SeekRead(5, SeekOrigin.Begin);
                        ushort zonetype = packet.ReadUInt16();
                        packet.SeekRead(7, SeekOrigin.Begin);
                        if (byteDes != 0)
                        {
                            if (uintObjectID == _charracter.Id)
                            {
                                if (zonetype >= 32769 && zonetype <= 32775)
                                {
                                    X = (ushort)(packet.ReadUInt16() - packet.ReadUInt16());
                                    packet.SeekRead(15, SeekOrigin.Begin);
                                    Y = (ushort)(packet.ReadUInt16() - packet.ReadUInt16());
                                }
                                else
                                {
                                    X = packet.ReadUInt16();
                                    packet.ReadUInt16();
                                    Y = packet.ReadUInt16();
                                }
                                _charracter.XCoord = (int)Math.Floor((XX - 135) * 192 + X / 10.0);
                                _charracter.YCoord = (int)Math.Floor((YY - 92) * 192 + Y / 10.0);

                                _mainForm.UpdateCoord("X : " + _charracter.XCoord + "   " + "Y : " + _charracter.YCoord);
                            }
                        }
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
                case 0x7001:
                    {
                        string charname = packet.ReadAscii();
                        _mainForm.UpdateCharName(charname);
                        _charracter.CharName = charname;
                    }
                    break;
                default:
                    break;
            }
            return retval;
        }

    }
}
