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

using Serenity.Collections;
using Serenity.Web;
using Serenity.Web.Drivers;

namespace Serenity
{
    /// <summary>
    /// Provides a structured representation of a running Serenity server instance.
    /// </summary>
	public sealed class SerenityServer
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the SerenityServer class.
        /// </summary>
        public SerenityServer()
        {
        }
        #endregion
        #region Fields - Private
        private ContextHandler contextHandler = new ContextHandler();
        private DriverPool driverPool = new DriverPool();
        private DomainCollection domains = new DomainCollection();
        private ResourceTree commonResources = new ResourceTree();
		#endregion
        #region Properties - Public
        public ResourceTree CommonResources
        {
            get
            {
                return this.commonResources;
            }
        }
        /// <summary>
        /// Gets or sets the ContextHandler used to handle recieved CommonContexts.
        /// </summary>
        public ContextHandler ContextHandler
		{
			get
			{
				return this.contextHandler;
			}
			set
			{
				this.contextHandler = value;
			}
		}
        /// <summary>
        /// Gets the DriverPool containing WebDrivers used by the current SerenityServer.
        /// </summary>
		public DriverPool DriverPool
		{
			get
			{
				return this.driverPool;
			}
		}
        public DomainCollection Domains
        {
            get
            {
                return this.domains;
            }
        }

		#endregion
	}
}