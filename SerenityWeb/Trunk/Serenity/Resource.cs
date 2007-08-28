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
	public abstract class Resource
	{
		#region Fields - Protected
		protected string name;
		#endregion
		#region Properties - Public
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
