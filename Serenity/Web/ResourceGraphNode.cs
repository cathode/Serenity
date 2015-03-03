/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2015 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Serenity.Web
{
    /// <summary>
    /// Represents a node in a resource graph.
    /// </summary>
    public sealed class ResourceGraphNode : IEnumerable<ResourceGraphNode>
    {
        #region Fields
        /// <summary>
        /// Holds child nodes of the current node.
        /// </summary>
        private readonly ResourceGraphNodeCollection children = new ResourceGraphNodeCollection();

        /// <summary>
        /// Backing field for the <see cref="ResourceGraphNode.Resource"/> property.
        /// </summary>
        private Resource resource;

        /// <summary>
        /// Backing field for the <see cref="ResourceGraphNode.Name"/> property.
        /// </summary>
        private string name;

        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceGraphNode"/> class.
        /// </summary>
        /// <param name="name">The name of the graph node.</param>
        public ResourceGraphNode(string name)
        {
            Contract.Requires(ResourceGraph.IsValidName(name));

            this.name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceGraphNode"/> class.
        /// </summary>
        /// <param name="resource">A <see cref="Resource"/> to attach to the new graph node.</param>
        public ResourceGraphNode(Resource resource)
        {
            Contract.Requires(resource != null);

            this.name = resource.Name;
            this.AttachResource(resource);
        }

        public ResourceGraphNode(string name, params ResourceGraphNode[] children)
        {
            Contract.Requires(ResourceGraph.IsValidName(name));

            this.name = name;

            foreach (var child in children)
                if (child != null)
                    this.AddChild(child);
        }
        public ResourceGraphNode(Resource resource, params ResourceGraphNode[] children)
        {
            Contract.Requires(resource != null);

            this.name = resource.Name;
            this.AttachResource(resource);

            foreach (var child in children)
                if (child != null)
                    this.AddChild(child);
        }
        internal ResourceGraphNode()
        {
            this.name = string.Empty;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the <see cref="ResourceGraphNode"/> that is the parent of the current node.
        /// </summary>
        public ResourceGraphNode Parent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the <see cref="Resource"/> that is referenced by the graph node.
        /// </summary>
        /// <remarks>
        /// To change the <see cref="Resource"/> that is attached, use the <see cref="AttachResource"/> method.
        /// </remarks>
        public Resource Resource
        {
            get
            {
                return this.resource;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current node has a parent node.
        /// </summary>
        public bool HasParent
        {
            get
            {
                return this.Parent != null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current node has child nodes.
        /// </summary>
        public bool HasChildren
        {
            get
            {
                return this.children.Count > 0;
            }
        }

        /// <summary>
        /// Gets or sets the name of the current node.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                Contract.Requires(ResourceGraph.IsValidName(value));

                this.name = value;
            }
        }

        public string SegmentName
        {
            get
            {
                return (this.HasChildren) ? this.Name + "/" : this.Name;
            }
        }

        public string Path
        {
            get
            {
                string path = this.SegmentName;
                if (this.HasParent)
                    path = this.Parent.Path + path;

                return path;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Resource"/>
        /// that is referenced by this node should prefer to use this node as the
        /// default mount point when determining its full path.
        /// </summary>
        /// <remarks>
        /// If a resource is referenced by more than one node marked to be a preferred
        /// default, the first preferred node is used.
        /// </remarks>
        public bool PreferDefault
        {
            get;
            set;
        }
        #endregion
        #region Methods
        public void AddChild(ResourceGraphNode node)
        {
            Contract.Requires(node != null);

            this.AddChild(node, false);
        }

        public void AddChild(ResourceGraphNode node, bool adopt)
        {
            Contract.Requires(node != null);

            this.children.Add(node);

            if (adopt || !node.HasParent)
                node.Parent = this;
        }

        public void Remove(ResourceGraphNode node)
        {
            Contract.Requires(node != null);

            if (this.children.Remove(node))
                if (node.Parent == this)
                    node.Parent = null;
        }

        public void Remove(string name)
        {
            if (this.children.Contains(name))
            {
                var node = this.children[name];
                this.children.Remove(node);
                if (node.Parent == this)
                    node.Parent = null;
            }
        }

        /// <summary>
        /// Creates an association between the specified resource and the current graph node.
        /// </summary>
        /// <param name="resource">A <see cref="Resource"/> to attach.</param>
        /// <remarks>
        /// Specifying a null resource is equivalent to calling <see cref="ResourceGraphNode.DetachResource"/>.
        /// </remarks>
        public void AttachResource(Resource resource)
        {
            this.DetachResource();

            if (resource != null)
            {
                this.resource = resource;
                this.resource.LocationsMutable.Add(this);
            }
        }

        /// <summary>
        /// Removes any existing association between an attachd resource and the current graph node.
        /// </summary>
        public void DetachResource()
        {
            Contract.Ensures(this.Resource == null);

            if (this.resource == null)
                return;
            else
                this.resource.LocationsMutable.Remove(this);

            this.resource = null;
        }
        public IEnumerator<ResourceGraphNode> GetEnumerator()
        {
            return this.children.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.children.GetEnumerator();
        }

        #endregion
        #region Types
        public sealed class ResourceGraphNodeCollection : KeyedCollection<string, ResourceGraphNode>
        {
            protected override string GetKeyForItem(ResourceGraphNode item)
            {
                return item.Name;
            }
        }
        #endregion
    }
}
