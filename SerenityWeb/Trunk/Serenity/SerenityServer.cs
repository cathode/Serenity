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
	public static class SerenityServer
    {
        #region Constructors - Private
        static SerenityServer()
        {
            SerenityServer.contextHandler = new ContextHandler();
            SerenityServer.driverPool = new DriverPool();
            SerenityServer.OperationLog.Write("Server created.", LogMessageLevel.Debug);
        }
        #endregion
        #region Fields - Private
        private static Log accessLog = new Log();
        private static readonly ResourceTree commonResources = new ResourceTree();
        private static ContextHandler contextHandler;
        private static readonly DomainCollection domains = new DomainCollection();
        private static readonly DriverPool driverPool;
        private static Log errorLog = new Log();
        private static bool isCaseSensitive = false;
        private static ModuleCollection modules = new ModuleCollection();
        private static Log operationLog = new Log();
        private static OperationStatus status = OperationStatus.None;
        private static StringComparison stringComparison = StringComparison.Ordinal;
		#endregion
        #region Methods - Public
        public static void ExtractResources(Domain domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }

            SerenityServer.domains.Add(domain);
        }
        public static void ExtractResources(Module module)
        {
            if (module == null)
            {
                throw new ArgumentNullException("module");
            }
            if (SerenityServer.modules.Contains(module.Name))
            {
                return;
            }
            foreach (Page page in module.Pages)
            {
                SerenityServer.commonResources.Add("/dynamic/" + module.Name + "/", page);
            }
        }
        public static void ExtractResources(IEnumerable<Domain> domains)
        {
            foreach (Domain domain in domains)
            {
                SerenityServer.ExtractResources(domain);
            }
        }
        public static void ExtractResources(IEnumerable<Module> modules)
        {
            foreach (Module module in modules)
            {
                SerenityServer.ExtractResources(module);
            }
        }
        #endregion
        #region Properties - Public
        public static Log AccessLog
        {
            get
            {
                return SerenityServer.accessLog;
            }
        }
        public static Log ErrorLog
        {
            get
            {
                return SerenityServer.errorLog;
            }
        }
        public static Log OperationLog
        {
            get
            {
                return SerenityServer.operationLog;
            }
        }
        public static ResourceTree CommonResources
        {
            get
            {
                return SerenityServer.commonResources;
            }
        }
        /// <summary>
        /// Gets or sets the ContextHandler used to handle recieved CommonContexts.
        /// </summary>
        public static ContextHandler ContextHandler
		{
			get
			{
                return SerenityServer.contextHandler;
			}
			set
			{
                SerenityServer.contextHandler = value;
			}
		}
        /// <summary>
        /// Gets the DriverPool containing WebDrivers used by the current SerenityServer.
        /// </summary>
        public static DriverPool DriverPool
		{
			get
			{
                return SerenityServer.driverPool;
			}
		}
        public static DomainCollection Domains
        {
            get
            {
                return SerenityServer.domains;
            }
        }
        public static bool IsCaseSensitive
        {
            get
            {
                return SerenityServer.isCaseSensitive;
            }
            set
            {
                SerenityServer.isCaseSensitive = value;
                if (SerenityServer.isCaseSensitive)
                {
                    SerenityServer.stringComparison = StringComparison.Ordinal;
                }
                else
                {
                    SerenityServer.stringComparison = StringComparison.OrdinalIgnoreCase;
                }
            }
        }
        public static StringComparison StringComparison
        {
            get
            {
                return SerenityServer.stringComparison;
            }
        }
        public static OperationStatus Status
        {
            get
            {
                return SerenityServer.status;
            }
            set
            {
                SerenityServer.status = value;
            }
        }
		#endregion
	}
}