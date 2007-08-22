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

namespace Serenity.Collections.Generic
{
	public abstract class ResourceTree<T> : ResourceTreeBranch<T> where T : Resource
	{
		public ResourceTree()
		{

		}
	}
	public class ResourceTreeBranch<T> : ResourceTreeNode<T> where T : Resource
	{
		public ResourceTreeBranch()
		{

		}
		#region Fields - Private
		private List<ResourceTreeBranch<T>> branches = new List<ResourceTreeBranch<T>>();
		private List<ResourceTreeNode<T>> nodes = new List<ResourceTreeNode<T>>();
		#endregion
		#region Properties - Public
		public int BranchCount
		{
			get
			{
				return this.branches.Count;
			}
		}
		public int NodeCount
		{
			get
			{
				return this.nodes.Count;
			}
		}
		public int DeepBranchCount
		{
			get
			{
				if (this.BranchCount > 0)
				{
					int n = this.BranchCount;
					foreach (ResourceTreeBranch<T> branch in this.branches)
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
		public int DeepNodeCount
		{
			get
			{
				int n = this.NodeCount;
				foreach (ResourceTreeBranch<T> branch in this.branches)
				{
					n += branch.DeepNodeCount;
				}

				return n;
			}
		}
		#endregion
	}
	public class ResourceTreeNode<T> where T : Resource
	{
		public ResourceTreeNode()
		{

		}
		public ResourceTreeNode(T value)
		{
			this.value = value;
		}
		private T value;

		public T Value
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
	}
}
