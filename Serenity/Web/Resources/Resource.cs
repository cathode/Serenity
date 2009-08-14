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
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Resource"/>.
        /// </summary>
        protected Resource()
        {
            this.Created = DateTime.Now;
            this.Modified = this.Created;
        }
        #endregion
        #region Fields
        private readonly List<Resource> children = new List<Resource>();
        private DateTime created;
        private DateTime modified;
        private MimeType mimeType = MimeType.Default;
        private string name;
        private Server owner;
        private Resource parent;
        #endregion
        #region Methods
        /// <summary>
        /// Adds a <see cref="Resource"/> as a child of the current
        /// <see cref="Resource"/>.
        /// </summary>
        /// <param name="resource">The child <see cref="Resource"/> to add.
        /// </param>
        /// <exception cref="System.ArgumentNullException">Thrown when
        /// <paramref name="resource"/> is passed as null.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when the
        /// current <see cref="Resource"/> does not support children.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">Thrown when
        /// <paramref name="resource"/> has a parent defined that is not the
        /// current <see cref="Resource"/>.
        /// </exception>
        /// <remarks>
        /// Adding a resource creates a strong bond between the parent and
        /// child; a resource cannot be added if that resource already has an
        /// existing parent defined, unless the parent is 
        /// </remarks>
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
            else if (resource == this)
            {
                throw new InvalidOperationException(AppResources.ResourceAddCreatesCircularRelationException);
            }
            else if (resource.Parent != null)
            {
                if (resource.Parent == this)
                {
                    return;
                }
                throw new InvalidOperationException(AppResources.ResourceHasParentException);
            }
            this.children.Add(resource);
            resource.Parent = this;
        }
        /// <summary>
        /// Adds a collection of <see cref="Resource"/>s to the current
        /// <see cref="Resource"/>.
        /// </summary>
        /// <param name="resources"></param>
        public void Add(IEnumerable<Resource> resources)
        {
            if (!this.CanHaveChildren)
                throw new InvalidOperationException(AppResources.ResourceDoesNotSupportChildrenException);
            
            List<Resource> filteredResources = new List<Resource>();

            foreach (var res in resources)
            {
                if (res == null)
                {
                    continue;
                }
                else if (res == this)
                {
                    throw new InvalidOperationException(AppResources.ResourceAddCreatesCircularRelationException);
                }
                else if (res.Parent != null)
                {
                    if (res.Parent == this)
                    {
                        //res is already a child of the current resource, skip.
                        continue;
                    }
                    throw new InvalidOperationException(AppResources.ResourceHasParentException);
                }
                filteredResources.Add(res);
            }
            foreach (Resource resource in filteredResources)
            {
                this.children.Add(resource);
                resource.Parent = this;
            }
        }
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
            return new Uri(new Uri(baseUri.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped)), this.RelativeUri);
        }
        /// <summary>
        /// Gets the child <see cref="Resource"/> with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Resource GetChild(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (!this.CanHaveChildren)
                return null;
            
            return (from res in this.Children
                    where res.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                    select res).FirstOrDefault();
        }
        /// <summary>
        /// Gets the child <see cref="Resource"/> which matches the
        /// specified <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public virtual Resource GetChild(Uri uri)
        {
            if (uri.Segments.Length <= this.Depth + 1)
                return null;
            
            return this.GetChild(uri.Segments[this.Depth + 1].TrimEnd('/'));
        }
        /// <summary>
        /// Follows the resource graph in reverse until the root resource is
        /// reached, in other words, the first resource encountered that has no
        /// parent.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// If the current <see cref="Resource"/> has no parent, then it is the
        /// root, and is returned.
        /// </remarks>
        public Resource GetRoot()
        {
            Resource res = this;

            while (res.HasParent)
                res = res.Parent;

            return res;
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
                response.ContentType = this.ContentType;
        }
        /// <summary>
        /// Invoked before OnRequest.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public virtual void PreRequest(Request request, Response response)
        {
        }
        /// <summary>
        /// Removes the specified <see cref="Resource"/> from the current <see cref="Resource"/>.
        /// </summary>
        /// <param name="resource">A <see cref="Resource"/> instance to remove from the current <see cref="Resource"/>.</param>
        /// <returns>true if <paramref name="resource"/> was removed, otherwise false.</returns>
        public bool Remove(Resource resource)
        {
            if (resource == null)
                throw new ArgumentNullException("resource");
            if (resource.Parent != this || !this.CanHaveChildren)
                return false;
            if (this.children.Remove(resource))
            {
                resource.Parent = null;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Removes the specified collection of <see cref="Resource"/>s from the current <see cref="Resource"/>.
        /// </summary>
        /// <param name="resources"></param>
        /// <remarks>
        /// Does not stop if a resource fails to be removed, nor does this method return any indication of success.
        /// </remarks>
        public void Remove(IEnumerable<Resource> resources)
        {
            foreach (Resource resource in resources)
                this.Remove(resource);
        }
        #endregion
        #region Properties
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
                if (this.HasParent)
                {
                    return new Uri(this.Parent.RelativeUri.ToString() + this.SegmentName, UriKind.Relative);
                }
                else
                {
                    return new Uri(this.SegmentName, UriKind.Relative);
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
        public virtual long Size
        {
            get
            {
                return -1;
            }
        }
        /// <summary>
        /// Gets the depth of the current <see cref="Resource"/> in relation to
        /// its ancestry. The depth is the length of the path to the root node.
        /// </summary>
        /// <remarks>
        /// If the current <see cref="Resource"/> has no parent, it's depth is
        /// 0.
        /// <note>This property is computed on the fly based on the state of
        /// the resource graph at the time the property value is requested.
        /// </note>
        /// </remarks>
        public int Depth
        {
            get
            {
                if (this.HasParent)
                    return this.Parent.Depth + 1;
                else
                    return 0;
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
        #endregion
    }
}
