﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Server server = new Server();
                server.Start();
                Console.ReadLine();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }
    }
}
