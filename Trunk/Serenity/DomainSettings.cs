/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://serenityproject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
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
    public sealed class DomainSettings : Multiton<string, DomainSettings>
    {
        #region Constructors - Public
        static DomainSettings()
        {
            if (Directory.Exists(SPath.DomainsFolder) == false)
            {
                Directory.CreateDirectory(SPath.DomainsFolder);
            }
            DomainSettings.SystemInstance = new DomainSettings("");
            if (Settings.Default.ActiveEnvironments != null)
            {
                DomainSettings.SystemInstance.ActiveEnvironments.Value = new string[Settings.Default.ActiveEnvironments.Count];
                Settings.Default.ActiveEnvironments.CopyTo(DomainSettings.SystemInstance.ActiveEnvironments.Value, 0);
            }
            if (Settings.Default.ActiveModules != null)
            {
                DomainSettings.SystemInstance.ActiveModules.Value = new string[Settings.Default.ActiveModules.Count];
                Settings.Default.ActiveModules.CopyTo(DomainSettings.SystemInstance.ActiveModules.Value, 0);
            }
            if (Settings.Default.ActiveThemes != null)
            {
                DomainSettings.SystemInstance.ActiveThemes.Value = new string[Settings.Default.ActiveThemes.Count];
                Settings.Default.ActiveThemes.CopyTo(DomainSettings.SystemInstance.ActiveThemes.Value, 0);
            }

            DomainSettings.SystemInstance.DefaultEnvironment.Value = Settings.Default.DefaultEnvironment;
            DomainSettings.SystemInstance.DefaultModule.Value = Settings.Default.DefaultModule;
            DomainSettings.SystemInstance.DefaultTheme.Value = Settings.Default.DefaultTheme;
            DomainSettings.SystemInstance.DefaultResourceClass.Value = Settings.Default.DefaultResourceClass;
            DomainSettings.SystemInstance.DefaultResourceName.Value = Settings.Default.DefaultResourceName;

            if (Settings.Default.InactiveEnvrionments != null)
            {
                DomainSettings.SystemInstance.InactiveEnvironments.Value = new string[Settings.Default.InactiveEnvrionments.Count];
                Settings.Default.InactiveEnvrionments.CopyTo(DomainSettings.SystemInstance.InactiveEnvironments.Value, 0);
            }
            if (Settings.Default.InactiveModules != null)
            {
                DomainSettings.SystemInstance.InactiveModules.Value = new string[Settings.Default.InactiveModules.Count];
                Settings.Default.InactiveModules.CopyTo(DomainSettings.SystemInstance.InactiveModules.Value, 0);
            }
            if (Settings.Default.InactiveThemes != null)
            {
                DomainSettings.SystemInstance.InactiveThemes.Value = new string[Settings.Default.InactiveThemes.Count];
                Settings.Default.InactiveThemes.CopyTo(DomainSettings.SystemInstance.InactiveThemes.Value, 0);
            }

        }
        public DomainSettings(string name) : base(name)
        {
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
                this.InactiveEnvironments = new DomainSettingValue<string[]>();
                this.InactiveModules = new DomainSettingValue<string[]>();
                this.InactiveThemes = new DomainSettingValue<string[]>();
                this.OmitEnvironment = new DomainSettingValue<bool>();
                this.OmitResourceClass = new DomainSettingValue<bool>();
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
                this.InactiveEnvironments = new DomainSettingValue<string[]>(this.parent.InactiveEnvironments);
                this.InactiveModules = new DomainSettingValue<string[]>(this.parent.InactiveModules);
                this.InactiveThemes = new DomainSettingValue<string[]>(this.parent.InactiveThemes);
                this.OmitEnvironment = new DomainSettingValue<bool>(this.parent.OmitEnvironment);
                this.OmitResourceClass = new DomainSettingValue<bool>(this.parent.OmitResourceClass);
            }
        }
        #endregion
        #region Fields - Private
        [NonSerialized]
        private bool hasParent;
        [NonSerialized]
        private readonly DomainSettings parent;
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
                if (DomainSettings.ContainsInstance(hostName) == true)
                {
                    return DomainSettings.GetInstance(hostName);
                }
                else if (recurse == true)
                {
                    return DomainSettings.GetParent(hostName);
                }
                else
                {
                    return DomainSettings.SystemInstance;
                }
            }
            else
            {
                //system instance is the only domain settings instance that has an empty name.
                return DomainSettings.SystemInstance;
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
                if (DomainSettings.ContainsInstance(newHostName) == true)
                {
                    return DomainSettings.GetInstance(newHostName);
                }
                else
                {
                    return DomainSettings.GetParent(newHostName);
                }
            }
            else
            {
                //system instance is the only domain settings instance that has an empty name.
                return DomainSettings.SystemInstance;
            }
        }
        public static void LoadAll()
        {
            /*
             * Domain loading not implemented yet!
            lock (DomainSettings)
            {
                
                foreach (string domain in Directory.GetFiles(SPath.DomainsDirectory))
                {

                }
            }
             * */
        }
        public static void Save(DomainSettings settings)
        {
            if (settings != DomainSettings.SystemInstance)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    formatter.Serialize(ms, settings);
                    ms.Close();

                    File.WriteAllBytes(Path.Combine(SPath.DomainsFolder, settings.Key), ms.ToArray());
                }
            }
            else
            {
                throw new ArgumentException("Cannot specify SystemInstance as object to be saved.");
            }
        }
        #endregion
        #region Properties - Public
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
