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
using System.Text;
using Serenity.Web;

namespace Serenity
{
	public static class FileTypeRegistry
	{
		#region Constructors - Private
		static FileTypeRegistry()
		{
			FileTypeRegistry.DefaultEntry = new FileTypeEntry();
			FileTypeRegistry.DefaultEntry.Description = "File";
			FileTypeRegistry.DefaultEntry.Icon = "page_white";
			FileTypeRegistry.DefaultEntry.MimeType = MimeType.Default;
			FileTypeRegistry.DefaultEntry.UseCompression = false;
		}
		#endregion
		#region Methods - Public
		public static void Initialize()
		{
            /*
            FileTypeRegistry.entries = new Dictionary<string, FileTypeEntry>();
            IniReader reader = new IniReader(IniReaderSettings.Win32Style);

            IniFile file;
            bool result = reader.Read(File.OpenRead(Path.Combine(SerenityPath.ConfigurationDirectory, "FileTypeRegistry.ini")), out file);
            if (result)
            {
                file.IsCaseSensitive = false;

                foreach (IniSection section in file)
                {
                    string description, extension, icon;
                    MimeType mimeType;
                    bool useCompression;

                    extension = section.Name;
                    if (!string.IsNullOrEmpty(extension))
                    {
                        if (section.ContainsEntry("Description"))
                        {
                            description = (string)section["Description"].Value.Value;
                        }
                        else
                        {
                            description = extension + " file";
                        }

                        if (section.ContainsEntry("MimeType"))
                        {
                            mimeType = MimeType.FromString((string)section["MimeType"].Value.Value);
                        }
                        else
                        {
                            mimeType = MimeType.Default;
                        }
                        if (section.ContainsEntry("Compress"))
                        {
                            useCompression = (bool)section["Compress"].Value.Value;
                        }
                        else
                        {
                            useCompression = false;
                        }
                        if (section.ContainsEntry("Icon"))
                        {
                            icon = (string)section["Icon"].Value.Value;
                        }
                        else
                        {
                            icon = "page_white";
                        }
                        FileTypeEntry typeEntry = new FileTypeEntry();
                        typeEntry.Description = description;
                        typeEntry.Icon = icon;
                        typeEntry.MimeType = mimeType;
                        typeEntry.UseCompression = useCompression;
                        FileTypeRegistry.entries.Add(extension, typeEntry);
                    }
                }
            }*/
		}

		private static Dictionary<string, FileTypeEntry> entries = new Dictionary<string, FileTypeEntry>();
		public static readonly FileTypeEntry DefaultEntry;

		public static bool GetCompressionUsage(string extension)
		{
			extension = extension.ToLower().TrimStart('.');
			if (FileTypeRegistry.entries.ContainsKey(extension))
			{
				return FileTypeRegistry.entries[extension].UseCompression;
			}
			else
			{
				return FileTypeRegistry.DefaultEntry.UseCompression;
			}

		}
		public static string GetDescription(string extension)
		{
			extension = extension.ToLower().TrimStart('.');
			if (FileTypeRegistry.entries.ContainsKey(extension))
			{
				return FileTypeRegistry.entries[extension].Description;
			}
			else
			{
				return FileTypeRegistry.DefaultEntry.Description;
			}
		}
		
		public static FileTypeEntry GetEntry(string extension)
		{
			extension = extension.ToLower().TrimStart('.');
			if (FileTypeRegistry.entries.ContainsKey(extension))
			{
				return FileTypeRegistry.entries[extension];
			}
			else
			{
				return FileTypeRegistry.DefaultEntry;
			}
		}
		public static string GetIcon(string extension)
		{
			extension = extension.ToLower().TrimStart('.');
			if (FileTypeRegistry.entries.ContainsKey(extension))
			{
				return FileTypeRegistry.entries[extension].Icon;
			}
			else
			{
				return FileTypeRegistry.DefaultEntry.Icon;
			}
		}
		public static MimeType GetMimeType(string extension)
		{
			extension = extension.ToLower().TrimStart('.');
			if (FileTypeRegistry.entries.ContainsKey(extension))
			{
				return FileTypeRegistry.entries[extension].MimeType;
			}
			else
			{
				return FileTypeRegistry.DefaultEntry.MimeType;
			}
		}
		public static IEnumerable<string> GetRegisteredExtensions()
		{
			foreach (string extension in FileTypeRegistry.entries.Keys)
			{
				yield return extension;
			}
		}
		#endregion
	}
}
