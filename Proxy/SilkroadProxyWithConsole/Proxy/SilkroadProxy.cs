﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Proxy
{
    class SilkroadProxy
    {
        private Socket _gwLocalServer;
        private Socket _agLocalServer;

        public SilkroadProxy()
        {
            _gwLocalServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _agLocalServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        internal void StartProxy()
        {
            if (GatewayLocalListening())
            {
                Console.WriteLine("gateway local is listening");
                AcceptGatewayConnection();
            }

            if (AgentLocalListening())
            {
                Console.WriteLine("agent local is listening");
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
                Console.WriteLine(exception.ToString());
            }
        }

        private void AcceptGatewayCallback(IAsyncResult result)
        {
            try
            {
                SilkroadTunnel silkroadTunnel = new SilkroadTunnel(this);
                silkroadTunnel.LocalClient = _gwLocalServer.EndAccept(result);
                Console.WriteLine("Gateway Local connection has been made !");

                AcceptGatewayConnection();

                silkroadTunnel.Start();

                

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        #region listening
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
                Console.WriteLine(exception.ToString());
            }
            return retval;
        }
        #endregion

        private void AcceptAgentCallback(IAsyncResult result)
        {
            try
            {
                KeyValuePair<string, ushort> kvp = (KeyValuePair<string, ushort>)result.AsyncState;
                SilkroadTunnel silkroadTunnel = new SilkroadTunnel(this);
                silkroadTunnel.SetRemoteServerAddress(kvp.Key, kvp.Value);
                silkroadTunnel.LocalClient = _agLocalServer.EndAccept(result);
                Console.WriteLine("agent local connection has been made !");

                silkroadTunnel.Start();

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
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
                Console.WriteLine(exception.ToString());
            }
        }
    }
}