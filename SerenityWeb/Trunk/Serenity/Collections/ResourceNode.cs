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
    public sealed class ResourceNode
    {
        #region Constructors - Internal
        internal ResourceNode(ResourceTree tree, ResourceNode parent, string name)
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
        private ResourceNode parent;
        private ResourceNodeCollection nodes = new ResourceNodeCollection(false);
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
        public void Add(ResourceNode value)
        {
            this.nodes.Add(value);
        }
        public bool ContainsNode(string name)
        {
            return this.nodes.Contains(name);
        }
        public bool ContainsResource(string name)
        {
            return this.resources.Contains(name);
        }
        public ResourceNode GetNode(string name)
        {
            return this.nodes[name];
        }
        public Resource GetResource(string name)
        {
            return this.resources[name];
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets the number resources located at the current node.
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
        public IEnumerable<ResourceNode> Nodes
        {
            get
            {
                return this.nodes;
            }
        }
        public ResourceNode Parent
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
                if (this.HasParent)
                {
                    return parent.Path + this.name + "/";
                }
                else if (!string.IsNullOrEmpty(this.name))
                {
                    return "/" + this.name + "/";
                }
                else
                {
                    return "/";
                }
            }
        }
        public IEnumerable<Resource> Resources
        {
            get
            {
                return this.resources;
            }
        }
        #endregion
    }
}
