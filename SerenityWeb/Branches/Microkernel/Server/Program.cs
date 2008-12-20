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
using Serenity;
using Serenity.Data;
using Serenity.Net;
using System.Collections.Generic;
using NDesk.Options;

namespace Server
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            //Print out program name, version, copyright, and contact information.
            Console.WriteLine("{0}, v{1}\r\n{2} ({3})\r\n",
                SerenityInfo.Name, SerenityInfo.Version, SerenityInfo.Copyright, "http://serenityproject.net/");

            string profilePath = "default.profile.xml";

            OptionSet ops = new OptionSet() { { "p|profile=", "Path to the XML profile definition", v => profilePath = v }, };
            ops.Parse(args);

            ServerProfile profile = new ServerProfile();
            if (profilePath != null)
            {
                Console.WriteLine("Loading server profile from {0}.", profilePath);
                profile.LoadXmlProfile(profilePath);
            }

            HttpServer server = new HttpServer()
            {
                Profile = profile
            };

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
    }
}
