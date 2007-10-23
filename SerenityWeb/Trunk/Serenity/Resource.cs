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
using System.Text;

using Serenity.Web;

namespace Serenity
{
	/// <summary>
	/// Provides a base class that all web-accessible resources must inherit from.
	/// </summary>
	public abstract class Resource
	{
		#region Fields - Protected
		/// <summary>
		/// Holds the name of the resource.
		/// </summary>
		protected string name;
		#endregion
		#region Properties - Public
		/// <summary>
		/// Gets the MimeType that should be used to describe the content of the current Resource.
		/// </summary>
		public virtual MimeType MimeType
		{
			get
			{
				return MimeType.Default;
			}
		}
		/// <summary>
		/// Gets or sets the name of the current Resource.
		/// </summary>
		public virtual string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}
		/// <summary>
		/// Gets the name of the resource in a form that can be "safely" used.
		/// </summary>
		public string SystemName
		{
			get
			{
				return this.Name.ToLower();
			}
		}
		#endregion
	}
}
