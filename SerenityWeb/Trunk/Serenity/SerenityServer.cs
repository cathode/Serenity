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
            FileTypeRegistry.Initialize();

            SerenityServer.domains.Add(SerenityServer.commonDomain);
            SerenityServer.OperationLog.Write("Server created.", LogMessageLevel.Debug);
        }
        #endregion
        #region Fields - Private
        private static Log accessLog = new Log();
        private static readonly Domain commonDomain = new Domain("");
        private static ContextHandler contextHandler = new ContextHandler();
        private static readonly DomainCollection domains = new DomainCollection();
        private static readonly DriverPool driverPool = new DriverPool();
        private static Log errorLog = new Log();
        private static ModuleCollection modules = new ModuleCollection();
        private static Log operationLog = new Log();
        private static OperationStatus status = OperationStatus.None;
        #endregion
        #region Methods - Public
        public static void AddDomain(Domain domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }

            SerenityServer.domains.Add(domain);
        }
        public static void AddModule(Module module)
        {
            if (module == null)
            {
                throw new ArgumentNullException("module");
            }
            else if (SerenityServer.Status == OperationStatus.Started)
            {
                throw new InvalidOperationException("Cannot modify server state while server is running.");
            }
            if (SerenityServer.modules.Contains(module.Name))
            {
                return;
            }
            SerenityServer.modules.Add(module);
            ResourcePath path = new ResourcePath("/dynamic/" + module.Name + "/");
            foreach (Page page in module.Pages)
            {
                SerenityServer.commonDomain.Resources.Add(path, page);
            }
            foreach (string embedPath in module.Assembly.GetManifestResourceNames())
            {
                string newpath = embedPath.Remove(0, module.ResourceNamespace.Length);
                string[] parts = newpath.Split('.');
                
                if (parts.Length > 2)
                {
                    path = new ResourcePath("/resource/" + module.Name + "/" + string.Join("/", parts, 0, parts.Length - 2) + "/");
                }
                else
                {
                    path = new ResourcePath("/resource/" + module.Name + "/");
                }
                string name = "";
                if (parts.Length > 1)
                {
                    name = parts[parts.Length - 2] + "." + parts[parts.Length - 1];
                }
                else
                {
                    name = parts[0];
                }

                using (Stream stream = module.Assembly.GetManifestResourceStream(embedPath))
                {
                    byte[] data = new byte[stream.Length];
                    if (stream.Read(data, 0, data.Length) == data.Length)
                    {
                        ResourceResource res = new ResourceResource(name, data);
                        res.ContentType = FileTypeRegistry.GetMimeType(parts[parts.Length - 1]);

                        SerenityServer.commonDomain.Resources.Add(path, res);
                    }
                }
            }
        }
        public static void ExtractResources(IEnumerable<Domain> domains)
        {
            foreach (Domain domain in domains)
            {
                SerenityServer.AddDomain(domain);
            }
        }
        public static void ExtractResources(IEnumerable<Module> modules)
        {
            foreach (Module module in modules)
            {
                SerenityServer.AddModule(module);
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
        public static Domain CommonDomain
        {
            get
            {
                return SerenityServer.commonDomain;
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
                if (SerenityServer.Status == OperationStatus.Started)
                {
                    throw new InvalidOperationException("Cannot modify server state while server is running.");
                }
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