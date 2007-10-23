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

namespace Serenity.Attributes
{
	/// <summary>
	/// Provides a way to specify the resource namespace of a module.
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class ModuleResourceNamespaceAttribute : Attribute
	{
		#region Constructors - Public
		/// <summary>
		/// Initializes a new instance of the ModuleResourceNamespaceAttribute class.
		/// </summary>
		/// <param name="resourceNamespace">The Resource Namespace of the module.</param>
		public ModuleResourceNamespaceAttribute(string resourceNamespace)
		{
			this.ResourceNamespace = resourceNamespace;
		}
		#endregion
		#region Fields - Public
		/// <summary>
		/// Holds the Resource Namespace of the module.
		/// </summary>
		public readonly string ResourceNamespace;
		#endregion
	}
}
