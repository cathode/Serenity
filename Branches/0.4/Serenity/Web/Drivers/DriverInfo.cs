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
using System.Text;

namespace Serenity.Web.Drivers
{
	/// <summary>
	/// Provides a way for WebDrivers to expose information about themselves.
	/// </summary>
	public sealed class DriverInfo
	{
		#region Constructors - Internal
		internal DriverInfo()
		{
			this.className = "unknown";
			this.version = new Version();
			this.typeName = "unknown";
			this.uriSchema = "unknown";
		}
		public DriverInfo(string className, string typeName, string uriSchema, Version version)
		{
			this.className = className;
			this.typeName = typeName;
			this.uriSchema = uriSchema;
			this.version = version;
		}
		#endregion
		#region Fields - Private
		private string className;
		private string typeName;
		private string uriSchema;
		private Version version;
		#endregion
		#region Properties - Public
		/// <summary>
		/// Gets a string describing the classification of the current DriverInfo.
		/// </summary>
		public string ClassName
		{
			get
			{
				return this.className;
			}
			internal set
			{
				this.className = value;
			}
		}
		/// <summary>
		/// Gets a string describing the type of the current DriverInfo.
		/// </summary>
		public string TypeName
		{
			get
			{
				return this.typeName;
			}
			internal set
			{
				this.typeName = value;
			}
		}
		/// <summary>
		/// Gets the URI schema used by the current DriverInfo.
		/// </summary>
		public string UriSchema
		{
			get
			{
				return this.uriSchema;
			}
			internal set
			{
				this.uriSchema = value;
			}
		}
		/// <summary>
		/// Gets the version of the current DriverInfo.
		/// </summary>
		public Version Version
		{
			get
			{
				return this.version;
			}
			internal set
			{
				this.version = value;
			}
		}
		#endregion
	}
}
