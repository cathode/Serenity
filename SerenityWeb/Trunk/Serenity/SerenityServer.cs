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

using Serenity.Logging;
using Serenity.Resources;
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
            SerenityServer.OperationLog.Write("Server created.", LogMessageLevel.Debug);
        }
        #endregion
        #region Fields - Private
        private static Log accessLog = new Log();
        private static ContextHandler contextHandler = new ContextHandler();
        private static readonly DomainCollection domains = new DomainCollection();
        private static readonly DriverPool driverPool = new DriverPool();
        private static Log errorLog = new Log();
        private static readonly ModuleCollection modules = new ModuleCollection();
        private static Log operationLog = new Log();
        private static OperationStatus status = OperationStatus.None;
        private static readonly ResourceCollection resources = new ResourceCollection();
        #endregion
        #region Methods - Public
        public static void AddDomain(Domain domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else if (SerenityServer.Status == OperationStatus.Started)
            {
                throw new InvalidOperationException(__Strings.CannotModifyWhileRunning);
            }
            else if (SerenityServer.domains.Contains(domain.HostName))
            {
                throw new InvalidOperationException(__Strings.DomainNameAlreadyExists);
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
                throw new InvalidOperationException(__Strings.CannotModifyWhileRunning);
            }
            else if (SerenityServer.modules.Contains(module.Name))
            {
                throw new InvalidOperationException(__Strings.ModuleNameAlreadyExists);
            }
            SerenityServer.modules.Add(module);
            string path = "/dynamic/" + module.Name + "/";
            DirectoryResource dr = new DirectoryResource(ResourcePath.Create(path));
            SerenityServer.Resources.Add(dr);
            foreach (DynamicResource page in module.Pages)
            {
                page.Path = ResourcePath.Create(path + page.Name);
                SerenityServer.Resources.Add(page);
            }
            foreach (string embedPath in module.Assembly.GetManifestResourceNames())
            {
                string newpath = embedPath.Remove(0, module.ResourceNamespace.Length);
                string[] parts = newpath.Split('.');

                if (parts.Length > 2)
                {
                    path = "/resource/" + module.Name + "/" + string.Join("/", parts, 0, parts.Length - 2) + "/";
                }
                else
                {
                    path = "/resource/" + module.Name + "/";
                }
                if (!SerenityServer.Resources.Contains(ResourcePath.Create(path)))
                {
                    SerenityServer.Resources.Add(new DirectoryResource(ResourcePath.Create(path)));
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

                        res.Path = ResourcePath.Create(path + name);
                        SerenityServer.Resources.Add(res);
                    }
                }
            }
        }
        public static void AddDomains(IEnumerable<Domain> domains)
        {
            if (domains == null)
            {
                throw new ArgumentNullException("domains");
            }
            else if (SerenityServer.Status == OperationStatus.Started)
            {
                throw new InvalidOperationException(__Strings.CannotModifyWhileRunning);
            }

            foreach (Domain domain in domains)
            {
                SerenityServer.AddDomain(domain);
            }
        }
        public static void AddModules(IEnumerable<Module> modules)
        {
            if (modules == null)
            {
                throw new ArgumentNullException("modules");
            }
            else if (SerenityServer.Status == OperationStatus.Started)
            {
                throw new InvalidOperationException(__Strings.CannotModifyWhileRunning);
            }

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
                    throw new InvalidOperationException(__Strings.CannotModifyWhileRunning);
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
        public static ModuleCollection Modules
        {
            get
            {
                return SerenityServer.modules;
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
        public static ResourceCollection Resources
        {
            get
            {
                return SerenityServer.resources;
            }
        }
        #endregion
    }
}