/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Serenity;
using Serenity.Web.Drivers;

using Server.OperatingModes;

namespace Server
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            //WS: For all the MechWarrior 3 fans out there :D
            Console.WriteLine("Reactor...online.");
            Console.WriteLine("Sensors...online.");
            Console.WriteLine("Weapons...online.");
            Console.WriteLine("All systems nominal.\r\n");
            Log.LogToConsole = true;
            Log.LogToFile = true;

            Log.Write("Beginning File Verification", LogMessageLevel.Info);

            int copied = 0;
            foreach (SpecialFolder specialFolder in Program.RecurseEnum<SpecialFolder>())
            {
                try
                {
                    if (specialFolder != SpecialFolder.Root)
                    {
                        if (Directory.Exists(SPath.ResolveSpecialPath(specialFolder, ResolutionScope.Global)) == false)
                        {
                            Directory.CreateDirectory(SPath.ResolveSpecialPath(specialFolder, ResolutionScope.Global));
                        }
                        if (Directory.Exists(SPath.ResolveSpecialPath(specialFolder, ResolutionScope.Local)) == false)
                        {
                            Directory.CreateDirectory(SPath.ResolveSpecialPath(specialFolder, ResolutionScope.Local));
                        }
                        copied += SPath.CopyDirectory(SPath.ResolveSpecialPath(specialFolder, ResolutionScope.Global),
                            SPath.ResolveSpecialPath(specialFolder, ResolutionScope.Local));
                    }
                }
                catch
                {

                }
            }

            Log.Write("Completed File Verfication (" + copied.ToString() + " files)", LogMessageLevel.Info);

            string operatingMode = "server";
            if (args.Length > 0)
            {
                operatingMode = args[0];
            }
            Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
            switch (operatingMode)
            {
                default:
                case "server":
                    OperatingModes.ServerMode.Run();
                    break;

                case "debug":
                    OperatingModes.DebugMode.Run();
                    break;
                case "service":
                case "serverui":
                case "serviceui":
                    Console.WriteLine("Not implemented yet! Press any key to continue...");
                    Console.ReadLine();
                    break;
            }
        }

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Log.Write("Stopping server", LogMessageLevel.Info);
            WebManager.StopAll();
            Log.Write("Server stopped", LogMessageLevel.Info);
        }
        private static IEnumerable<T> RecurseEnum<T>()
        {
            Array values = Enum.GetValues(typeof(T));
            foreach (T item in values)
            {
                yield return item;
            }
        }
    }
}