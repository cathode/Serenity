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
			DomainSettings.root = DomainSettings.CreateDefaultRoot();
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
				this.defaultEnvironment = new DomainSettingValue<string>();
				this.omitEnvironment = new DomainSettingValue<bool>();
				this.omitResourceClass = new DomainSettingValue<bool>();
				this.compressionThreshhold = new DomainSettingValue<int>();
			}
			else
			{
				this.parent = parent;
				this.defaultEnvironment = new DomainSettingValue<string>(this.parent.DefaultEnvironment);
				this.omitEnvironment = new DomainSettingValue<bool>(this.parent.omitEnvironment);
				this.omitResourceClass = new DomainSettingValue<bool>(this.parent.omitResourceClass);
				this.compressionThreshhold = new DomainSettingValue<int>(this.parent.CompressionThreshhold);
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
		private static DomainSettings root;
		[NonSerialized]
		private static readonly Dictionary<string, DomainSettings> settings;
		#endregion
		#region Fields - Public
		private readonly DomainSettingValue<string> defaultEnvironment;
		private readonly DomainSettingValue<bool> omitEnvironment;
		private readonly DomainSettingValue<bool> omitResourceClass;
		private readonly DomainSettingValue<int> compressionThreshhold;
		#endregion
		#region Methods - Private
		private static DomainSettings CreateDefaultRoot()
		{
			DomainSettings root = new DomainSettings("");
			root.defaultEnvironment.Value = "system";
			root.compressionThreshhold.Value = 8192;
			root.omitEnvironment.Value = false;
			root.omitResourceClass.Value = false;
			return root;
		}
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
			string domainPath = SPath.ResolveSpecialPath(SpecialFolder.Domains);
			DomainSettings oldRoot = DomainSettings.root;
			DomainSettings.settings.Clear();
			if (Directory.Exists(domainPath))
			{
				string[] files = Directory.GetFiles(domainPath);
				foreach (string path in files)
				{
					DomainSettings settings = DomainSettings.Load(path);
					if (settings != null)
					{
						DomainSettings.settings.Add(settings.name, settings);
					}
				}
				if (DomainSettings.settings.ContainsKey(""))
				{
					DomainSettings.root = DomainSettings.settings[""];
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
			foreach (DomainSettings ds in DomainSettings.settings.Values)
			{
				DomainSettings.Save(ds);
			}

			return true;
		}
		public static DomainSettings Load(string path)
		{
			//start with null, thats what is returned if it couldnt be loaded.
			DomainSettings result = null;

			if (File.Exists(path))
			{
				using (FileStream fs = File.Open(path, FileMode.Open))
				{
					BinaryFormatter formatter = new BinaryFormatter();

					object settings = formatter.Deserialize(fs);

					if (settings is DomainSettings)
					{
						result = (DomainSettings)settings;
					}
				}
			}
			return result;
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
		public DomainSettingValue<int> CompressionThreshhold
		{
			get
			{
				return this.compressionThreshhold;
			}
		}

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
		public DomainSettingValue<string> DefaultEnvironment
		{
			get
			{
				return this.defaultEnvironment;
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
		public DomainSettingValue<bool> OmitEnvironment
		{
			get
			{
				return this.omitEnvironment;
			}
		}
		public DomainSettingValue<bool> OmitResourceClass
		{
			get
			{
				return this.omitResourceClass;
			}
		}
		#endregion
	}
}
