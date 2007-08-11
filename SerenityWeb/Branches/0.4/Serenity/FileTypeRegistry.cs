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
		#region Methods - Public
		public static void Initialize()
		{
			/*
[c]
Compress = true
Description = "C Code File"
Icon = "page_white_c"
MimeType = "text/plain"

[cpp]
Compress = true
Description = "C++ Code File"
Icon = "page_white_cplusplus"
MimeType = "text/plain"

[cs]
Compress = true
Description = "CSharp Code File"
Icon = "page_white_csharp"
MimeType = "text/plain"

[css]
Compress = true
Description = "Cascading Style Sheet"
Icon = "page_white_text"
MimeType = "text/css"

[dll]
Compress = false
Description = "Application Extension"
Icon = "page_white_gear"
MimeType = "application/octet-stream"

[exe]
Compress = false
Description = "Application"
Icon = "page_white_gear"
MimeType = "application/octet-stream"

[html]
Compress = true
Description = "HTML Document"
MimeType = "text/html"

[pdf]
Compress = true
Description = "PDF Document"
Icon = "page_white_acrobat"
MimeType = "application/pdf"

[png]
Compress = false
Description = "PNG Image"
Icon = "page_white_picture"
Type = "image/png"

[rar]
Compress = false
Description = "RAR Archive"
Icon = "page_white_zip"
Type = "application/x-rar-compressed"

[sql]
Compress = true
Description = "SQL Script"
Icon = "page_white_database"
Type = "text/plain"

[txt]
Compress = true
Description = "Text Document"
icon = "page_white"
MimeType = "text/plain"

[zip]
Compress = false
Description = "ZIP Archive"
Icon = "page_white_zip"
Type = "application/zip"
			 */
			entries.Add("c", new FileTypeEntry("C Code File", MimeType.TextPlain, true));
			entries.Add("cpp", new FileTypeEntry("C++ Code File", MimeType.TextPlain, true));
			entries.Add("cs", new FileTypeEntry("C# Code File", MimeType.TextPlain, true));
			entries.Add("css", new FileTypeEntry("Cascading Style Sheet", MimeType.TextCss, true));
			entries.Add("dll", new FileTypeEntry("Application Extension", MimeType.ApplicationOctetStream, false));
			entries.Add("exe", new FileTypeEntry("Application", MimeType.ApplicationOctetStream, false));
			entries.Add("html", new FileTypeEntry("HTML Document", MimeType.TextHtml, true));
			entries.Add("pdf", new FileTypeEntry("PDF Document", MimeType.FromString("application/pdf"), true));
			entries.Add("png", new FileTypeEntry("PNG Image", MimeType.ImagePng, false));
			entries.Add("rar", new FileTypeEntry("RAR Archive", MimeType.FromString("application/x-rar-compressed"), false));
			entries.Add("sql", new FileTypeEntry("SQL Script", MimeType.TextPlain, true));
			entries.Add("txt", new FileTypeEntry("Text Document", MimeType.TextPlain, true));
			entries.Add("zip", new FileTypeEntry("ZIP Archive", MimeType.FromString("application/zip"), false));
			/*
			 * WS: Some unknown problem with LibINI, disabling this code block untill its fixed.
			
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
                string description, extension;
				MimeType mimeType;
                bool compress;

                extension = section.Name;
                if (string.IsNullOrEmpty(extension) == false)
                {
                    if (section.ContainsEntry("Description") == true)
                    {
                        description = section["Description"].Value.Trim('"');
                    }
                    else
                    {
                        description = extension + " file";
                    }

                    if (section.ContainsEntry("MimeType") == true)
                    {
						string mt = section["MimeType"].Value.Trim('"');
						mimeType = MimeType.FromString(mt);
                    }
                    else
                    {
						mimeType = MimeType.Default;
                    }
                    if (section.ContainsEntry("Compress") == true)
                    {
                        try
                        {
                            compress = bool.Parse(section["Compress"].Value.Trim('"'));
                        }
                        catch
                        {
                            compress = false;
                        }
                    }
                    else
                    {
                        compress = false;
                    }
                    FileTypeRegistry.entries.Add(extension, new FileTypeEntry(description, mimeType, compress));
                }
            }
			*/
		}

		private static Dictionary<string, FileTypeEntry> entries = new Dictionary<string, FileTypeEntry>();
		public static readonly FileTypeEntry DefaultEntry = new FileTypeEntry("File", MimeType.Default, false);

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
