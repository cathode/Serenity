/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
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
