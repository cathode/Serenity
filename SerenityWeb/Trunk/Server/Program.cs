﻿/******************************************************************************
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
                }
            }

			//Perform loading of the server configuration file.
			ServerConfig config = new ServerConfig();
			config.Read("Configuration/Serenity.ini");

            SerenityServerSettings settings = new SerenityServerSettings();
            settings.LogToConsole = true;

            SerenityServer server = new SerenityServer();
            server.Configure(settings);

			

			//This loads the file type registry information.
			FileTypeRegistry.Initialize();
			DomainSettings.LoadAll();

			//(Temporary) Runs the server.
			foreach (KeyValuePair<string, string> pair in config.Modules)
			{
				string name = pair.Key;
				string path = (pair.Value.StartsWith("@")) ? Path.GetFullPath("./Modules/" + pair.Value.TrimStart('@')) : pair.Value;
				Module.AddModule(Module.LoadModuleFile(name, path));
			}

			Log.Write(string.Format("Loaded: {0} domains, {1} modules, {2} themes.",
				DomainSettings.Count,
				Module.LoadedCount,
				0), LogMessageLevel.Info);
			WebDriverSettings driverSettings = new WebDriverSettings();
            driverSettings.ContextHandler = new ContextHandler();
            driverSettings.Ports = config.Ports;

            WebDriver driver = new HttpDriver(driverSettings);

			driver.Initialize();

			if (!driver.Start())
			{
				Log.Write("Failed to start web driver", LogMessageLevel.Warning);
			}
			else
			{
				//WS: Temporary loop to keep the main thread from exiting when in async mode.
				while (driver.Status == WebDriverStatus.Started)
				{
					Thread.Sleep(1000);
				}
			}

			Log.Write("Server shutting down", LogMessageLevel.Info);
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
