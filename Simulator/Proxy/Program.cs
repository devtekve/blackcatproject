using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Proxy
{
    class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint gatewayProxyEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 20000);
            IPEndPoint agentProxyEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 20001);
            IPEndPoint gatewayServerEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 30000);

            Proxy proxy = new Proxy();
            proxy.TimeInterval = 50;
            proxy.Setup(gatewayProxyEndPoint, agentProxyEndPoint, gatewayServerEndPoint);
            Console.ReadLine();
            proxy.Exit();
        }
    }
}
