/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright � 2006-2007 Serenity Project - http://SerenityProject.net/       *
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
            this.contextHandler = new ContextHandler(this);
            this.driverPool = new DriverPool(this);
        }
        #endregion
        #region Fields - Private
        private ContextHandler contextHandler;
        private readonly DriverPool driverPool;
        private readonly DomainCollection domains = new DomainCollection();
        private readonly ResourceTree commonResources = new ResourceTree();
        private bool isCaseSensitive = false;
        private StringComparison stringComparison = StringComparison.Ordinal;
		#endregion
        #region Methods - Private
        private void ExtractResources(Domain domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }

            this.domains.Add(domain);
        }
        private void ExtractResources(Module module)
        {
            if (module == null)
            {
                throw new ArgumentNullException("module");
            }

        }
        private void ExtractResources(IEnumerable<Domain> domains)
        {
            foreach (Domain domain in domains)
            {
                this.ExtractResources(domain);
            }
        }
        private void ExtractResources(IEnumerable<Module> modules)
        {
            foreach (Module module in modules)
            {
                this.ExtractResources(module);
            }
        }
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
        public bool IsCaseSensitive
        {
            get
            {
                return this.isCaseSensitive;
            }
            set
            {
                this.isCaseSensitive = value;
                if (this.isCaseSensitive)
                {
                    this.stringComparison = StringComparison.Ordinal;
                }
                else
                {
                    this.stringComparison = StringComparison.OrdinalIgnoreCase;
                }
            }
        }
        public StringComparison StringComparison
        {
            get
            {
                return this.stringComparison;
            }
        }
		#endregion
	}
}