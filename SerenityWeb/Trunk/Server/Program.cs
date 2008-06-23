/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Serenity;
using Serenity.Logging;
using Serenity.Web.Drivers;
using Serenity.IO;
using NDesk.Options;
using Serenity.Data;

namespace Server
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            //Print out program name, version, copyright, and contact information.
            Console.WriteLine("{0}, v{1}\r\n{2} ({3})\r\n",
                SerenityInfo.Name, SerenityInfo.Version, SerenityInfo.Copyright, "http://serenityproject.net/");

            //Set up SerenityPath with correct values.
            
            

            var ops = new OptionSet()
            {
                { "a|appdata",
                    d => SerenityPath.WorkingDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "Serenity Project\\Serenity") }
            };
            ops.Parse(args);

            Database.Create(DataScope.Global);

            //Perform loading of the server configuration file.
            ServerConfig config = new ServerConfig();
            if (!config.Read(Path.Combine(Serenity.IO.SerenityPath.ConfigurationDirectory, "Serenity.ini")))
            {
                Console.WriteLine("Failure when reading server configuration.");
                return;
            }

            foreach (KeyValuePair<string, string> pair in config.Modules)
            {
                string name = pair.Key;
                string path = (pair.Value.StartsWith("@")) ? Path.GetFullPath("./Modules/" + pair.Value.TrimStart('@')) : pair.Value;
                SerenityServer.AddModule(Module.LoadModuleFile(name, path));
            }

            SerenityServer.OperationLog.Write(string.Format("Loaded: {0} domains, {1} modules, {2} themes.",
                SerenityServer.Domains.Count,
                SerenityServer.Modules.Count,
                0), LogMessageLevel.Info);
            WebDriverSettings driverSettings = new WebDriverSettings();
            driverSettings.Ports = config.Ports;

            WebDriver driver = new HttpDriver(driverSettings);

            SerenityServer.DriverPool.Add(driver);

            driver.Initialize();

            if (!driver.Start())
            {
                SerenityServer.ErrorLog.Write("Failed to start web driver", LogMessageLevel.Error);
            }
            else
            {
                //WS: Temporary loop to keep the main thread from exiting when in async mode.
                while (driver.Status == OperationStatus.Started)
                {
                    Thread.Sleep(1000);
                }
            }

            SerenityServer.OperationLog.Write("Server shutting down", LogMessageLevel.Info);
            Console.WriteLine("Press any key...");
            Console.Read();
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
