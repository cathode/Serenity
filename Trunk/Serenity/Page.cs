/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.Text;

using Serenity.Web;

namespace Serenity
{
    /// <summary>
    /// Provides the base class for dynamic resources.
    /// </summary>
    public abstract class Page
    {
        #region Methods - Public

        
        /// <summary>
        /// When overridden in a derived class, performs actions when the module is loaded into the server,
        /// usually when the server first starts up.
        /// </summary>
        public virtual void OnInitialization()
        {
        }
        /// <summary>
        /// When overridden in a derived class, uses the supplied CommonContext to dynamically generate
        /// a response which is sent back to the client.
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnRequest(CommonContext context)
        {
        }
        /// <summary>
        /// When overridden in a derived class, performs actions when the module is unloaded or the server
        /// is shut down.
        /// </summary>
        public virtual void OnShutdown()
        {
        }
        #endregion
        #region Properties - Public
        
        /// <summary>
        /// When overridden in a derived class, gets the name of the current Page.
        /// </summary>
        public abstract string Name
        {
            get;
        }
        #endregion
    }
}
