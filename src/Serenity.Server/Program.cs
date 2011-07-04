using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Server
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Serenity Console Mode, starting up...");
            WebServer server = new WebServer();
            server.LoadBuiltinApplications();
            server.Start();
            Console.WriteLine("Server shutting down. Press any key...");
            Console.ReadLine();
        }
    }
}
