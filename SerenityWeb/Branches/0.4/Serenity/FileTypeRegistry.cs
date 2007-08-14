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
using System.Text;

using LibINI;

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
			
            FileTypeRegistry.entries = new Dictionary<string, FileTypeEntry>();
            IniFile file = new IniFile(SPath.ResolveSpecialPath(SpecialFile.FileTypeRegistry));
            try
            {
                file.Read();
            }
            catch (IniOperationException opEx)
            {
                Console.WriteLine(opEx.ToString());
            }

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
                        description = section["Description"].Value.Trim('"');
                    }
                    else
                    {
                        description = extension + " file";
                    }

                    if (section.ContainsEntry("MimeType"))
                    {
						string mt = section["MimeType"].Value.Trim('"');
						mimeType = MimeType.FromString(mt);
                    }
                    else
                    {
						mimeType = MimeType.Default;
                    }
                    if (section.ContainsEntry("Compress"))
                    {
                        try
                        {
                            useCompression = bool.Parse(section["Compress"].Value.Trim('"'));
                        }
                        catch
                        {
                            useCompression = false;
                        }
                    }
                    else
                    {
                        useCompression = false;
                    }
					if (section.ContainsEntry("Icon"))
					{
						icon = section["Icon"].Value.Trim('"');
					}
					else
					{
						icon = "page_white";
					}
					FileTypeEntry entry = new FileTypeEntry();
					entry.Description = description;
					entry.Icon = icon;
					entry.MimeType = mimeType;
					entry.UseCompression = useCompression;
                    FileTypeRegistry.entries.Add(extension, entry);
                }
            }
		}

		private static Dictionary<string, FileTypeEntry> entries = new Dictionary<string, FileTypeEntry>();
		public static readonly FileTypeEntry DefaultEntry;

		public static bool GetCompressionUsage(string extension)
		{
			if (FileTypeRegistry.entries.ContainsKey(extension) == true)
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
			if (FileTypeRegistry.entries.ContainsKey(extension) == true)
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
			if (FileTypeRegistry.entries.ContainsKey(extension) == true)
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
			if (FileTypeRegistry.entries.ContainsKey(extension) == true)
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
