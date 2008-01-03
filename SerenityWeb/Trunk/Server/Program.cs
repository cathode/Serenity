/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

using Serenity;
using Serenity.Logging;
using Serenity.Themes;
using Serenity.Web;
using Serenity.Web.Drivers;

namespace Server
{
	internal class Program
	{
		internal static void Main(string[] args)
		{
			//Print out program name, version, copyright, and contact information.
			Console.WriteLine("{0}, v{1}\r\n{2} ({3})\r\n",
				SerenityInfo.Name, SerenityInfo.Version, SerenityInfo.Copyright, "http://serenityproject.net/");


            //Make sure the server has the correct folders to use,
            //if they don't exist we need to create them.
            foreach (SpecialFolder sf in Program.RecurseEnum<SpecialFolder>())
            {
                string dir = SPath.ResolveSpecialPath(sf);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                    SerenityServer.OperationLog.Write("Creating " + dir, LogMessageLevel.Info);
                }
            }

			//Perform loading of the server configuration file.
			ServerConfig config = new ServerConfig();
			config.Read("Configuration/Serenity.ini");

            

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
