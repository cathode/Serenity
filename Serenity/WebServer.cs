/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using Serenity.Net;
using Serenity.Web;

namespace Serenity
{
    /// <summary>
    /// Represents a web server instance.
    /// </summary>
    public class WebServer : IDisposable
    {
        #region Fields
        private static WebServer active;
        private readonly List<WebApplication> apps;
        private readonly ResourceGraph resources;
        private HttpConnectionListener listener;
        private ManualResetEvent waitHandle;
        private bool isDisposed;
        private bool isRunning;
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WebServer"/> class.
        /// </summary>
        public WebServer()
        {
            this.waitHandle = new ManualResetEvent(false);
            this.resources = new ResourceGraph();
            this.apps = new List<WebApplication>();
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

        public ResourceGraph Resources
        {
            get
            {
                return this.resources;
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

            var segs = request.Url.Segments;
            ResourceGraphNode node = this.resources.Root;
            foreach (var seg in segs.Skip(1))
            {
                node = node.FirstOrDefault(n => n.SegmentName == seg);

                if (node == null)
                    break;
            }

            if (node != null)
                res = node.Resource;

            if (res == null)
            {
                response.Status = StatusCode.Http404NotFound;
            }
            else
            {
                response.Status = StatusCode.Http200Ok;
                res.OnRequest(request, response);
                response.ContentType = res.ContentType;
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

            this.resources.Root.AddChild(webApp.ApplicationRoot);
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
            Contract.Requires<ObjectDisposedException>(!this.IsDisposed);

            if (!this.isRunning)
            {
                this.isRunning = true;
                this.listener = new HttpConnectionListener(80);
                //this.listener.ProcessRequestCallback = this.ProcessRequestCallback;
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

        public static void MakeActive(WebServer server)
        {
            Contract.Requires(server != null);

            WebServer.active = server;
        }

        [ContractInvariantMethod]
        private void __ContractInvariant()
        {
            Contract.Invariant(this.apps != null);
        }
        #endregion
    }
}
