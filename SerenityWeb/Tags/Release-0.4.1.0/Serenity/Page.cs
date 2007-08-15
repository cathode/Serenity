/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
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
