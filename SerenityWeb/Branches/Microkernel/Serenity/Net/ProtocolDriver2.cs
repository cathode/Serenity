/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Serenity.Net
{
    /// <summary>
    /// Represents an object that provides support for an application-layer network protocol.
    /// </summary>
    public abstract class ProtocolDriver2 : IDisposable
    {
        #region Constructors - Protected
        /// <summary>
        /// Initializes a new instance of the <see cref="ProtocolDriver2"/> class.
        /// </summary>
        protected ProtocolDriver2()
        {
            this.Description = this.DefaultDescription;
            this.ListeningPort = this.DefaultListeningPort;
            this.ProviderName = this.DefaultProviderName;
            this.SchemeName = this.DefaultSchemeName;
            this.SupportedVersion = this.DefaultSupportedVersion;
        }
        #endregion
        #region Events - Public
        /// <summary>
        /// Raised when a recieved request is available to be processed.
        /// </summary>
        public event EventHandler<RequestAvailableEventArgs> RequestAvailable;
        /// <summary>
        /// Raised when the current <see cref="ProtocolDriver2"/> has been started.
        /// </summary>
        public event EventHandler Started;
        /// <summary>
        /// Raised when the current <see cref="ProtocolDriver2"/> has been stopped.
        /// </summary>
        public event EventHandler Stopped;
        #endregion
        #region Fields - Private
        private string description;
        private bool isRunning;
        private ushort listeningPort;
        private string providerName;
        private string schemeName;
        private Version supportedVersions;
        private bool isDisposed;
        #endregion
        #region Methods - Protected
        public void Dispose()
        {
            if (!this.IsDisposed)
            {
                this.Dispose(true);
                this.isDisposed = true;
            }
        }
        /// <summary>
        /// Disposes the current <see cref="ProtocolDriver2"/>.
        /// </summary>
        /// <param name="disposing"></param>
        protected void Dispose(bool disposing)
        {

        }
        /// <summary>
        /// Raises the <see cref="RequestAvailable"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnRequestAvailable(RequestAvailableEventArgs e)
        {
            if (this.RequestAvailable != null)
            {
                this.RequestAvailable(this, e);
            }
        }
        /// <summary>
        /// Raises the <see cref="Started"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnStarted(EventArgs e)
        {
            if (this.Started != null)
            {
                this.Started(this, e);
            }
        }
        /// <summary>
        /// Raises the <see cref="Stopped"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnStopped(EventArgs e)
        {
            if (this.Stopped != null)
            {
                this.Stopped(this, e);
            }
        }
        #endregion
        #region Methods - Public
        /// <summary>
        /// Starts the current <see cref="ProtocolDriver2"/>.
        /// </summary>
        public void Start()
        {
            this.OnStarted(null);

            this.isRunning = true;
        }
        /// <summary>
        /// Stops the current <see cref="ProtocolDriver2"/>.
        /// </summary>
        public void Stop()
        {
            this.OnStopped(null);

            this.isRunning = false;
        }
        #endregion
        #region Properties - Protected
        /// <summary>
        /// When overridden in a derived class, gets the default description for the current <see cref="ProtocolDriver2"/>.
        /// </summary>
        protected abstract string DefaultDescription
        {
            get;
        }
        /// <summary>
        /// When overridden in a derived class, gets the default listening port for the current <see cref="ProtocolDriver2"/>.
        /// </summary>
        protected abstract ushort DefaultListeningPort
        {
            get;
        }
        /// <summary>
        /// When overridden in a derived class, gets the default provider name for the current <see cref="ProtocolDriver2"/>.
        /// </summary>
        protected abstract string DefaultProviderName
        {
            get;
        }
        /// <summary>
        /// When overridden in a derived class, gets the default scheme name for the current <see cref="ProtocolDriver2"/>.
        /// </summary>
        protected abstract string DefaultSchemeName
        {
            get;
        }
        /// <summary>
        /// When overridden in a derived class, gets the default supported version range for the current <see cref="ProtocolDriver2"/>.
        /// </summary>
        protected abstract Version DefaultSupportedVersion
        {
            get;
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets or sets a description of the current <see cref="ProtocolDriver2"/>.
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }
        /// <summary>
        /// Gets a value that indicates if the current <see cref="ProtocolDriver2"/> is running.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
        }
        /// <summary>
        /// Gets or sets the port number to use when listening for connections.
        /// </summary>
        public ushort ListeningPort
        {
            get
            {
                return this.listeningPort;
            }
            set
            {
                this.listeningPort = value;
            }
        }
        /// <summary>
        /// Gets or sets the name of the vendor that provides the current <see cref="ProtocolDriver2"/>.
        /// </summary>
        public string ProviderName
        {
            get
            {
                return this.providerName;
            }
            set
            {
                this.providerName = value;
            }
        }
        /// <summary>
        /// Gets or sets the URI scheme used by the current <see cref="ProtocolDriver2"/>.
        /// </summary>
        public string SchemeName
        {
            get
            {
                return this.schemeName;
            }
            set
            {
                this.schemeName = value;
            }
        }
        /// <summary>
        /// Gets or sets a <see cref="VersionRange"/> that indicates the version(s) supported by the current <see cref="ProtocolDriver2"/>.
        /// </summary>
        public Version SupportedVersion
        {
            get
            {
                return this.supportedVersions;
            }
            set
            {
                this.supportedVersions = value;
            }
        }
        public bool IsDisposed
        {
            get
            {
                return this.isDisposed;
            }
        }
        #endregion
    }
}
