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
using System.IO;
using System.Text;

using Serenity.Themes;
using Serenity.Web;

namespace Serenity.Web
{
    /// <summary>
    /// Provides a handler for incoming CommonContexts by directing them to the proper resource class.
    /// </summary>
    public class ContextHandler
    {
        protected internal ContextHandler(SerenityServer server)
        {
            this.server = server;
        }
        #region Fields - Private
        private SerenityServer server;
        #endregion
        #region Methods - Public
        /// <summary>
        /// Handles an incoming CommonContext.
        /// </summary>
        /// <param name="context">The incoming CommonContext to be handled.</param>
        public virtual void HandleContext(CommonContext context)
        {
           
        }
        #endregion
        #region Properties - Public
        public SerenityServer Server
        {
            get
            {
                return this.server;
            }
        }
        #endregion
    }
}