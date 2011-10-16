/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Serenity.Web;

namespace Serenity
{
    /// <summary>
    /// Represents a web application.
    /// </summary>
    public abstract class WebApplication
    {
        #region Fields
        private ResourceGraphNode applicationRoot;
        #endregion
        #region Constructors
        protected WebApplication()
        {
            this.applicationRoot = new ResourceGraphNode(this.GetType().Name);
        }
        #endregion
        #region Methods
        public abstract void InitializeResources();
        public abstract void ProcessRequest(Request request, Response response);
        #endregion
        #region Properties
        public ResourceGraphNode ApplicationRoot
        {
            get
            {
                return this.applicationRoot;
            }
        }
        /// <summary>
        /// Gets the name of the current <see cref="WebApplication"/>.
        /// </summary>
        public string Name
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the unique identifier of the current <see cref="WebApplication"/>.
        /// </summary>
        public Guid UniqueID
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the <see cref="Version"/> of the current <see cref="WebApplication"/>.
        /// </summary>
        public Version Version
        {
            get;
            protected set;
        }

        public string DefaultBinding
        {
            get;
            protected set;
        }
        #endregion


    }
}
