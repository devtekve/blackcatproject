using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proxy;

namespace SilkroadProxyWithConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SilkroadProxy silkroadProxy = new SilkroadProxy();
                silkroadProxy.StartProxy();
                Console.ReadLine();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }
    }
}
