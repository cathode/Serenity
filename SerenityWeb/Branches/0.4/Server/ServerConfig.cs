using LibINI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server
{
	internal class ServerConfig
	{
		internal bool Read(string path)
		{
			IniFile file = new IniFile(path);
			file.Read();


			if (file.ContainsSection("General"))
			{
				IniSection section = file["General"];
				if (section.ContainsEntry("LogToConsole"))
				{
					try
					{
						this.LogToConsole = bool.Parse(section["LogToConsole"].Value);
					}
					catch
					{
					}
				}
				if (section.ContainsEntry("LogToFile"))
				{
					try
					{
						this.LogToFile = bool.Parse(section["LogToFile"].Value);
					}
					catch
					{
					}
				}
			}
			if (file.ContainsSection("Network"))
			{
				IniSection section = file["Network"];

				if (section.ContainsEntry("BlockingIO"))
				{
					try
					{
						this.BlockingIO = bool.Parse(section["BlockingIO"].Value);
					}
					catch
					{
					}
				}
				if (section.ContainsEntry("Ports"))
				{
					
					try
					{
						string[] portValues = section["Ports"].Value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
						this.Ports = new ushort[portValues.Length];

						for (int i = 0; i < portValues.Length; i++)
						{
							this.Ports[i] = ushort.Parse(portValues[i]);
						}
					}
					catch
					{
					}
				}
			}
			return true;
		}
		internal ushort[] Ports = new ushort[] { 80, 8080 };
		internal bool BlockingIO = true;
		internal bool LogToConsole = true;
		internal bool LogToFile = true;
	}
}
