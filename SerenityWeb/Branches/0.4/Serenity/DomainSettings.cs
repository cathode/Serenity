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
			DomainSettings root = new DomainSettings("root");
			root.defaultResourceName.Value = "system";
			root.defaultResourceClass.Value = "dynamic";
			root.omitResourceClass.Value = false;
			root.theme.Value = SerenityInfo.SystemName;
			root.documentRoot.Value = "Domains/Common";
			DomainSettings.root = root;
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

			DomainSettings parent = DomainSettings.GetParent(name);

			if (parent == null)
			{
				this.hasParent = false;
				this.parent = null;
				this.defaultResourceName = new DomainSettingValue<string>();
				this.defaultResourceClass = new DomainSettingValue<string>();
				this.documentRoot = new DomainSettingValue<string>();
				this.omitResourceClass = new DomainSettingValue<bool>();
				this.theme = new DomainSettingValue<string>();
			}
			else
			{
				this.parent = parent;
				this.defaultResourceName = new DomainSettingValue<string>(this.parent.defaultResourceName);
				this.defaultResourceClass = new DomainSettingValue<string>(this.parent.defaultResourceClass);
				this.documentRoot = new DomainSettingValue<string>(this.parent.documentRoot);
				this.omitResourceClass = new DomainSettingValue<bool>(this.parent.omitResourceClass);
				this.theme = new DomainSettingValue<string>(this.parent.theme);
			}
		}
		#endregion
		#region Fields - Private
		private readonly DomainSettingValue<string> defaultResourceName;
		private readonly DomainSettingValue<string> defaultResourceClass;
		private readonly DomainSettingValue<string> documentRoot;
		private bool hasParent;
		private readonly string name;
		private readonly DomainSettings parent;
		private static readonly DomainSettings root;
		private static readonly Dictionary<string, DomainSettings> instances;
		private readonly DomainSettingValue<bool> omitResourceClass;
		private readonly DomainSettingValue<string> theme;
		#endregion
		#region Fields - Public
		[ThreadStatic]
		public static DomainSettings Current;
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
			if (string.IsNullOrEmpty(hostName) == false)
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
			if (string.IsNullOrEmpty(hostName) == false)
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
					DomainSettings settings = DomainSettings.Load(path);
					if (settings != null)
					{
						DomainSettings.instances.Add(settings.name, settings);
					}
				}
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
			DomainSettings result = null;
			string path = SPath.Combine(SPath.DomainsFolder, name + ".ini");
			if (File.Exists(path))
			{
				
			}
			return result;
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
		#endregion
		#region Properties - Public
		public static int Count
		{
			get
			{
				return DomainSettings.instances.Count;
			}
		}
		public string DefaultResource
		{
			get
			{
				return this.defaultResourceName.Value;
			}
		}
		public string DefaultResourceClass
		{
			get
			{
				return this.defaultResourceClass.Value;
			}
		}
		public string DocumentRoot
		{
			get
			{
				return this.documentRoot.Value;
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
				return this.omitResourceClass.Value;
			}
		}
		public string Theme
		{
			get
			{
				return this.theme.Value;
			}
		}
		#endregion
	}
}
