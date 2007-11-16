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

namespace Serenity.Resources
{
    /// <summary>
    /// Represents a node in a tree of resources.
    /// </summary>
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
        /// <summary>
        /// Adds a node as a child of the current node.
        /// </summary>
        /// <param name="node"></param>
        public void AddNode(ResourceNode node)
        {
            this.nodes.Add(node);
            node.parent = this;
        }
        /// <summary>
        /// Adds a resource to the current node.
        /// </summary>
        /// <param name="resource"></param>
        public void Add(Resource resource)
        {
            this.resources.Add(resource);
        }
        /// <summary>
        /// Determines if a named node exists as a child of the current node.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets a named child node.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets a named resource.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets a DirectoryResource object that can be used to generate an index of the current node.
        /// </summary>
        public DirectoryResource DirectoryResource
        {
            get
            {
                return this.directoryResource;
            }
        }
        /// <summary>
        /// Gets a boolean value that indicates if the current node has a parent.
        /// </summary>
        public bool HasParent
        {
            get
            {
                return (this.parent != null);
            }
        }
        /// <summary>
        /// Gets the name of the current node.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        /// <summary>
        /// Gets an object which allows enumeration over the child nodes of the current node.
        /// </summary>
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
        /// <summary>
        /// Gets the path of the current node.
        /// </summary>
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
        /// <summary>
        /// Gets the parent node of the current node.
        /// </summary>
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
        /// <summary>
        /// Gets an object that allows enumeration over the resources located at the current node.
        /// </summary>
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
