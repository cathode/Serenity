/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
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
    public class ResourceGraphNode : KeyedCollection<string, ResourceGraphNode>
    {
        #region Fields
        /// <summary>
        /// Backing field for the <see cref="ResourceGraphNode.Resource"/> property.
        /// </summary>
        private Resource resource;

        /// <summary>
        /// Backing field for the <see cref="ResourceGraphNode.Name"/> property.
        /// </summary>
        private string name;

        /// <summary>
        /// Backing field for the <see cref="ResourceGraphNode.IsReverseAttached"/> property.
        /// </summary>
        private bool invisible;
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
                return this.Items.Count > 0;
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

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Resource"/>
        /// that is referenced by this graph node is made aware of the reference.
        /// </summary>
        public bool IsReverseAttached
        {
            get
            {
                return this.invisible;
            }
            set
            {
                this.invisible = value;
                // Detach, then re-attach the resource to refresh it's reverse association.
                this.AttachResource(this.Resource);
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

                if (this.IsReverseAttached)
                    this.resource.MountPointsMutable.Add(this);
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
                this.resource.MountPointsMutable.Remove(this);

            this.resource = null;
        }
        [Pure]
        protected override string GetKeyForItem(ResourceGraphNode item)
        {
            return item.Name;
        }
        #endregion
    }
}
