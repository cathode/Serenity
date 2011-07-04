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
        #region Methods
        public abstract void InitializeResources();
        public abstract void ProcessRequest(Request request, Response response);
        #endregion
        #region Properties
        /// <summary>
        /// Gets the name of the current <see cref="WebApplication"/>.
        /// </summary>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// Gets the unique identifier of the current <see cref="WebApplication"/>.
        /// </summary>
        public abstract Guid UniqueID
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Version"/> of the current <see cref="WebApplication"/>.
        /// </summary>
        public abstract Version Version
        {
            get;
        }

        public abstract string DefaultBinding
        {
            get;
        }
        #endregion

       
    }
}
