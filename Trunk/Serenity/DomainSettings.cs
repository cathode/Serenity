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
    public sealed class InstanceManager<T> where T : class
    {
        #region Constructors - Private
        /// <summary>
        /// Initializes the static members of the Multiton class.
        /// </summary>
        static InstanceManager()
        {
            InstanceManager<T>.manager = new InstanceManager<T>();
        }
        private InstanceManager()
        {
            this.instances = new Dictionary<string, T>();
        }
        #endregion
        #region Fields - Private
        [ThreadStatic]
        private T currentInstance;
        private T defaultInstance;
        private Dictionary<string, T> instances;
        private static InstanceManager<T> manager;
        private T systemInstance;
        #endregion
        #region Methods - Public
        public T GetInstance(string key)
        {
            if (this.ContainsKey(key))
            {
                return this.instances[key];
            }
            else
            {
                return default(T);
            }
        }
        public bool ContainsKey(string key)
        {
            return this.instances.ContainsKey(key);
        }
        public bool ContainsInstance(T instance)
        {
            return this.instances.ContainsValue(instance);
        }
        #endregion
        #region Properties - Public
        public T CurrentInstance
        {
            get
            {
                return this.currentInstance;
            }
            set
            {
                this.currentInstance = value;
            }
        }
        /// <summary>
        /// Gets or sets the default instance.
        /// </summary>
        public T DefaultInstance
        {
            get
            {
                if (this.defaultInstance != null)
                {
                    return this.defaultInstance;
                }
                else
                {
                    return this.systemInstance;
                }
            }
            set
            {
                this.defaultInstance = value;
            }
        }
        public static InstanceManager<T> Default
        {
            get
            {
                return InstanceManager<T>.manager;
            }
        }
        /// <summary>
        /// Gets or sets the system instance.
        /// </summary>
        public T SystemInstance
        {
            get
            {
                return this.systemInstance;
            }
            set
            {
                if ((this.systemInstance == null) && (value != null))
                {
                    this.systemInstance = value;
                }
            }
        }
        #endregion  
    }

    /// <summary>
    /// Represents a collection of settings that are specific to a domain name.
    /// </summary>
    public sealed class DomainSettings
    {
        #region Constructors - Private
        static DomainSettings()
        {
            if (Directory.Exists(SPath.DomainsFolder) == false)
            {
                Directory.CreateDirectory(SPath.DomainsFolder);
            }
            DomainSettings sys = new DomainSettings("");

            sys.DefaultEnvironment.Value = GlobalSettings.DefaultEnvironment;
            sys.DefaultModule.Value = GlobalSettings.DefaultModule;
            sys.DefaultTheme.Value = GlobalSettings.DefaultTheme;
            sys.DefaultResourceClass.Value = GlobalSettings.DefaultResourceClass;
            sys.DefaultResourceName.Value = GlobalSettings.DefaultResourceName;
            sys.OutputCompressionThreshhold.Value = 4096;

            InstanceManager<DomainSettings>.Default.SystemInstance = sys;
        }
        #endregion
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the DomainSettings class.
        /// </summary>
        /// <param name="name"></param>
        public DomainSettings(string name)
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
                this.OutputCompressionThreshhold = new DomainSettingValue<int>();
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
                this.OutputCompressionThreshhold = new DomainSettingValue<int>(this.parent.OutputCompressionThreshhold);
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
        public readonly DomainSettingValue<int> OutputCompressionThreshhold;
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
                if (InstanceManager<DomainSettings>.Default.ContainsKey(hostName) == true)
                {
                    return InstanceManager<DomainSettings>.Default.GetInstance(hostName);
                }
                else if (recurse == true)
                {
                    return DomainSettings.GetParent(hostName);
                }
                else
                {
                    return InstanceManager<DomainSettings>.Default.SystemInstance;
                }
            }
            else
            {
                //system instance is the only domain settings instance that has an empty name.
                return InstanceManager<DomainSettings>.Default.SystemInstance;
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
                if (InstanceManager<DomainSettings>.Default.ContainsKey(newHostName) == true)
                {
                    return InstanceManager<DomainSettings>.Default.GetInstance(newHostName);
                }
                else
                {
                    return DomainSettings.GetParent(newHostName);
                }
            }
            else
            {
                //system instance is the only domain settings instance that has an empty name.
                return InstanceManager<DomainSettings>.Default.SystemInstance;
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
            if (settings != InstanceManager<DomainSettings>.Default.SystemInstance)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    formatter.Serialize(ms, settings);
                    ms.Close();

                    File.WriteAllBytes(Path.Combine(SPath.DomainsFolder, settings.ToString()), ms.ToArray());
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
