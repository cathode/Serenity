/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
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
            Console.WriteLine("{0}, v{1}\r\n{2} ({3})\r\n",
                SerenityInfo.Name, SerenityInfo.Version, SerenityInfo.Copyright, "http://serenityproject.net/");

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
