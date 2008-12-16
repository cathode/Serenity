/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *----------------------------------------------------------------------------*
 * Authors:                                                                   *
 * - Will 'AnarkiNet' Shelley (AnarkiNet@gmail.com): Original Author          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Serenity.Net
{
    public abstract class Server : IDisposable
    {
        #region Constructors
        protected Server()
        {
            
        }
        #endregion
        #region Events
        /// <summary>
        /// Raised when the current <see cref="Server"/> is being initialized.
        /// </summary>
        public event EventHandler Initializing;
        /// <summary>
        /// Raised when the current <see cref="Server"/> is being started.
        /// </summary>
        public event EventHandler Starting;
        /// <summary>
        /// Raised when the current <see cref="Server"/> is being stopped.
        /// </summary>
        public event EventHandler Stopping;
        #endregion
        #region Fields
        private ServerProfile profile;
        private bool isRunning;
        private bool isDisposed;
        private bool isInitialized;
        #endregion
        #region Methods
        /// <summary>
        /// Disposes the current <see cref="Server"/>.
        /// </summary>
        public void Dispose()
        {
            if (!this.IsDisposed)
            {
                this.Dispose(true);
            }
        }
        /// <summary>
        /// Disposes the current <see cref="Server"/>.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            this.isDisposed = true;
        }
        public void Initialize()
        {
            if (!this.IsInitialized)
            {
                this.OnInitializing(null);
                this.isInitialized = true;
            }
        }
        protected virtual void OnInitializing(EventArgs e)
        {
            if (this.Initializing != null)
            {
                this.Initializing(this, e);
            }
        }
        /// <summary>
        /// Raises the <see cref="Started"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnStarting(EventArgs e)
        {
            if (this.Starting != null)
            {
                this.Starting(this, e);
            }
        }
        /// <summary>
        /// Raises the <see cref="Stopped"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnStopping(EventArgs e)
        {
            if (this.Stopping != null)
            {
                this.Stopping(this, e);
            }
        }
        /// <summary>
        /// Starts the current <see cref="Server"/>.
        /// </summary>
        public void Start()
        {
            this.OnStarting(null);

            this.isRunning = true;
        }
        /// <summary>
        /// Stops the current <see cref="Server"/>.
        /// </summary>
        public void Stop()
        {
            this.OnStopping(null);

            this.isRunning = false;
        }
        #endregion
        #region Properties
        public bool IsInitialized
        {
            get
            {
                return this.isInitialized;
            }
        }
        public bool IsDisposed
        {
            get
            {
                return this.isDisposed;
            }
        }
        /// <summary>
        /// Gets a value that indicates if the current <see cref="Server"/> is running.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
        }
        public ServerProfile Profile
        {
            get
            {
                return this.profile;
            }
            set
            {
                if (this.IsRunning)
                {
                    throw new InvalidOperationException("Cannot alter the server profile while the server is running.");
                }
                this.profile = value;
            }
        }
        #endregion
    }
}
