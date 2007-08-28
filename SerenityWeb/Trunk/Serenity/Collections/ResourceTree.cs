/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Collections
{
	/// <summary>
	/// Represents a tree of resources.
	/// </summary>
	public abstract class ResourceTree : ResourceTreeElement
	{
		#region Methods - Public
		/// <summary>
		/// Initializes a new instance of the ResourceTree class.
		/// </summary>
		public ResourceTree()
		{

		}
		#endregion
		#region Fields - Private

		#endregion
		#region Methods - Public
		public bool AddResource(string relativeUri, Resource resource)
		{
			return false;
		}
		#endregion
	}
	
}
