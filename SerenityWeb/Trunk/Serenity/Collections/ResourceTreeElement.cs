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
	public class ResourceTreeElement : IEnumerable<ResourceTreeElement>
	{
		public ResourceTreeElement()
		{

		}
		#region Fields - Private
		private List<ResourceTreeElement> children = new List<ResourceTreeElement>();
		private Resource value;
		#endregion
		#region Methods - Public
		public IEnumerator<ResourceTreeElement> GetEnumerator()
		{
			foreach (ResourceTreeElement item in this.children)
			{
				yield return item;
			}
		}
		#endregion
		#region Properties - Public
		public int Count
		{
			get
			{
				return this.children.Count;
			}
		}
		/*
		public int DeepCount
		{
			get
			{
				if (this.Count > 0)
				{
					int n = this.Count;
					foreach (ResourceTreeElement<T> branch in this.branches)
					{
						n += branch.DeepBranchCount;
					}
					return n;
				}
				else
				{

					return 0;
				}
			}
		}
		 */
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
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion
	}
}
