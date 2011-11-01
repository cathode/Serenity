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
using Serenity.Net;

namespace Serenity.Web
{
    /// <summary>
    /// Provides the basic functionality of a requestable resource.
    /// </summary>
    public abstract class Resource
    {
        #region Fields
        /// <summary>
        /// Holds a list of the nodes in the resource graph to which the
        /// current <see cref="Resource"/> is attached.
        /// </summary>
        private readonly List<ResourceGraphNode> locations;

        private readonly ReadOnlyCollection<ResourceGraphNode> locationsRO;

        /// <summary>
        /// Backing field for the <see cref="Resource.Created"/> property.
        /// </summary>
        private DateTime created;

        /// <summary>
        /// Backing field for the <see cref="Resource.Modified"/> property.
        /// </summary>
        private DateTime modified;

        /// <summary>
        /// Backing field for the <see cref="Resource.MimeType"/> property.
        /// </summary>
        private MimeType mimeType = MimeType.Default;

        /// <summary>
        /// Backing field for the <see cref="Resource.Name"/> property.
        /// </summary>
        private string name;

        /// <summary>
        /// Backing field for the <see cref="Resource.UniqueID"/> property.
        /// </summary>
        private Guid uniqueID;

        /// <summary>
        /// Backing field for the <see cref="Resource.Grouping"/> property.
        /// </summary>
        private ResourceGrouping grouping;
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Resource"/> class.
        /// </summary>
        protected Resource()
        {
            this.locations = new List<ResourceGraphNode>();
            this.locationsRO = new ReadOnlyCollection<ResourceGraphNode>(this.locations);
            this.Created = DateTime.Now;
            this.Modified = this.Created;
            this.uniqueID = Guid.NewGuid();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the <see cref="MimeType"/> of the content produced by
        /// this resource.
        /// </summary>
        public MimeType ContentType
        {
            get
            {
                return this.mimeType;
            }
            set
            {
                this.mimeType = value;
            }
        }

        /// <summary>
        /// Gets a value that indicates if the size in bytes of the current
        /// <see cref="Resource"/> is known or can be determined.
        /// </summary>
        public virtual bool IsSizeKnown
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the name of the current <see cref="Resource"/>.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name ?? this.UniqueID.ToString();
            }
            set
            {
                Contract.Requires(ResourceGraph.IsValidName(value));

                this.name = value;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the size in bytes of the
        /// content of the current Resource.
        /// </summary>
        public virtual long Size
        {
            get
            {
                return -1;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> representing the last time
        /// the current <see cref="Resource"/> was modified.
        /// </summary>
        /// <remarks>
        /// The last modified timestamp is not automatically updated by the
        /// <see cref="Resource"/> class, it must be maintained by the deriving
        /// class.
        /// <note>The last modified time is set to <see cref="DateTime.Now"/>
        /// when the current <see cref="Resource"/> is created.</note>
        /// </remarks>
        public virtual DateTime Modified
        {
            get
            {
                return this.modified;
            }
            set
            {
                this.modified = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> representing when the
        /// current <see cref="Resource"/> was created.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public virtual DateTime Created
        {
            get
            {
                return this.created;
            }
            set
            {
                this.created = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Guid"/> of the current resource instance.
        /// </summary>
        public Guid UniqueID
        {
            get
            {
                return this.uniqueID;
            }
            set
            {
                this.uniqueID = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ResourceGrouping"/> of the current <see cref="Resource"/>.
        /// </summary>
        public ResourceGrouping Grouping
        {
            get
            {
                Contract.Ensures(Contract.Result<ResourceGrouping>() != null);

                return this.grouping ?? ResourceGrouping.Unspecified;
            }
            set
            {
                this.grouping = value;
            }
        }

        /// <summary>
        /// Gets a read-only collection of the <see cref="ResourceGraphNode">resource graph nodes</see>
        /// that this resource is attached to.
        /// </summary>
        public ReadOnlyCollection<ResourceGraphNode> Locations
        {
            get
            {
                return this.locationsRO;
            }
        }

        internal List<ResourceGraphNode> LocationsMutable
        {
            get
            {
                return this.locations;
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Creates an absolute <see cref="Uri"/> using the scheme, host and
        /// port information in the base <see cref="Uri"/>, and the relative
        /// path information in the current <see cref="Resource"/>'s relative
        /// <see cref="Uri"/>.
        /// </summary>
        /// <param name="baseUri"></param>
        /// <returns></returns>
        public Uri GetAbsoluteUri(Uri baseUri)
        {
            return new Uri(baseUri, this.GetRelativeUri());
        }

        public Uri GetRelativeUri()
        {
            if (this.locations.Count > 0)
            {
                var mp = this.locations.FirstOrDefault(n => n.PreferDefault) ?? this.locations[0];
                return new Uri(mp.Path, UriKind.Relative);
            }
            else
                return new Uri("/" + this.Name, UriKind.Relative);
        }

        /// <summary>
        /// When overridden in a derived class, uses the supplied CommonContext to dynamically generate response content.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public virtual void OnRequest(Request request, Response response)
        {
            Contract.Requires(request != null);
            Contract.Requires(response != null);
        }
        #endregion
    }
}