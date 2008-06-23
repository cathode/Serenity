/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
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
	
    
	public static class SPath
	{
		#region Constructors - Static
		static SPath()
		{
			SPath.specialFolders = new Dictionary<SpecialDirectory, string>();
			SPath.specialFiles = new Dictionary<SpecialFile, string>();

			string root = Path.GetFullPath("./");

			SPath.specialFolders[SpecialDirectory.Configuration] = Path.GetFullPath(SPath.Combine(root, "Configuration"));
			SPath.specialFolders[SpecialDirectory.Domains] = Path.GetFullPath(SPath.Combine(root, "Domains"));
			SPath.specialFolders[SpecialDirectory.Logs] = Path.GetFullPath(SPath.Combine(root, "Logs"));
			SPath.specialFolders[SpecialDirectory.Modules] = Path.GetFullPath(SPath.Combine(root, "Modules"));
			SPath.specialFolders[SpecialDirectory.Root] = root;
			SPath.specialFolders[SpecialDirectory.Themes] = Path.GetFullPath(SPath.Combine(root, "Themes"));

			SPath.specialFiles[SpecialFile.FileTypeRegistry] = SPath.Combine(SPath.specialFolders[SpecialDirectory.Configuration], "FileTypeRegistry.ini");
		}
		#endregion
		#region Fields - Private
		private static Dictionary<SpecialDirectory, string> specialFolders;
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
		public static string ResolveSpecialPath(SpecialDirectory specialFolder)
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
				return SPath.ResolveSpecialPath(SpecialDirectory.Domains);
			}
		}
		/// <summary>
		/// Gets an absolute path to the directory where log files are stored.
		/// </summary>
		public static string LogsFolder
		{
			get
			{
				return SPath.ResolveSpecialPath(SpecialDirectory.Logs);
			}
		}

		/// <summary>
		/// Gets an absolute path to the directory where modules are stored.
		/// </summary>
		public static string ModulesFolder
		{
			get
			{
				return SPath.ResolveSpecialPath(SpecialDirectory.Modules);
			}
		}
		/// <summary>
		/// Gets an absolute path to the root directory where data is stored.
		/// </summary>
		public static string RootFolder
		{
			get
			{
				return SPath.ResolveSpecialPath(SpecialDirectory.Root);
			}
		}
		/// <summary>
		/// Gets an absolute path to the directory where themes are stored.
		/// </summary>
		public static string ThemesFolder
		{
			get
			{
				return SPath.ResolveSpecialPath(SpecialDirectory.Themes);
			}
		}
		#endregion
	}
}
