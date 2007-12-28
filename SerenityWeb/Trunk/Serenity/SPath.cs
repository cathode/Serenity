/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Serenity
{
	/// <summary>
	/// Represents special folders used by Serenity.
	/// </summary>
	public enum SpecialFolder
	{
		/// <summary>
		/// Represents the root folder (where all the other data is kept).
		/// </summary>
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
    /// <summary>
    /// Represents special files used by Serenity.
    /// </summary>
	public enum SpecialFile
	{
		FileTypeRegistry,
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
			SPath.specialFolders[SpecialFolder.Logs] = Path.GetFullPath(SPath.Combine(root, "Logs"));
			SPath.specialFolders[SpecialFolder.Modules] = Path.GetFullPath(SPath.Combine(root, "Modules"));
			SPath.specialFolders[SpecialFolder.Root] = root;
			SPath.specialFolders[SpecialFolder.Themes] = Path.GetFullPath(SPath.Combine(root, "Themes"));

			SPath.specialFiles[SpecialFile.FileTypeRegistry] = SPath.Combine(SPath.specialFolders[SpecialFolder.Configuration], "FileTypeRegistry.ini");
		}
		#endregion
		#region Fields - Private
		private static Dictionary<SpecialFolder, string> specialFolders;
		private static Dictionary<SpecialFile, string> specialFiles;
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
		public static string ResolveSpecialPath(SpecialFolder specialFolder)
		{
			return SPath.specialFolders[specialFolder];
		}
		public static string ResolveSpecialPath(SpecialFile specialFile)
		{
			return SPath.specialFiles[specialFile];
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
