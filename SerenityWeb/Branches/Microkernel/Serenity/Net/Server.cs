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
using System.Net.Sockets;
using Serenity.Web.Resources;
using System.IO;
using Serenity.Properties;

namespace Serenity.Net
{
    /// <summary>
    /// Represents basic functionality for server objects. A server handles
    /// network communication with clients and directs request/response
    /// generation.
    /// </summary>
    public abstract class Server : IDisposable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Server"/> class.
        /// </summary>
        protected Server()
        {
            this.rootResource = new RootResource(this);
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
        private Socket listener;
        private readonly ModuleCollection modules = new ModuleCollection();
        private readonly EventLog log = new EventLog();
        private Resource rootResource;
        #endregion
        #region Methods
        /// <summary>
        /// Provides a callback method for asynchronous accept operations.
        /// </summary>
        /// <param name="result"></param>
        protected virtual void AcceptCallback(IAsyncResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }
            var state = (ServerAsyncState)result.AsyncState;

            try
            {
                state.Client = this.Listener.EndAccept(result);
                this.Log.RecordEvent(string.Format(AppResources.ClientConnectedMessage, state.Client.RemoteEndPoint), EventKind.Info);
            }
            catch (SocketException ex)
            {
                this.Log.RecordEvent(ex.Message, EventKind.Notice, ex.StackTrace);
                return;
            }
            state.Reset();
            state.Client.BeginReceive(state.ReceiveBuffer, 0,
                          Math.Min(state.Client.Available, state.ReceiveBuffer.Length),
                          SocketFlags.None, new AsyncCallback(this.ReceiveCallback), state);

            var newState = this.CreateStateObject();
            newState.Listener = state.Listener;

            this.Listener.BeginAccept(new AsyncCallback(this.AcceptCallback), newState);
        }
        /// <summary>
        /// Creates a new <see cref="ServerAsyncState"/>.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// To utilize a more complex async state object, make your async state
        /// object inherit from <see cref="ServerAsyncState"/> and then
        /// override this method to return a new instance of your derived type.
        /// </remarks>
        protected virtual ServerAsyncState CreateStateObject()
        {
            return new ServerAsyncState();
        }
        /// <summary>
        /// Disposes the current <see cref="Server"/>.
        /// </summary>
        public void Dispose()
        {
            if (!this.IsDisposed)
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
                this.isDisposed = true;
            }
        }
        /// <summary>
        /// Disposes the current <see cref="Server"/>.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            //TODO: Implement dispose for Server
        }
        /// <summary>
        /// Initializes the current <see cref="Server"/>. Commonly, tasks such
        /// as creating and binding sockets, loading modules, and other similar
        /// actions are carried out during initialization.
        /// </summary>
        public void Initialize()
        {
            if (!this.IsInitialized)
            {
                this.OnInitializing(null);
                this.isInitialized = true;
            }
        }
        /// <summary>
        /// Raises the <see cref="Initializing"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnInitializing(EventArgs e)
        {
            if (this.Initializing != null)
            {
                this.Initializing(this, e);
            }

            foreach (string modulePath in this.Profile.Modules)
            {
                foreach (var module in Module.LoadModules(modulePath))
                {
                    DirectoryResource modTree = new DirectoryResource()
                    {
                        Name = module.Name,
                        Owner = this
                    };

                    modTree.Add(module.Resources);

                    if (this.RootResource is DirectoryResource)
                    {
                        ((DirectoryResource)this.RootResource).Add(modTree);
                    }
                    this.modules.Add(module);
                }
            }
        }
        /// <summary>
        /// Raises the <see cref="Starting"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnStarting(EventArgs e)
        {
            if (this.Starting != null)
            {
                this.Starting(this, e);
            }
            if (this.Profile.UseIPv6)
            {
                this.Listener = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                this.Listener.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName)27, 0); //Set IPV6_V6ONLY to false
            }
            else
            {
                this.Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            this.listener.Bind(this.Profile.LocalEndPoint);
            this.Listener.Listen(this.Profile.ConnectionBacklog);

            var newState = this.CreateStateObject();
            newState.Listener = this.Listener;

            this.Listener.BeginAccept(new AsyncCallback(this.AcceptCallback), newState);
        }
        /// <summary>
        /// Raises the <see cref="Stopping"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnStopping(EventArgs e)
        {
            if (this.Stopping != null)
            {
                this.Stopping(this, e);
            }
            this.Listener.Close();
        }
        /// <summary>
        /// Provides a callback method for asynchronous socket receive calls.
        /// </summary>
        /// <param name="result"></param>
        protected virtual void ReceiveCallback(IAsyncResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }
            var state = (ServerAsyncState)result.AsyncState;
            if (state.Client.Connected)
            {
                try
                {
                    state.Client.EndReceive(result);
                }
                catch (SocketException ex)
                {
                    this.Log.RecordEvent(ex.Message, EventKind.Info, ex.StackTrace);
                }

                state.SwapBuffers();

                if (state.Client.Connected)
                {
                    state.Client.BeginReceive(state.ReceiveBuffer, 0,
                        Math.Min(state.Client.Available, state.ReceiveBuffer.Length),
                        SocketFlags.None, new AsyncCallback(this.ReceiveCallback), state);
                }
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
        /// <summary>
        /// Gets a value that indicates if the current <see cref="Server"/>
        /// has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                return this.isDisposed;
            }
        }
        /// <summary>
        /// Gets a value that indicates if the current <see cref="Server"/> 
        /// has been initialized.
        /// </summary>
        public bool IsInitialized
        {
            get
            {
                return this.isInitialized;
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
        /// <summary>
        /// Gets the <see cref="Socket"/> that is used to listen for incoming
        /// connections from clients.
        /// </summary>
        protected Socket Listener
        {
            get
            {
                return this.listener;
            }
            set
            {
                this.listener = value;
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="ServerProfile"/> which controls the
        /// operating behavior of the current <see cref="Server"/>.
        /// </summary>
        /// <remarks>
        /// This property can only be set when the <see cref="Server"/> is not
        /// running. That is, when <see cref="IsRunning"/> returns false. If
        /// an attempt is made to alter the server's profile while it is
        /// running, a <see cref="InvalidOperationException"/> will be thrown.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown when an attempt
        /// is made to alter the profile of the current <see cref="Server"/>
        /// while it is running.</exception>
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
        public EventLog Log
        {
            get
            {
                return this.log;
            }
        }
        public Resource RootResource
        {
            get
            {
                return this.rootResource;
            }
            set
            {
                this.rootResource = value;
            }
        }
        #endregion
    }
}
