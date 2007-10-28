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
        internal ResourceTreeBranch(ResourceTree tree, ResourceTreeBranch parent, string name)
        {
            this.tree = tree;
            this.parent = parent;
            this.name = name;

            if (this.HasParent)
            {
                this.directoryResource = new DirectoryResource(this.Path);
            }
            else
            {
                this.directoryResource = new DirectoryResource("/");
            }
        }
        #endregion
        #region Fields - Private
        private string name;
        private ResourceTreeBranch parent;
        private ResourceTreeBranchCollection branches = new ResourceTreeBranchCollection();
        private ResourceTree tree;
        private ResourceCollection resources = new ResourceCollection();
        private DirectoryResource directoryResource;
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
        public ResourceTreeBranch GetBranch(string name)
        {
            return this.branches[name];
        }
        public Resource GetResource(string name)
        {
            return this.resources[name];
        }
        #endregion
        #region Properties - Public
        public ResourceTreeBranchCollection Branches
        {
            get
            {
                return this.branches;
            }
        }

        /// <summary>
        /// Gets the number resources located at the current branch.
        /// </summary>
        public int Count
        {
            get
            {
                return this.resources.Count;
            }
        }
        public DirectoryResource DirectoryResource
        {
            get
            {
                return this.directoryResource;
            }
            set
            {
                this.directoryResource = value;
            }
        }
        public bool HasParent
        {
            get
            {
                if (this.Parent == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
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
        public ResourceCollection Resources
        {
            get
            {
                return this.resources;
            }
        }
        #endregion
    }
}
