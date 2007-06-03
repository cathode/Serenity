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
using System.Threading;

using Serenity;
using Serenity.Web.Drivers;

using Server.OperatingModes;

namespace Server
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            Console.WriteLine("{0}, v{1}\r\n{2} ({3})\r\n",
                SerenityInfo.Name, SerenityInfo.Version, SerenityInfo.Copyright, "http://serenityproject.net/");

            Log.LogToConsole = true;
            Log.LogToFile = true;
            OverwriteMode mode = OverwriteMode.None;

            foreach (string arg in args)
            {
                switch (arg)
                {
                    case "-g":
                    case "--global":
                        SPath.DefaultScope = ResolutionScope.Global;
                        SPath.ForceDefaultScope = true;
                        Log.Write("Forced global scope mode engaged", LogMessageLevel.Info);
                        break;

                    case "-l":
                    case "--local":
                        SPath.DefaultScope = ResolutionScope.Local;
                        SPath.ForceDefaultScope = true;
                        Log.Write("Forced local scope mode engaged", LogMessageLevel.Info);
                        break;

                    case "-o":
                    case "--overwrite":
                        mode = OverwriteMode.Older;
                        Log.Write("Overwrite Older Files mode engaged", LogMessageLevel.Info);
                        break;

                    default:
                        break;
                }
            }

            Log.Write("Beginning File Verification", LogMessageLevel.Info);

            int copied = 0;
            foreach (SpecialFolder specialFolder in Program.RecurseEnum<SpecialFolder>())
            {
                if (specialFolder != SpecialFolder.Root)
                {
                    try
                    {
                        if (Directory.Exists(SPath.ResolveSpecialPath(specialFolder, ResolutionScope.Global)) == false)
                        {
                            Directory.CreateDirectory(SPath.ResolveSpecialPath(specialFolder, ResolutionScope.Global));
                        }
                    }
                    catch
                    {
                    }
                    try
                    {
                        if (Directory.Exists(SPath.ResolveSpecialPath(specialFolder, ResolutionScope.Local)) == false)
                        {
                            Directory.CreateDirectory(SPath.ResolveSpecialPath(specialFolder, ResolutionScope.Local));
                        }
                    }
                    catch
                    {
                    }
                    try
                    {
                        copied += SPath.CopyDirectory(SPath.ResolveSpecialPath(specialFolder, ResolutionScope.Global),
                            SPath.ResolveSpecialPath(specialFolder, ResolutionScope.Local), mode);
                    }
                    catch
                    {
                    }
                }
            }

            Log.Write("Completed File Verfication (" + copied.ToString() + " files)", LogMessageLevel.Info);

            Log.StartLogging();

            string operatingMode = "server";
            if (args.Length > 0)
            {
                operatingMode = args[0];
            }
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