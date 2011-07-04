/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
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
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WebServer"/> class.
        /// </summary>
        public WebServer()
        {
            this.waitHandle = new ManualResetEvent(false);
        }

        ~WebServer()
        {
            this.Dispose(false);
        }
        #endregion
        #region Fields
        private HttpServer listener;
        private ManualResetEvent waitHandle;
        private bool isDisposed;
        private bool isRunning;
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

        public void ProcessRequestCallback(Request request, Response response)
        {
            Contract.Requires(request != null);
            Contract.Requires(response != null);

            Trace.WriteLine("Processing request");

        }

        /// <summary>
        /// Loads a <see cref="WebApplication"/> into the current web server instance.
        /// </summary>
        /// <param name="webApp">The <see cref="WebApplication"/> to load.</param>
        public void LoadApplication(WebApplication webApp)
        {

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
        #endregion
        #region Properties
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
    }
}
