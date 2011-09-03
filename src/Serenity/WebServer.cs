﻿/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using Serenity.Net;
using Serenity.Web;
using System.Collections.ObjectModel;

namespace Serenity
{
    /// <summary>
    /// Represents a web server instance.
    /// </summary>
    public class WebServer : IDisposable
    {
        #region Fields
        private HttpServer listener;
        private ManualResetEvent waitHandle;
        private bool isDisposed;
        private bool isRunning;
        private readonly List<WebApplication> apps = new List<WebApplication>();
        private Dictionary<Guid, ResourceBinding> resources = new Dictionary<Guid, ResourceBinding>();
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WebServer"/> class.
        /// </summary>
        public WebServer()
        {
            this.waitHandle = new ManualResetEvent(false);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="WebServer"/> class.
        /// </summary>
        ~WebServer()
        {
            this.Dispose(false);
        }
        #endregion
        #region Properties
        public ReadOnlyCollection<WebApplication> LoadedWebApps
        {
            get
            {
                return new ReadOnlyCollection<WebApplication>(this.apps);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the current <see cref="WebServer"/> is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                return this.isDisposed;
            }
            protected set
            {
                this.isDisposed = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the web server is running.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Releases all resources used by the web server.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void ProcessRequestCallback(Request request, Response response)
        {
            Contract.Requires(request != null);
            Contract.Requires(response != null);

            Trace.WriteLine("Processing request");

            // Obtain or create session
            if (request.Cookies.Contains("sws_session"))
            {

            }
            else
            {

            }

            Resource res = null;
            if (request.Url.Segments.Length > 1)
            {
                var seg1 = Uri.UnescapeDataString(request.Url.Segments[1]);

                if (seg1.StartsWith("{") && ((seg1.EndsWith("}") && seg1.Length == 38) || (seg1.EndsWith("}/") && seg1.Length == 39)))
                {
                    res = this.resources[Guid.Parse(seg1.TrimEnd('/'))].Resource;
                }
                else
                {
                    var bind = this.resources.FirstOrDefault(e => e.Value.Path == request.Url.AbsolutePath);
                    if (bind.Value != null)
                        res = bind.Value.Resource;
                }
            }

            if (res == null)
            {
                response.Status = StatusCode.Http404NotFound;
            }
            else
            {
                response.Status = StatusCode.Http200Ok;
                res.OnRequest(request, response);
            }

        }

        /// <summary>
        /// Loads a <see cref="WebApplication"/> into the current web server instance.
        /// </summary>
        /// <param name="webApp">The <see cref="WebApplication"/> to load.</param>
        public void LoadApplication(WebApplication webApp)
        {
            Contract.Requires(webApp != null);

            if (this.apps.Any(a => a.Name.Equals(webApp.Name, StringComparison.OrdinalIgnoreCase)))
                return;

            webApp.InitializeResources();

            this.apps.Add(webApp);

            foreach (var bind in webApp.Resources)
            {
                this.resources.Add(bind.Resource.UniqueID, bind);
            }
        }

        /// <summary>
        /// Loads 'built-in' web applications which are included in the Serenity API library.
        /// </summary>
        public void LoadBuiltinApplications()
        {
            this.LoadApplication(new Serenity.WebApps.ServerInfo.ServerInfoWebApp());
            this.LoadApplication(new Serenity.WebApps.ServerManagement.ServerManagementWebApp());
            this.LoadApplication(new Serenity.WebApps.UserManagement.UserManagement());
        }

        /// <summary>
        /// Unloads a <see cref="WebApplication"/> from the current web server instance.
        /// </summary>
        /// <param name="webApp">The <see cref="WebApplication"/> to unload.</param>
        public void UnloadApplication(WebApplication webApp)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Starts the web server.
        /// </summary>
        public void Start()
        {
            if (!this.isRunning)
            {
                this.isRunning = true;
                this.listener = new HttpServer(80);
                this.listener.ProcessRequestCallback = this.ProcessRequestCallback;
                this.listener.Start();
                this.waitHandle.WaitOne();
            }
        }

        /// <summary>
        /// Stops the web server.
        /// </summary>
        public void Stop()
        {
            this.isRunning = false;
            this.waitHandle.Set();
        }

        /// <summary>
        /// Releases unmanaged and managed resources used by the web server.
        /// </summary>
        /// <param name="disposing">Indicates whether to release managed resources (true), or unmanaged resources only (false).</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    // Release any managed resources.
                }

                // Release unmanaged resources.
                this.isDisposed = true;
            }
        }

        [ContractInvariantMethod]
        private void __ContractInvariant()
        {
            Contract.Invariant(this.apps != null);
        }
        #endregion
    }
}
