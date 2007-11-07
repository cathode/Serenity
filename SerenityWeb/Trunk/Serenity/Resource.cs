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

using Serenity.Web;

namespace Serenity
{
	/// <summary>
	/// Provides a base class that all web-accessible resources must inherit from.
	/// </summary>
	public abstract class Resource
    {
        #region Fields - Private
        private MimeType mimeType = MimeType.Default;
        private string name = string.Empty;
        private SerenityServer server;
        #endregion
        #region Methods - Public
        /// <summary>
        /// When overridden in a derived class, uses the supplied CommonContext to dynamically generate response content.
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnRequest(CommonContext context)
        {
            ErrorHandler.Handle(context, StatusCode.Http501NotImplemented);
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
		public virtual MimeType MimeType
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
                this.name = value;
            }
		}
        public SerenityServer Server
        {
            get
            {
                return this.server;
            }
            internal set
            {
                this.server = value;
            }
        }
        public virtual int Size
        {
            get
            {
                return -1;
            }
        }
		/// <summary>
		/// Gets the name of the resource in a form that can be "safely" used.
		/// </summary>
		public string SystemName
		{
			get
			{
				return this.Name.ToLower();
			}
		}
		#endregion
	}
}
