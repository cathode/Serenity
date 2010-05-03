using System;
using System.Net;
using NDesk.Options;
using Serenity;
using Serenity.Net;

namespace Server
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            //Print out program name, version, copyright, and contact information.
            //Console.WriteLine("Serenity HTTP Server v{0}\r\n", System.Reflection.Assembly.GetAssembly(typeof(Module)).GetCustomAttributes(typeof(System.Reflection.AssemblyVersionAttribute), true)[0]);

            Serenity.Net.Server server = new Serenity.Net.Server();

            server.Log.EventRecorded += new EventHandler<EventRecordedEventArgs>(Log_EventRecorded);

            server.Initialize();
            server.Start();

            Console.WriteLine("Server running, press ESC to shut down.");

            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("Shutting down server now...");
                    server.Stop();
                    break;
                }
            }
        }

        static void Log_EventRecorded(object sender, EventRecordedEventArgs e)
        {
            Console.WriteLine(e.Details.ToString());
        }
    }
}
