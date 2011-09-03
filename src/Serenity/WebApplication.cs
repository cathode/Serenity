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

        #endregion
        #region Constructors
        protected WebApplication()
        {
            this.Resources = new List<ResourceBinding>();
        }
        #endregion
        #region Methods
        /// <summary>
        /// Binds a resource using the resource's default binding.
        /// </summary>
        /// <param name="resource">The resource to bind.</param>
        protected internal void BindResource(Resource resource)
        {

        }

        /// <summary>
        /// Binds a resource using an explicit binding instead of the resource's own default binding.
        /// </summary>
        /// <param name="resource">The resource to bind.</param>
        /// <param name="path">The path to bind the resource to.</param>
        /// <param name="absolute">A bool indicating whether the specified path represents an absolute path (true), or a relative path (false).</param>
        protected internal void BindResource(Resource resource, string path, bool absolute)
        {
            if (!absolute)
                path = "/" + this.Name + "/" + path;

            this.Resources.Add(new ResourceBinding()
            {
                Resource = resource,
                IsAbsolutePath = absolute,
                Path = path,
            });
        }
        public abstract void InitializeResources();
        public abstract void ProcessRequest(Request request, Response response);
        #endregion
        #region Properties
        public List<ResourceBinding> Resources
        {
            get;
            private set;
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
