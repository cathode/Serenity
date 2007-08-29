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
	/// Represents an element within a ResourceTree, which can contain children.
	/// </summary>
	public class ResourceTreeElement : IEnumerable<ResourceTreeElement>
	{
		#region Constructors - Public
		/// <summary>
		/// Initializes a new instance of the ResourceTreeElement class.
		/// </summary>
		public ResourceTreeElement()
		{

		}
		public ResourceTreeElement(Resource value)
		{
			this.value = value;
		}
		#endregion
		#region Fields - Private
		private List<ResourceTreeElement> children = new List<ResourceTreeElement>();
		private Resource value;
		#endregion
		#region Methods - Public
		/// <summary>
		/// Gets a generic enumerator for the current ResourceTreeElement,
		/// and allows enumeration over direct children.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<ResourceTreeElement> GetEnumerator()
		{
			foreach (ResourceTreeElement item in this.children)
			{
				yield return item;
			}
		}
		#endregion
		#region Properties - Public
		/// <summary>
		/// Gets the number of elements that are direct descendants of the current ResourceTreeElement.
		/// </summary>
		public int Count
		{
			get
			{
				return this.children.Count;
			}
		}
		/// <summary>
		/// Gets the Resource located at the current ResourceTreeElement.
		/// </summary>
		public Resource Value
		{
			get
			{
				return this.value;
			}
			internal set
			{
				this.value = value;
			}
		}
		#endregion
		#region IEnumerable Members
		/// <summary>
		/// Gets a nongeneric enumerator.
		/// </summary>
		/// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion
	}
}
