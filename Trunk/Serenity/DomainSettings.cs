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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using Serenity.Properties;

namespace Serenity
{
    /// <summary>
    /// Represents a collection of settings that are specific to a domain name.
    /// </summary>
    public sealed class DomainSettings : Multiton<string, DomainSettings>
    {
        #region Constructors - Private
        static DomainSettings()
        {
            if (Directory.Exists(SPath.DomainsFolder) == false)
            {
                Directory.CreateDirectory(SPath.DomainsFolder);
            }
            DomainSettings.SystemInstance = new DomainSettings("");

            DomainSettings.SystemInstance.DefaultEnvironment.Value = GlobalSettings.DefaultEnvironment;
            DomainSettings.SystemInstance.DefaultModule.Value = GlobalSettings.DefaultModule;
            DomainSettings.SystemInstance.DefaultTheme.Value = GlobalSettings.DefaultTheme;
            DomainSettings.SystemInstance.DefaultResourceClass.Value = GlobalSettings.DefaultResourceClass;
            DomainSettings.SystemInstance.DefaultResourceName.Value = GlobalSettings.DefaultResourceName;
        }
        #endregion
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the DomainSettings class.
        /// </summary>
        /// <param name="name"></param>
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
