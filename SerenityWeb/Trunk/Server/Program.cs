/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
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

			//Perform loading of the server configuration file.
			ServerConfig config = new ServerConfig();
			config.Read("Configuration/Serenity.ini");

			Log.LogToConsole = config.LogToConsole;
			Log.LogToFile = config.LogToFile;

			if (Log.LogToConsole || Log.LogToFile)
			{
				//Only start logging if the log messages will be recorded somewhere.
				Log.StartLogging();
			}

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
			WebDriverSettings settings = new WebDriverSettings();
			settings.Block = config.BlockingIO;
			settings.ContextHandler = new ContextHandler();
			settings.Ports = config.Ports;

			WebDriver driver = new HttpDriver(settings);

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
