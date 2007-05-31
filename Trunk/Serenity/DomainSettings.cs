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
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using Serenity.Properties;

namespace Serenity
{
    /// <summary>
    /// Represents a collection of settings that are specific to a domain name.
    /// </summary>
    [Serializable]
    public sealed class DomainSettings
    {
        #region Constructors - Private
        static DomainSettings()
        {
            DomainSettings.settings = new Dictionary<string, DomainSettings>();

            DomainSettings.root = new DomainSettings("");

            DomainSettings.root.DefaultEnvironment.Value = "system";
            DomainSettings.root.DefaultModule.Value = "system";
            DomainSettings.root.DefaultTheme.Value = "system";
            DomainSettings.root.DefaultResourceClass.Value = "static";
            DomainSettings.root.DefaultResourceName.Value = "default.html";
            DomainSettings.root.CompressionThreshhold.Value = 4096;

            DomainSettings.settings.Add("", root);
        }
        #endregion
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the DomainSettings class.
        /// </summary>
        /// <param name="name"></param>
        public DomainSettings(string name)
        {
            this.name = name;

            DomainSettings parent = DomainSettings.GetParent(name);

            if (parent == null)
            {
                this.hasParent = false;
                this.parent = null;
                this.ActiveEnvironments = new DomainSettingValue<string[]>();
                this.ActiveModules = new DomainSettingValue<string[]>();
                this.ActiveThemes = new DomainSettingValue<string[]>();
                this.DefaultEnvironment = new DomainSettingValue<string>();
                this.DefaultModule = new DomainSettingValue<string>();
                this.DefaultResourceClass = new DomainSettingValue<string>();
                this.DefaultResourceName = new DomainSettingValue<string>();
                this.DefaultTheme = new DomainSettingValue<string>();
                this.OmitEnvironment = new DomainSettingValue<bool>();
                this.OmitResourceClass = new DomainSettingValue<bool>();
                this.CompressionThreshhold = new DomainSettingValue<int>();
            }
            else
            {
                this.parent = parent;
                this.ActiveEnvironments = new DomainSettingValue<string[]>(this.parent.ActiveEnvironments);
                this.ActiveModules = new DomainSettingValue<string[]>(this.parent.ActiveModules);
                this.ActiveThemes = new DomainSettingValue<string[]>(this.parent.ActiveThemes);
                this.DefaultEnvironment = new DomainSettingValue<string>(this.parent.DefaultEnvironment);
                this.DefaultModule = new DomainSettingValue<string>(this.parent.DefaultModule);
                this.DefaultResourceClass = new DomainSettingValue<string>(this.parent.DefaultResourceClass);
                this.DefaultResourceName = new DomainSettingValue<string>(this.parent.DefaultResourceName);
                this.DefaultTheme = new DomainSettingValue<string>(this.parent.DefaultTheme);
                this.OmitEnvironment = new DomainSettingValue<bool>(this.parent.OmitEnvironment);
                this.OmitResourceClass = new DomainSettingValue<bool>(this.parent.OmitResourceClass);
                this.CompressionThreshhold = new DomainSettingValue<int>(this.parent.CompressionThreshhold);
            }
        }
        #endregion
        #region Fields - Private
        [NonSerialized]
        [ThreadStatic]
        private static DomainSettings current;
        [NonSerialized]
        private bool hasParent;
        private readonly string name;
        [NonSerialized]
        private readonly DomainSettings parent;
        [NonSerialized]
        private static readonly DomainSettings root;
        [NonSerialized]
        private static readonly Dictionary<string, DomainSettings> settings;
        #endregion
        #region Fields - Public
        public readonly DomainSettingValue<string[]> ActiveEnvironments;
        public readonly DomainSettingValue<string[]> ActiveModules;
        public readonly DomainSettingValue<string[]> ActiveThemes;
        public readonly DomainSettingValue<string> DefaultEnvironment;
        public readonly DomainSettingValue<string> DefaultModule;
        public readonly DomainSettingValue<string> DefaultResourceClass;
        public readonly DomainSettingValue<string> DefaultResourceName;
        public readonly DomainSettingValue<string> DefaultTheme;
        public readonly DomainSettingValue<string[]> InactiveEnvironments;
        public readonly DomainSettingValue<string[]> InactiveModules;
        public readonly DomainSettingValue<string[]> InactiveThemes;
        public readonly DomainSettingValue<bool> OmitEnvironment;
        public readonly DomainSettingValue<bool> OmitResourceClass;
        public readonly DomainSettingValue<int> CompressionThreshhold;
        #endregion
        #region Methods - Public
        /// <summary>
        /// Gets the domain settings object which best matches the supplied hostUrl.
        /// </summary>
        /// <param name="hostUrl"></param>
        /// <returns></returns>
        public static DomainSettings GetBestMatch(Uri hostUrl)
        {
            if (hostUrl.HostNameType == UriHostNameType.Dns)
            {
                string[] names = hostUrl.Host.Split('.');
                Array.Reverse(names);
                return DomainSettings.GetBestMatch(string.Join(".", names), true);
            }
            else
            {
                return DomainSettings.GetBestMatch(hostUrl.Host, false);
            }
        }
        /// <summary>
        /// Gets the domain settings object which best matches the supplied hostName.
        /// </summary>
        /// <param name="hostName"></param>
        /// <returns></returns>
        public static DomainSettings GetBestMatch(string hostName)
        {
            return DomainSettings.GetBestMatch(hostName, true);
        }
        /// <summary>
        /// Gets the domain settings object which best matches the supplied hostName.
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="recurse"></param>
        /// <returns></returns>
        public static DomainSettings GetBestMatch(string hostName, bool recurse)
        {
            if (string.IsNullOrEmpty(hostName) == false)
            {
                if (DomainSettings.settings.ContainsKey(hostName) == true)
                {
                    return DomainSettings.settings[hostName];
                }
                else if (recurse == true)
                {
                    return DomainSettings.GetParent(hostName);
                }
                else
                {
                    return DomainSettings.root;
                }
            }
            else
            {
                //the root settings are the only domain settings instance that have an empty name.
                return DomainSettings.root;
            }
        }
        public static DomainSettings GetParent(string hostName)
        {
            if (string.IsNullOrEmpty(hostName) == false)
            {
                string[] oldNames = hostName.Split('.');
                string[] newNames = new string[oldNames.Length - 1];
                Array.Copy(oldNames, newNames, newNames.Length);
                string newHostName = string.Join(".", newNames);
                if (DomainSettings.settings.ContainsKey(newHostName) == true)
                {
                    return DomainSettings.settings[newHostName];
                }
                else
                {
                    return DomainSettings.GetParent(newHostName);
                }
            }
            else
            {
                return DomainSettings.root;
            }
        }
        public static bool LoadAll()
        {
            if (Directory.Exists(SPath.ResolveSpecialPath(SpecialFolder.Domains)))
            {

                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool SaveAll()
        {
            foreach (DomainSettings ds in DomainSettings.settings.Values)
            {
                DomainSettings.Save(ds);
            }

            return true;
        }
        public static bool Save(DomainSettings settings)
        {
            if (Directory.Exists(SPath.ResolveSpecialPath(SpecialFolder.Domains)))
            {
                using (FileStream fs = File.Open(Path.Combine(SPath.DomainsFolder, settings.name + ".ds"), FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    formatter.Serialize(fs, settings);
                    fs.Close();
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region Properties - Public
        public static DomainSettings Current
        {
            get
            {
                return DomainSettings.current;
            }
            set
            {
                DomainSettings.current = value;
            }
        }
        /// <summary>
        /// Gets a boolean value which indicates if the current DomainSettings has a parent.
        /// </summary>
        public bool HasParent
        {
            get
            {
                return this.hasParent;
            }
        }
        #endregion
    }
}
