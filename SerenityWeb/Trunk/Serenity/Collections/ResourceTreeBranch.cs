/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
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
    public sealed class ResourceTreeBranch
    {
        #region Constructors - Private
        /// <summary>
        /// Initializes a new instance of the ResourceTreeElement class.
        /// </summary>
        internal ResourceTreeBranch(ResourceTree tree, string name)
        {
            this.tree = tree;
            this.name = name;
        }
        #endregion
        #region Fields - Private
        private string name;
        private ResourceTreeBranch parent = null;
        private List<ResourceTreeBranch> branches = new List<ResourceTreeBranch>();
        private ResourceTree tree;
        private ResourceCollection resources = new ResourceCollection();
        #endregion
        #region Methods - Public
        public void Add(Resource value)
        {
            if (!this.resources.Contains(value.SystemName))
            {
                this.resources.Add(value);
            }
        }
        public void Add(ResourceTreeBranch value)
        {
            this.branches.Add(value);
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
                return this.resources.Count;
            }
        }
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        public ResourceTreeBranch Parent
        {
            get
            {
                return this.parent;
            }
            internal set
            {
                this.parent = value;
            }
        }
        public string Path
        {
            get
            {
                return string.Empty;
            }
        }
        #endregion
    }
}
