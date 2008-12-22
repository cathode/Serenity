/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Serenity.Properties;

using Serenity.Web;
using Serenity.Net;

namespace Serenity.Web.Resources
{
    /// <summary>
    /// Provides a base class that all web-accessible resources must inherit from.
    /// </summary>
    public abstract class Resource
    {
        #region Fields - Private
        private MimeType mimeType = MimeType.Default;
        private string name;
        private ResourcePath uri;
        private Server owner;
        private Resource parent;
        private readonly List<Resource> children = new List<Resource>();
        #endregion
        #region Methods
        public Resource GetChild(string name)
        {
            if (!this.CanHaveChildren)
            {
                return null;
            }
            return (from res in this.Children
                    where res.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                    select res).FirstOrDefault();
        }
        public void Add(Resource resource)
        {
            if (!this.CanHaveChildren)
            {
                throw new InvalidOperationException(AppResources.ResourceDoesNotSupportChildren);
            }
            else if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            else if (resource.Parent != null)
            {
                throw new InvalidOperationException(AppResources.ResourceHasParentException);
            }
            this.children.Add(resource);
            resource.Parent = this;
        }
        public void Add(IEnumerable<Resource> resources)
        {
            foreach (Resource resource in resources)
            {
                this.Add(resource);
            }
        }
        public void Remove(Resource resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            //Checking the parent should be faster than searching children
            else if (resource.Parent != this) 
            {
                //If the resource is not a child, ignore.
                return;
            }
            else if (!this.CanHaveChildren)
            {
                //TODO: Evaluate the appropriateness of throwing an exception
                //      when an attempt is made to remove a resource from a
                //      resource that does not support children.
                throw new InvalidOperationException(AppResources.ResourceDoesNotSupportChildren);
            }

            this.children.Remove(resource);
            resource.Parent = null;
        }
        public void Remove(IEnumerable<Resource> resources)
        {
            foreach (Resource resource in resources)
            {
                this.Remove(resource);
            }
        }
        /// <summary>
        /// Traverses the resource tree in reverse to determine the relative
        /// <see cref="Uri"/> of the current <see cref="Resource"/>.
        /// </summary>
        /// <returns></returns>
        public Uri GetRelativeUri()
        {
            if (this.Parent == null)
            {
                return new Uri(this.SegmentName, UriKind.Relative);
            }
            else
            {
                return new Uri(this.Parent.GetRelativeUri().ToString() + this.SegmentName, UriKind.Relative);
            }
        }
        /// <summary>
        /// When overridden in a derived class, uses the supplied CommonContext to dynamically generate response content.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public virtual void OnRequest(Request request, Response response)
        {
        }
        /// <summary>
        /// Invoked after OnRequest.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public virtual void PostRequest(Request request, Response response)
        {
            if (response.ContentType != this.ContentType)
            {
                response.ContentType = this.ContentType;
            }
            response.Status = StatusCode.Http200Ok;
        }
        /// <summary>
        /// Invoked before OnRequest.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public virtual void PreRequest(Request request, Response response)
        {
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets the grouping of the current Resource.
        /// </summary>
        public virtual ResourceGrouping Grouping
        {
            get
            {
                return ResourceGrouping.Unspecified;
            }
        }
        /// <summary>
        /// Gets a value that indicates if the size in bytes of the current
        /// Resource is known or can be determined.
        /// </summary>
        public virtual bool IsSizeKnown
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// Gets the MimeType that should be used to describe the content of the current Resource.
        /// </summary>
        public MimeType ContentType
        {
            get
            {
                return this.mimeType;
            }
            protected internal set
            {
                this.mimeType = value;
            }
        }
        public bool HasParent
        {
            get
            {
                return (this.Parent != null);
            }
        }
        public bool HasChildren
        {
            get
            {
                return this.CanHaveChildren && this.children.Count > 0;
            }
        }
        public virtual bool CanHaveChildren
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// Gets or sets the name of the current Resource.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name ?? string.Empty;
            }
            set
            {
                this.name = value;
            }
        }
        public string SegmentName
        {
            get
            {
                return (this.HasChildren) ? ((!this.Name.EndsWith("/")) ? this.Name + "/" : this.Name) : this.Name;
            }
        }
        /// <summary>
        /// Gets the ResourcePath of the current Resource.
        /// </summary>
        public ResourcePath Path
        {
            get
            {
                return this.uri ?? ResourcePath.Create(this.Name);
            }
            set
            {
                this.uri = value;
            }
        }
        /// <summary>
        /// When overridden in a derived class, gets the size in bytes of the
        /// content of the current Resource.
        /// </summary>
        public virtual int Size
        {
            get
            {
                return -1;
            }
        }
        public Server Owner
        {
            get
            {
                return this.owner;
            }
            set
            {
                this.owner = value;
            }
        }
        public Resource Parent
        {
            get
            {
                return this.parent;
            }
            private set
            {
                this.parent = value;
            }
        }
        /// <summary>
        /// Gets the child resources of the current <see cref="TreeResource"/>.
        /// </summary>
        public Resource[] Children
        {
            get
            {
                return this.children.ToArray();
            }
        }
        #endregion
    }
}
