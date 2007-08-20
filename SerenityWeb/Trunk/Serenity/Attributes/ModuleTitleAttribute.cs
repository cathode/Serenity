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

namespace Serenity.Attributes
{
	/// <summary>
	/// Provides a way to specify the title of a module.
	/// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class ModuleTitleAttribute : Attribute
	{
		#region Constructors - Public
		/// <summary>
		/// Initializes a new instance of the ModuleTitleAttribute class.
		/// </summary>
		/// <param name="title"></param>
		public ModuleTitleAttribute(string title)
        {
            this.Title = title;
		}
		#endregion
		#region Fields - Public
		/// <summary>
		/// Holds the title of the module.
		/// </summary>
		public readonly string Title;
		#endregion
	}
}
