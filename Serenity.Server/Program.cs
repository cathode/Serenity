﻿/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Serenity.Web;
using System.Xml.Linq;

namespace Serenity.Server
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Serenity Console Mode, starting up...");
            WebServer server = new WebServer();
            server.LoadBuiltinApplications();
            //Program.DumpResourceGraph(server.Resources);
            server.Start();
            Console.WriteLine("Server shutting down. Press any key...");
            Console.ReadLine();
        }

        internal static void DumpResourceGraph(ResourceGraph graph)
        {
            var doc = new XDocument(
                new XElement("ResourceGraph",
                    new XElement("Resource")));

            doc.Save("ResourceGraph.xml");
        }
    }
}
