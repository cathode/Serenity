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
using System.Globalization;
using System.IO;
using System.Text;

namespace Serenity
{
    /// <summary>
    /// Represents one of the "special" folders that Serenity uses.
    /// </summary>
    public enum SpecialFolder
    {
        /// <summary>
        /// Represents the root folder (where all the other data is kept).
        /// </summary>
        /// <remarks>
        /// When scope resolution is global, this directory also contains the Serenity binaries.
        /// </remarks>
        Root,
        /// <summary>
        /// The configuration folder (where server-wide configuration settings are stored)
        /// </summary>
        Configuration,
        /// <summary>
        /// The folder where domain settings are stored.
        /// </summary>
        Domains,
        /// <summary>
        /// The folder where environment data is stored.
        /// </summary>
        Environments,
        /// <summary>
        /// The folder where module data is stored.
        /// </summary>
        Modules,
        /// <summary>
        /// The folder where theme data is stored.
        /// </summary>
        Themes,
        /// <summary>
        /// The folder where log files are stored.
        /// </summary>
        Logs,
    }
    public enum SpecialFile
    {
        Log,
        FileTypeRegistry,
    }
    public enum OverwriteMode
    {
        All,
        Newer,
        Older,
        None,
    }
    public static class SPath
    {
        #region Constructors - Static
        static SPath()
        {
            SPath.specialFolders = new Dictionary<SpecialFolder, string>();
            SPath.specialFiles = new Dictionary<SpecialFile, string>();

			string root = Path.GetFullPath("./");

            SPath.specialFolders[SpecialFolder.Configuration] = Path.GetFullPath(SPath.Combine(root, "Configuration"));
            SPath.specialFolders[SpecialFolder.Domains] = Path.GetFullPath(SPath.Combine(root, "Domains"));
            SPath.specialFolders[SpecialFolder.Environments] =  Path.GetFullPath(SPath.Combine(root, "Environments"));
            SPath.specialFolders[SpecialFolder.Logs] =  Path.GetFullPath(SPath.Combine(root, "Logs"));
            SPath.specialFolders[SpecialFolder.Modules] =  Path.GetFullPath(SPath.Combine(root, "Modules"));
            SPath.specialFolders[SpecialFolder.Root] = root;
            SPath.specialFolders[SpecialFolder.Themes] = Path.GetFullPath(SPath.Combine(root, "Themes"));

            SPath.specialFiles[SpecialFile.FileTypeRegistry] = SPath.Combine(SPath.specialFolders[SpecialFolder.Configuration], "FileTypeRegistry.ini");
            SPath.specialFiles[SpecialFile.Log] = SPath.Combine(SPath.specialFolders[SpecialFolder.Logs], "Serenity.log");
        }
        #endregion
        #region Fields - Private
        private static Dictionary<SpecialFolder, string> specialFolders;
        private static Dictionary<SpecialFolder, string> specialFoldersGlobal;
        private static Dictionary<SpecialFile, string> specialFiles;
        private static Dictionary<SpecialFile, string> specialFilesGlobal;
        #endregion
        #region Methods - Public
        public static string Combine(params string[] paths)
        {
            string result = "";
            foreach (string path in paths)
            {
                result = Path.Combine(result, path);
            }
            return result;
        }
        public static int CopyDirectory(string source, string destination)
        {
            return SPath.CopyDirectory(source, destination, OverwriteMode.None);
        }
        /// <summary>
        /// Recursively copies files and directories from one directory to another.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static int CopyDirectory(string source, string destination, OverwriteMode mode)
        {
            int copied = 0;
            source = Path.GetFullPath(source);
            destination = Path.GetFullPath(destination);

            if (Directory.Exists(destination) == false)
            {
                Directory.CreateDirectory(destination);
            }

            foreach (string path in Directory.GetFiles(source))
            {
                string temp = Path.Combine(destination, Path.GetFileName(path));
                if (File.Exists(temp) == false)
                {
                    File.Copy(path, temp);
                    copied++;
                }
            }
            foreach (string path in Directory.GetDirectories(source))
            {
                string temp = Path.Combine(destination, Path.GetFileName(path));
                copied += SPath.CopyDirectory(path, temp, mode);
            }

            return copied;
        }
        public static string ResolveSpecialPath(SpecialFolder specialFolder)
        {
			return SPath.specialFolders[specialFolder];
        }
        public static string ResolveSpecialPath(SpecialFile specialFile)
        {
			return SPath.specialFiles[specialFile];
        }
        public static string GetEnvironmentFile(string name)
        {
            return SPath.Combine(SPath.EnvironmentsFolder, name, "Environment");
        }
        public static string GetEnvironmentDirectory(string name)
        {
            return SPath.Combine(SPath.EnvironmentsFolder, name);
        }
        public static string GetModuleFile(string name)
        {
            return SPath.Combine(SPath.ModulesFolder, name, name + ".dll");
        }
        public static string GetModuleDirectory(string name)
        {
            return SPath.Combine(SPath.ModulesFolder, name);
        }
        public static string GetStaticFile(SerenityEnvironment environment, string name)
        {
            return SPath.Combine(SPath.EnvironmentsFolder, environment.Key, name);
        }
        public static string GetLocalizedStaticFile(SerenityEnvironment environment, string name, CultureInfo ci)
        {
            return SPath.Combine(SPath.EnvironmentsFolder, environment.Key, "static", ci.Name, name);
        }
        public static string GetThemeFile(string name)
        {
            return SPath.Combine(SPath.ThemesFolder, name, "Theme");
        }
        public static string GetThemeDirectory(string name)
        {
            return SPath.Combine(SPath.ThemesFolder, name);
        }
        public static string SanitizePath(string path)
        {
            return path.Replace("../", "").Replace('/', '\\');
        }
        #endregion
        #region Properties - Public        
        /// <summary>
        /// Gets an absolute path to the directory where domain settings are stored.
        /// </summary>
        public static string DomainsFolder
        {
            get
            {
                return SPath.ResolveSpecialPath(SpecialFolder.Domains);
            }
        }
        /// <summary>
        /// Gets an absolute path to the directory where environments are stored.
        /// </summary>
        public static string EnvironmentsFolder
        {
            get
            {
                return SPath.ResolveSpecialPath(SpecialFolder.Environments);
            }
        }
        /// <summary>
        /// Gets an absolute path to the primary log file.
        /// </summary>
        public static string LogFile
        {
            get
            {
                return SPath.ResolveSpecialPath(SpecialFile.Log);
            }
        }
        /// <summary>
        /// Gets an absolute path to the directory where log files are stored.
        /// </summary>
        public static string LogsFolder
        {
            get
            {
                return SPath.ResolveSpecialPath(SpecialFolder.Logs);
            }
        }
        
        /// <summary>
        /// Gets an absolute path to the directory where modules are stored.
        /// </summary>
        public static string ModulesFolder
        {
            get
            {
                return SPath.ResolveSpecialPath(SpecialFolder.Modules);
            }
        }
        /// <summary>
        /// Gets an absolute path to the root directory where data is stored.
        /// </summary>
        public static string RootFolder
        {
            get
            {
                return SPath.ResolveSpecialPath(SpecialFolder.Root);
            }
        }
        /// <summary>
        /// Gets an absolute path to the directory where themes are stored.
        /// </summary>
        public static string ThemesFolder
        {
            get
            {
                return SPath.ResolveSpecialPath(SpecialFolder.Themes);
            }
        }
        #endregion        
    }
}
