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

using Serenity.Collections;

namespace Serenity
{
    public class ResourceNode
    {
        #region Constructors - Internal
        internal ResourceNode(string name) : this(name, null)
        {
        }
        internal ResourceNode(string name, ResourceNode parent)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            this.name = name;
            this.parent = parent;

            this.directoryResource = new DirectoryResource(this);
        }
        #endregion
        #region Fields - Private
        private DirectoryResource directoryResource;
        private string name;
        private ResourceNode parent;
        private ResourceCollection resources = new ResourceCollection();
        private ResourceNodeCollection nodes = new ResourceNodeCollection();
        #endregion
        #region Methods - Public
        public void AddNode(ResourceNode node)
        {
            this.nodes.Add(node);
        }
        public void Add(Resource resource)
        {
            this.resources.Add(resource);
        }
        public bool ContainsNode(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else if (name == string.Empty)
            {
                throw new ArgumentException("Argument 'name' cannot be empty.", "name");
            }

            return this.nodes.Contains(name);
        }
        public virtual ResourceNode GetNode(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else if (name == string.Empty)
            {
                throw new ArgumentException("Argument 'name' cannot be empty.", "name");
            }
            else if (!this.nodes.Contains(name))
            {
                throw new KeyNotFoundException("No node with the specified name exists as a child of the current ResourceNode");
            }

            return this.nodes[name];
        }
        public ResourceNode GetNode(ResourcePath path)
        {
            ResourceNode node = this;

            foreach (string segment in path)
            {
                if (node.ContainsNode(segment))
                {
                    node = node.GetNode(segment);
                }
                else
                {
                    throw new KeyNotFoundException("No node found at 'path'.");
                }
            }
            return node;
        }
        public virtual Resource GetResource(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else if (name == string.Empty)
            {
                throw new ArgumentException("Argument 'name' cannot be empty.", "name");
            }
            else if (!this.resources.Contains(name))
            {
                throw new KeyNotFoundException("No resource with the specified name exists as a child of the current ResourceNode");
            }
            return this.resources[name];
        }
        #endregion
        #region Properties - Public
        public DirectoryResource DirectoryResource
        {
            get
            {
                return this.directoryResource;
            }
        }
        public bool HasParent
        {
            get
            {
                return (this.parent != null);
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
                foreach (ResourceNode node in this.nodes)
                {
                    yield return node;
                }
            }
        }
        public ResourcePath Path
        {
            get
            {
                if (this.HasParent)
                {
                    return this.parent.Path + this.name;
                }
                else
                {
                    return new ResourcePath(this.name);
                }
            }
        }
        public ResourceNode Parent
        {
            get
            {
                if (this.HasParent)
                {
                    return this.parent;
                }
                else
                {
                    throw new InvalidOperationException("The current ResourceNode does not contain a defined parent node.");
                }
            }
        }
        public IEnumerable<Resource> Resources
        {
            get
            {
                foreach (Resource resource in this.resources)
                {
                    yield return resource;
                }
            }
        }
        #endregion
    }
}
