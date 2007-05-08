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

namespace Serenity
{
    public class FileTypeRegistry
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
                string description, mimeType, extension;
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
                        mimeType = section["MimeType"].Value.Trim('"');
                    }
                    else
                    {
                        mimeType = "text/plain";
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

        private static Dictionary<string, FileTypeEntry> entries;
        public static readonly FileTypeEntry DefaultEntry = new FileTypeEntry("File", "text/plain", false);

        public static bool GetCompressionUsage(string extension)
        {
            if (FileTypeRegistry.entries.ContainsKey(extension) == true)
            {
                return FileTypeRegistry.entries[extension].Compress;
            }
            else
            {
                return FileTypeRegistry.DefaultEntry.Compress;
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
        public static string GetMimeType(string extension)
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
