/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using Serenity.Web;

namespace Serenity.Web.Resources
{
    /// <summary>
    /// Provides the base class for dynamic resources.
    /// </summary>
    public abstract class DynamicResource : Resource
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the DynamicResource class.
        /// </summary>
        protected DynamicResource()
        {
        }
        #endregion
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
        /// <summary>
        /// Overridden. Gets the grouping of the current <see cref="DynamicResource"/>,
        /// which is always <see cref="ResourceGrouping.Dynamic"/>.
        /// </summary>
        public override ResourceGrouping Grouping
        {
            get
            {
                return ResourceGrouping.Dynamic;
            }
        }
        /// <summary>
        /// Gets the Module which the current DynamicResource belongs to.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown when setting
        /// if the value to be assigned is null.</exception>
        public Module Module
        {
            get
            {
                return this.module;
            }
            internal set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.module = value;
            }
        }
        #endregion
    }
}
