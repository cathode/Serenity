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

namespace Serenity.Resources
{
    /// <summary>
    /// Provides the base class for dynamic resources.
    /// </summary>
    public abstract class DynamicResource : Resource
    {
        #region Fields - Private
        private Module module;
        #endregion
        #region Methods - Public
        /// <summary>
        /// When overridden in a derived class, performs actions when the module is loaded into the server,
        /// usually when the server first starts up.
        /// </summary>
        public virtual void OnInitialization()
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
        public override ResourceGrouping Grouping
        {
            get
            {
                return ResourceGrouping.Dynamic;
            }
        }
        public bool IsOwned
        {
            get
            {
                return (this.module == null) ? false : true;
            }
        }
        public Module Modules
        {
            get
            {
                return this.module;
            }
            internal set
            {
                this.module = value;
            }
        }
        #endregion
    }
}
