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
        private readonly List<ResourceGraphNode> mountPoints;
        private DateTime created;
        private DateTime modified;
        private MimeType mimeType = MimeType.Default;
        private string name;
        private Guid uniqueID;
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Resource"/>.
        /// </summary>
        protected Resource()
        {
            this.mountPoints = new List<ResourceGraphNode>();
            this.Created = DateTime.Now;
            this.Modified = this.Created;
            this.uniqueID = Guid.NewGuid();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the <see cref="MimeType"/> used to describe the content of
        /// the current <see cref="Resource"/>.
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
                Contract.Assume(ResourceGraph.IsValidName(Contract.Result<string>()));

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

        public ReadOnlyCollection<ResourceGraphNode> MountPoints
        {
            get
            {
                return new ReadOnlyCollection<ResourceGraphNode>(this.mountPoints);
            }
        }

        internal List<ResourceGraphNode> MountPointsMutable
        {
            get
            {
                return this.mountPoints;
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overridden in a derived class, uses the supplied CommonContext to dynamically generate response content.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public virtual void OnRequest(Request request, Response response)
        {

        }
        #endregion

    }
}