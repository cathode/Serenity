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
						mimeType = MimeType.FromString(section["MimeType"].Value.Trim('"'));
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
