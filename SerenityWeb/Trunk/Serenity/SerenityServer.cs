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
            this.contextHandler = new ContextHandler(this);
            this.driverPool = new DriverPool(this);
            this.OperationLog.Write("Server created.", LogMessageLevel.Debug);
        }
        #endregion
        #region Fields - Private
        private Log accessLog = new Log();
        private readonly ResourceTree commonResources = new ResourceTree();
        private ContextHandler contextHandler;
        private readonly DomainCollection domains = new DomainCollection();
        private readonly DriverPool driverPool;
        private Log errorLog = new Log();
        private bool isCaseSensitive = false;
        private Log operationLog = new Log();
        private StringComparison stringComparison = StringComparison.Ordinal;
		#endregion
        #region Methods - Public
        public void ExtractResources(Domain domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }

            this.domains.Add(domain);
        }
        public void ExtractResources(Module module)
        {
            if (module == null)
            {
                throw new ArgumentNullException("module");
            }
        }
        public void ExtractResources(IEnumerable<Domain> domains)
        {
            foreach (Domain domain in domains)
            {
                this.ExtractResources(domain);
            }
        }
        public void ExtractResources(IEnumerable<Module> modules)
        {
            foreach (Module module in modules)
            {
                this.ExtractResources(module);
            }
        }
        #endregion
        #region Properties - Public
        public Log AccessLog
        {
            get
            {
                return this.accessLog;
            }
        }
        public Log ErrorLog
        {
            get
            {
                return this.errorLog;
            }
        }
        public Log OperationLog
        {
            get
            {
                return this.operationLog;
            }
        }
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