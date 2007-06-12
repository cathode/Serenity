/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using LibINI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using Serenity.Themes;
using Serenity.Xml;

namespace Serenity
{
    /// <summary>
    /// Represents an operating environment for a Serenity Module.
    /// </summary>
    public sealed class SerenityEnvironment : Multiton<string, SerenityEnvironment>
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the SerenityEnvironment class.
        /// </summary>
        public SerenityEnvironment(string name)
            : base(name.ToLower())
        {
            if (Directory.Exists(SPath.GetEnvironmentDirectory(this.Key)) == false)
            {
                Directory.CreateDirectory(SPath.GetEnvironmentDirectory(this.Key));
            }
        }
        #endregion
        #region Fields - Private;
        private Module defaultModule;
        private Theme theme;
        private string defaultResourceClass;
        private string defaultResourceName;
        #endregion
        #region Methods - Public
        public static string[] GetEnvironmentList()
        {
            string[] paths = Directory.GetDirectories(SPath.EnvironmentsFolder);

            for (int i = 0; i < paths.Length; i++)
            {
                paths[i] = Path.GetFileName(paths[i]);
            }

            return paths;
        }
        public static void LoadAllEnvironments()
        {
            string[] environments = SerenityEnvironment.GetEnvironmentList();
            foreach (string env in environments)
            {
                SerenityEnvironment.LoadEnvironment(env);
            }
        }
        public static SerenityEnvironment LoadEnvironment(string name)
        {
            SerenityEnvironment environment = SerenityEnvironment.GetInstance(name);
            if (environment == null)
            {
                string path = SPath.GetEnvironmentFile(name);
                if (File.Exists(path) == true)
                {
                    IniFile settings = new IniFile(path);
                    settings.Read();

                    IniSection envSection = settings["Environment"];

                    environment = new SerenityEnvironment(name);
                    environment.defaultModule = Module.GetModule(envSection["DefaultModule"].Value);
                    environment.defaultResourceClass = envSection["DefaultResourceClass"].Value;
                    environment.defaultResourceName = envSection["DefaultResourceName"].Value;
                    environment.theme = Theme.LoadTheme(envSection["Theme"].Value);

                    return environment;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public static TryResult<SerenityEnvironment> TryLoadEnvironment(string name)
        {
            try
            {
                SerenityEnvironment environment = SerenityEnvironment.LoadEnvironment(name);
                if (environment != null)
                {
                    return TryResult<SerenityEnvironment>.SuccessResult(environment);
                }
                else
                {
                    return TryResult<SerenityEnvironment>.FailResult(new Exception("Unspecified Error"));
                }
            }
            catch (Exception e)
            {
                return TryResult<SerenityEnvironment>.FailResult(e);
            }
        }
        public static void Save(SerenityEnvironment environment)
        {
            IniFile settings = new IniFile(SPath.GetEnvironmentFile(environment.Key));
            IniSection envSection = settings.CreateSection("Environment");
            envSection.CreateEntry("DefaultModule", (environment.defaultModule == null) ? "" : environment.defaultModule.Name);
            envSection.CreateEntry("DefaultResourceClass", environment.defaultResourceClass);
            envSection.CreateEntry("DefaultResourceName", environment.defaultResourceName);
            envSection.CreateEntry("Theme", (environment.theme == null) ? "" : environment.theme.Key);

            settings.Write();
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets or sets the default module for the current SerenityEnvironment.
        /// </summary>
        public Module DefaultModule
        {
            get
            {
                return this.defaultModule;
            }
            set
            {
                this.defaultModule = value;
            }
        }
        public string StaticFilesDirectory
        {
            get
            {
                return SPath.Combine(SPath.GetEnvironmentDirectory(this.Key), "static");
            }
        }
        public Theme Theme
        {
            get
            {
                if (this.theme == null)
                {
                    this.theme = Theme.DefaultInstance;
                }
                return this.theme;
            }
            set
            {
                this.theme = value;
            }
        }
        public string DefaultResourceClass
        {
            get
            {
                return this.defaultResourceClass;
            }
            set
            {
                this.defaultResourceClass = value;
            }
        }
        public string DefaultResourceName
        {
            get
            {
                return this.defaultResourceName;
            }
            set
            {
                this.defaultResourceName = value;
            }
        }
        #endregion
    }
    public sealed class FileTree
    {
        public FileTree.FileItem GetFile(string name)
        {
            return default(FileItem);
        }
        public static FileTree GenerateTree(SerenityEnvironment environment)
        {
            return null;
        }
        public struct FileItem
        {
            #region Constructors - Internal
            internal FileItem(string path)
            {
                if (File.Exists(path) == true)
                {
                    this.info = new FileInfo(path);
                    this.Name = info.Name;
                    this.Size = this.info.Length;
                    this.LastModified = this.info.LastWriteTimeUtc;
                }
                else
                {
                    throw new FileNotFoundException("File Not Found", path);
                }
            }
            #endregion
            #region Fields - Private
            private readonly FileInfo info;
            #endregion
            #region Fields - Public
            public readonly string Name;
            public readonly DateTime LastModified;
            public readonly long Size;
            //public readonly string MimeType;
            //public readonly string TypeDescription;
            #endregion
            #region Properties - Public
            public FileInfo Info
            {
                get
                {
                    return this.info;
                }
            }
            #endregion
        }
        public struct DirectoryItem
        {

        }
    }
}