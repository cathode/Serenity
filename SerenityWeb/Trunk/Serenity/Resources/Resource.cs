/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright � 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using Serenity.Web;

namespace Serenity.Resources
{
	/// <summary>
	/// Provides a base class that all web-accessible resources must inherit from.
	/// </summary>
	public abstract class Resource
    {
        #region Fields - Private
        private MimeType mimeType = MimeType.Default;
        private string name = string.Empty;
        private ResourcePath uri;
        #endregion
        #region Methods - Public
        /// <summary>
        /// When overridden in a derived class, uses the supplied CommonContext to dynamically generate response content.
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnRequest(CommonContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            ErrorHandler.Handle(context, StatusCode.Http501NotImplemented);
        }
        /// <summary>
        /// Invoked after OnRequest.
        /// </summary>
        /// <param name="context"></param>
        public virtual void PostRequest(CommonContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            else if (context.Response.ContentType != this.ContentType)
            {
                context.Response.ContentType = this.ContentType;
            }
            context.Response.Status = StatusCode.Http200Ok;
        }
        /// <summary>
        /// Invoked before OnRequest.
        /// </summary>
        /// <param name="context"></param>
        public virtual void PreRequest(CommonContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
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
		public virtual MimeType ContentType
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
		/// <summary>
		/// Gets or sets the name of the current Resource.
		/// </summary>
		public virtual string Name
		{
			get
			{
				return this.name;
			}
            protected internal set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.name = value;
            }
		}
        public ResourcePath Path
        {
            get
            {
                return this.uri;
            }
            internal set
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
		#endregion
	}
}