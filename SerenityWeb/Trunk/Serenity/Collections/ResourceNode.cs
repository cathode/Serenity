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
    public class ResourceNode
    {
        #region Constructors - Internal
        internal ResourceNode(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else if (name == string.Empty)
            {
                throw new ArgumentException("Argument 'name' cannot be empty.", "name");
            }
            this.name = name;
        }
        #endregion
        #region Fields - Private
        private string name;
        private ResourceNode parent;
        private ResourceNodeCollection nodes = new ResourceNodeCollection();
        private ResourceCollection resources = new ResourceCollection();
        private DirectoryResource directoryResource;
        #endregion
        #region Methods - Internal
        internal void Add(ResourceNode node)
        {
            if (!this.nodes.Contains(node.Name))
            {
                this.nodes.Add(node);
            }
        }
        #endregion
        #region Methods - Public
        public void Add(Resource value)
        {
            if (!this.resources.Contains(value.Name))
            {
                this.resources.Add(value);
            }
        }
        public bool ContainsNode(string path)
        {
            path = ResourceTree.SanitizePath(path);

            return this.nodes.Contains(path);
        }
        public bool ContainsResource(string path)
        {
            path = ResourceTree.SanitizePath(path);

            if (path.EndsWith("/") && this.nodes.Contains(path))
            {
                return true;
            }
            string container = System.IO.Path.GetDirectoryName(path);
            if (this.nodes.Contains(container))
            {
                return false;
            }
            else
            {
                return false;
            }
        }
        public ResourceNode GetNode(string path)
        {
            path = ResourceTree.SanitizePath(path);

            if (this.nodes.Contains(path))
            {
                return this.nodes[path];
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }
        public Resource GetResource(string path)
        {
            path = ResourceTree.SanitizePath(path);

            return null;
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
                return !(this.parent == null);
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
        public ResourceNode Parent
        {
            get
            {
                return this.parent;
            }
        }
        public string Path
        {
            get
            {
                return (this.HasParent) ? this.parent.Path + "/" + this.Name + "/" : "/" + this.name + "/";
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
