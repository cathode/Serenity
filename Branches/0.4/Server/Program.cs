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

			Log.StartLogging();

			foreach (SpecialFolder sf in Program.RecurseEnum<SpecialFolder>())
			{
				string dir = SPath.ResolveSpecialPath(sf);
				if (!Directory.Exists(dir))
				{
					Directory.CreateDirectory(dir);
				}
			}

			FileTypeRegistry.Initialize();

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