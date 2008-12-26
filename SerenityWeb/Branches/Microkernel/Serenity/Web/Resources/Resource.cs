/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *----------------------------------------------------------------------------*
 * Authors:                                                                   *
 * - Will 'AnarkiNet' Shelley (AnarkiNet@gmail.com): Original Author          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Serenity.Net;
using Serenity.Properties;

namespace Serenity.Web.Resources
{
    /// <summary>
    /// Provides a base class that all web-accessible resources must inherit from.
    /// </summary>
    public abstract class Resource
    {
        #region Fields
        private readonly List<Resource> children = new List<Resource>();
        private MimeType mimeType = MimeType.Default;
        private string name;
        private Server owner;
        private Resource parent;
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
        public virtual Resource GetChild(Uri uri)
        {
            if (uri.Segments.Length <= this.Position + 1)
            {
                return null;
            }
            return this.GetChild(uri.Segments[this.Position + 1].TrimEnd('/'));
        }
        public void Add(Resource resource)
        {
            if (!this.CanHaveChildren)
            {
                throw new InvalidOperationException(AppResources.ResourceDoesNotSupportChildrenException);
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
                throw new InvalidOperationException(AppResources.ResourceDoesNotSupportChildrenException);
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
        }
        /// <summary>
        /// Invoked before OnRequest.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public virtual void PreRequest(Request request, Response response)
        {
        }
        public Uri GetAbsoluteUri(Uri baseUri)
        {
            return new Uri(new Uri(baseUri.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped)), this.RelativeUri);
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets a value that indicates if the current <see cref="Resource"/>
        /// supports child resource associations.
        /// </summary>
        public virtual bool CanHaveChildren
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// Gets the child resources of the current <see cref="Resource"/>.
        /// </summary>
        public Resource[] Children
        {
            get
            {
                return this.children.ToArray();
            }
        }
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
        /// Gets a value that indicates if the current <see cref="Resource"/>
        /// contains any children.
        /// </summary>
        public bool HasChildren
        {
            get
            {
                return this.CanHaveChildren && this.children.Count > 0;
            }
        }
        /// <summary>
        /// Gets a value that indicates if a parent is defined for the current
        /// <see cref="Resource"/>.
        /// </summary>
        public bool HasParent
        {
            get
            {
                return (this.Parent != null);
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
                return this.name ?? string.Empty;
            }
            set
            {
                this.name = value;
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="Server"/> that is the owner of the
        /// current <see cref="Resource"/>.
        /// </summary>
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
        /// <summary>
        /// Gets the <see cref="Resource"/> that is the parent of the current
        /// <see cref="Resource"/>.
        /// </summary>
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
        /// Gets the relative <see cref="Uri"/> of the current
        /// <see cref="Resource"/> by traversing the resource tree in reverse.
        /// </summary>
        public Uri RelativeUri
        {
            get
            {
                if (this.Parent == null)
                {
                    return new Uri(this.Name, UriKind.Relative);
                }
                else
                {
                    return new Uri(this.Parent.RelativeUri.ToString() + this.SegmentName, UriKind.Relative);
                }
            }
        }
        /// <summary>
        /// Gets the name of the current <see cref="Resource"/> for use in
        /// the construction of a URI.
        /// </summary>
        public string SegmentName
        {
            get
            {
                return (this.HasChildren) ? ((!this.Name.EndsWith("/")) ? this.Name + "/" : this.Name) : this.Name;
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
        public int Position
        {
            get
            {
                if (!this.HasParent)
                {
                    return 0;
                }
                else
                {
                    return this.Parent.Position + 1;
                }
            }
        }
        #endregion
    }
}
