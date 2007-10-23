/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using LibINI;
using LibINI.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Serenity;

namespace Server
{
	internal class ServerConfig
	{
		internal bool Read(string path)
		{
            IniReader reader = new IniReader(IniReaderSettings.Win32Style);
            bool result = false;
            IniFile file = reader.Read(File.OpenRead(path), out result);
            if (result)
            {
                file.IsCaseSensitive = false;

                if (file.ContainsSection("General"))
                {
                    IniSection section = file["General"];
                    if (section.ContainsEntry("LogToConsole"))
                    {
                        this.LogToConsole = (bool)section["LogToConsole"].Value.Value;
                    }
                    if (section.ContainsEntry("LogToFile"))
                    {
                        this.LogToFile = (bool)section["LogToFile"].Value.Value;
                    }
                }
                if (file.ContainsSection("Modules"))
                {
                    IniSection section = file["Modules"];

                    foreach (IniEntry entry in section)
                    {
                        this.Modules.Add(entry.Name, (string)entry.Value.Value);
                    }
                }
                if (file.ContainsSection("Network"))
                {
                    IniSection section = file["Network"];

                    if (section.ContainsEntry("BlockingIO"))
                    {
                        try
                        {
                            //this.BlockingIO = bool.Parse(section["BlockingIO"].Value);
                        }
                        catch
                        {
                        }
                    }
                    if (section.ContainsEntry("Ports"))
                    {

                        try
                        {
                            string[] portValues = ((string)section["Ports"].Value.Value).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
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
            else
            {
                return false;
            }
		}
		internal ushort[] Ports = new ushort[] { 80, 8080 };
		internal bool BlockingIO = true;
		internal bool LogToConsole = true;
		internal bool LogToFile = true;
		internal Dictionary<string, string> Modules = new Dictionary<string, string>();
		internal bool OnDemandLoading = true;
		
	}
}
