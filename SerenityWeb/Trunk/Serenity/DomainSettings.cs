/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using LibINI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Serenity
{
	/// <summary>
	/// Represents a collection of settings that are specific to a domain name.
	/// </summary>
	public sealed class DomainSettings
	{
		#region Constructors - Private
		static DomainSettings()
		{
			DomainSettings.instances = new Dictionary<string, DomainSettings>();
			DomainSettings.root = new DomainSettings("root");
			DomainSettings.instances.Add(root.name, root);
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
			this.parent = DomainSettings.GetParent(name);
		}
		#endregion
		#region Fields - Private
		private string defaultResourceClass;
		private bool defaultResourceClassIsDefined = false;
		private string defaultResourceName;
		private bool defaultResourceNameIsDefined = false;
		private string documentRoot;
		private bool documentRootIsDefined = false;
		private static readonly Dictionary<string, DomainSettings> instances;
		private readonly string name;
		private bool omitResourceClass;
		private bool omitResourceClassIsDefined = false;
		private DomainSettings parent = null;
		private static readonly DomainSettings root;
		private string themeName;
		private bool themeNameIsDefined = false;
		#endregion
		#region Fields - Public
		[ThreadStatic]
		public static DomainSettings Current;
		public const string DefaultDefaultResourceClass = "static";
		public const string DefaultDefaultResourceName = "default.html";
		public const string DefaultDocumentRoot = "./Domains/Common/";
		public const bool DefaultOmitResourceClass = false;
		public const string DefaultThemeName = "system";
		#endregion
		#region Methods - Public
		/// <summary>
		/// Gets the domain settings object which best matches the supplied hostUrl.
		/// </summary>
		/// <param name="hostUrl"></param>
		/// <returns></returns>
		public static DomainSettings GetBestMatch(Uri hostUrl)
		{
			if (hostUrl != null)
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
			else
			{
				return DomainSettings.root;
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
			if (!string.IsNullOrEmpty(hostName))
			{
				if (DomainSettings.instances.ContainsKey(hostName) == true)
				{
					return DomainSettings.instances[hostName];
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
			if (!string.IsNullOrEmpty(hostName))
			{
				string[] oldNames = hostName.Split('.');
				string[] newNames = new string[oldNames.Length - 1];
				Array.Copy(oldNames, newNames, newNames.Length);
				string newHostName = string.Join(".", newNames);
				if (DomainSettings.instances.ContainsKey(newHostName) == true)
				{
					return DomainSettings.instances[newHostName];
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
			string domainPath = SPath.ResolveSpecialPath(SpecialFolder.Domains);
			DomainSettings oldRoot = DomainSettings.root;
			DomainSettings.instances.Clear();
			if (Directory.Exists(domainPath))
			{
				string[] files = Directory.GetFiles(domainPath);
				foreach (string path in files)
				{
					DomainSettings settings = DomainSettings.Load(Path.GetFileNameWithoutExtension(path));
					if (settings != null)
					{
						DomainSettings.instances.Add(settings.name, settings);
					}
				}
				DomainSettings.RecomputeRelationships();
				return true;
			}
			else
			{
				return false;
			}
		}
		public static bool SaveAll()
		{
			foreach (DomainSettings ds in DomainSettings.instances.Values)
			{
				DomainSettings.Save(ds);
			}

			return true;
		}
		public static DomainSettings Load(string name)
		{
			string path = Path.GetFullPath(SPath.Combine(SPath.DomainsFolder, name + ".ini"));

            return DomainSettings.LoadFile(path);
		}
        public static DomainSettings LoadFile(string path)
        {
            DomainSettings settings = null;

            if (File.Exists(path))
            {
                string name = Path.GetFileNameWithoutExtension(path);
                if (DomainSettings.instances.ContainsKey(name))
                {
                    settings = DomainSettings.instances[name];
                }
                else
                {
                    settings = new DomainSettings(name);
                }
                IniFile file = new IniFile(path);
                file.CaseSensitiveRetrieval = false;

                file.Load();

                if (file.ContainsSection("DomainSettings"))
                {
                    IniSection section = file["DomainSettings"];

                    if (section.ContainsEntry("DefaultResourceName"))
                    {
                        settings.DefaultResourceName = section["DefaultResourceName"].Value;
                    }
                    if (section.ContainsEntry("DefaultResourceClass"))
                    {
                        settings.DefaultResourceClass = section["DefaultResourceClass"].Value;
                    }
                    if (section.ContainsEntry("DocumentRoot"))
                    {
                        settings.DocumentRoot = Path.GetFullPath(section["DocumentRoot"].Value);
                    }
                    if (section.ContainsEntry("OmitResourceClass"))
                    {
                        try
                        {
                            settings.OmitResourceClass = bool.Parse(section["OmitResourceClass"].Value);
                        }
                        catch
                        {
                        }
                    }
                    if (section.ContainsEntry("ThemeName"))
                    {
                        settings.ThemeName = section["ThemeName"].Value;
                    }
                }
            }
            return settings;
        }
		public static bool Save(DomainSettings settings)
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
		public static void RecomputeRelationships()
		{
			foreach (DomainSettings settings in DomainSettings.instances.Values)
			{
				settings.parent = DomainSettings.GetParent(settings.name);
			}
		}
		#endregion
		#region Properties - Public
		public static int Count
		{
			get
			{
				return DomainSettings.instances.Count;
			}
		}
		public string DefaultResourceClass
		{
			get
			{
				if (this.defaultResourceClassIsDefined)
				{
					return this.defaultResourceClass;
				}
				else if (this.HasParent)
				{
					return this.parent.DefaultResourceClass;
				}
				else
				{
					return DomainSettings.DefaultDefaultResourceClass;
				}
			}
			set
			{
				this.defaultResourceClassIsDefined = (value == null) ? false : true;
				this.defaultResourceClass = value;
			}
		}
		public bool DefaultResourceClassIsDefined
		{
			get
			{
				return this.defaultResourceClassIsDefined;
			}
			set
			{
				this.defaultResourceClassIsDefined = value;
			}
		}
		public string DefaultResourceName
		{
			get
			{
				if (this.defaultResourceNameIsDefined)
				{
					return this.defaultResourceName;
				}
				else if (this.HasParent)
				{
					return this.parent.DefaultResourceName;
				}
				else
				{
					return DomainSettings.DefaultDefaultResourceName;
				}
			}
			set
			{
				this.defaultResourceNameIsDefined = (value == null) ? false : true;
				this.defaultResourceName = value;
			}
		}
		public bool DefaultResourceNameIsDefined
		{
			get
			{
				return this.defaultResourceNameIsDefined;
			}
			set
			{
				this.defaultResourceNameIsDefined = value;
			}
		}
		public string DocumentRoot
		{
			get
			{
				if (this.documentRootIsDefined)
				{
					return this.documentRoot;
				}
				else if (this.HasParent)
				{
					return this.parent.DocumentRoot;
				}
				else
				{
					return DomainSettings.DefaultDocumentRoot;
				}
			}
			set
			{
				this.documentRootIsDefined = (value == null) ? false : true;
				this.documentRoot = value;
			}
		}
		public bool DocumentRootIsDefined
		{
			get
			{
				return this.documentRootIsDefined;
			}
			set
			{
				this.documentRootIsDefined = value;
			}
		}
		/// <summary>
		/// Gets a boolean value which indicates if the current DomainSettings has a parent.
		/// </summary>
		public bool HasParent
		{
			get
			{
				return (this.parent == null) ? false : true;
			}
		}
		public string Name
		{
			get
			{
				return this.name;
			}
		}
		public bool OmitResourceClass
		{
			get
			{
				if (this.omitResourceClassIsDefined)
				{
					return this.omitResourceClass;
				}
				else if (this.HasParent)
				{
					return this.parent.OmitResourceClass;
				}
				else
				{
					return DomainSettings.DefaultOmitResourceClass;
				}
			}
			set
			{
				this.omitResourceClassIsDefined = true;
				this.omitResourceClass = value;
			}
		}
		public bool OmitResourceClassIsDefined
		{
			get
			{
				return this.omitResourceClassIsDefined;
			}
			set
			{
				this.omitResourceClassIsDefined = value;
			}
		}
		public string ThemeName
		{
			get
			{
				if (this.themeNameIsDefined)
				{
					return this.themeName;
				}
				else if (this.HasParent)
				{
					return this.parent.ThemeName;
				}
				else
				{
					return DomainSettings.DefaultThemeName;
				}
			}
			set
			{
				this.themeNameIsDefined = (value == null) ? false : true;
				this.themeName = value;
			}
		}
		public bool ThemeNameIsDefined
		{
			get
			{
				return this.themeNameIsDefined;
			}
			set
			{
				this.themeNameIsDefined = value;
			}
		}
		#endregion
	}
}
