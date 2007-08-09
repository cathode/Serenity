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

namespace Serenity
{
	/// <summary>
	/// Represents a page that generates dynamic content.
	/// </summary>
	public abstract class ContentPage : Page
	{
		public abstract ContentPage CreateInstance();
		public virtual MasterPage CreateMasterPageInstance()
		{
			return null;
		}
		#region Properties - Public
		/// <summary>
		/// Gets a boolean value which indicates if the current Page uses a MasterPage.
		/// </summary>
		public virtual bool HasMasterPage
		{
			get
			{
				return (this.CreateMasterPageInstance() == null) ? false : true;
			}
		}
		#endregion
	}
}
