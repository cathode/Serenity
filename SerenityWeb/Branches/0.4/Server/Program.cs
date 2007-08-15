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
using Serenity.Web.Drivers;

using Server.OperatingModes;

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


			//(Temporary) Runs the server.
			ServerMode.Run();
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
