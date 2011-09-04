using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using SilkroadProxyWithForms;
using System.Windows.Forms;
using System.Threading;

namespace Proxy
{
    class SilkroadProxy:IDisposable
    {
        private Socket _gwLocalServer;
        private Socket _agLocalServer;
        private MainForm _mainForm;
        private List<SilkroadTunnel> _gatewayTunnels;
        private List<SilkroadTunnel> _agentTunnels;

        private bool disposed = false;

        public SilkroadProxy(MainForm mainForm)
        {
            _gwLocalServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _agLocalServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _mainForm = mainForm;
            _gatewayTunnels = new List<SilkroadTunnel>();
            _agentTunnels = new List<SilkroadTunnel>();
        }

        // the destructor
		~SilkroadProxy()
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
					if (_gwLocalServer != null)
					{
                        _gwLocalServer.Close();
						_gwLocalServer=null;
					}

                    if (_agLocalServer != null)
                    {
                        _agLocalServer.Close();
                        _agLocalServer = null;
                    }

                    if (_gatewayTunnels != null)
                    {
                        for (int i = 0; i < _gatewayTunnels.Count; i++ )
                        {
                            _gatewayTunnels[i].Dispose();
                            _gatewayTunnels[i] = null;
                        }
                        _gatewayTunnels = null;
                    }

                    if (_agentTunnels != null)
                    {
                        for (int i = 0; i < _agentTunnels.Count; i++)
                        {
                            _agentTunnels[i].Dispose();
                            _agentTunnels[i] = null;
                        }
                        _agentTunnels = null;
                    }
				}
				disposed=true;
			}
		}

        #region GUI stuff
        private void UpdateStatusGateway(string msg)
        {
            _mainForm.UpdateStatusGateway(msg);
        }

        private void UpdateStatusAgent(string msg)
        {
            _mainForm.UpdateStatusAgent(msg);
        }

        public void UpdateNotify(string msg)
        {
            _mainForm.UpdateNotify(msg);
        }

        public bool HasConnectedClient()
        {
            return _agentTunnels.Count != 0 || _gatewayTunnels.Count != 0;
        }

        public void UpdateLabelStartGameButton(bool flag)
        {
            _mainForm.UpdateLabelStartGameButton(flag);
        }

        #endregion

        #region handle proxy methods
        internal void StartProxy()
        {
            if (GatewayLocalListening())
            {
                UpdateStatusGateway("Listening !");
                AcceptGatewayConnection();
            }

            if (AgentLocalListening())
            {
                UpdateStatusAgent("Listening !");
            }
        }

        private void AcceptGatewayConnection()
        {
            try
            {
                _gwLocalServer.BeginAccept(new AsyncCallback(AcceptGatewayCallback), null);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void AcceptGatewayCallback(IAsyncResult result)
        {
            SilkroadTunnel silkroadTunnel = null;
            try
            {
                //CA2000 don't care
                silkroadTunnel = new SilkroadTunnel(this, _gatewayTunnels);
                silkroadTunnel.LocalClient = _gwLocalServer.EndAccept(result);
                UpdateNotify("Gateway Local connection has been made !");

                lock (_gatewayTunnels)
                {
                    _gatewayTunnels.Add(silkroadTunnel);
                    UpdateLabelStartGameButton(HasConnectedClient());
                }
                
                AcceptGatewayConnection();

                silkroadTunnel.Start();
            }
            catch (Exception exception)
            {
                if (silkroadTunnel != null)
                {
                    silkroadTunnel.Dispose();
                }
                MessageBox.Show(exception.ToString());
            }
        }


        private bool GatewayLocalListening()
        {
            return Listening(_gwLocalServer, "127.0.0.1", 15778);
        }

        private bool AgentLocalListening()
        {
            return Listening(_agLocalServer, "127.0.0.1", 15779);
        }

        private bool Listening(Socket serverSocket, string ip, int port)
        {
            bool retval = false;
            try
            {
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                serverSocket.Bind(ipEndPoint);
                serverSocket.Listen((int)SocketOptionName.MaxConnections);
                retval = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
            return retval;
        }

        private void AcceptAgentCallback(IAsyncResult result)
        {
            SilkroadTunnel silkroadTunnel = null;
            try
            {
                KeyValuePair<string, ushort> kvp = (KeyValuePair<string, ushort>)result.AsyncState;
                silkroadTunnel = new SilkroadTunnel(this, _agentTunnels);
                silkroadTunnel.SetRemoteServerAddress(kvp.Key, kvp.Value);
                silkroadTunnel.LocalClient = _agLocalServer.EndAccept(result);
                UpdateNotify("Agent local connection has been made !");
                lock (_agentTunnels)
                {
                    _agentTunnels.Add(silkroadTunnel);
                    UpdateLabelStartGameButton(HasConnectedClient());
                }
                
                silkroadTunnel.Start();

            }
            catch (Exception exception)
            {
                if (silkroadTunnel != null)
                {
                    silkroadTunnel.Dispose();
                }
                MessageBox.Show(exception.ToString());
            }
        }

        internal void AcceptAgentConnection(string ip, ushort port)
        {
            try
            {
                KeyValuePair<string, ushort> kvp = new KeyValuePair<string, ushort>(ip, port);
                _agLocalServer.BeginAccept(new AsyncCallback(AcceptAgentCallback), kvp);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }
        #endregion
    }
}
