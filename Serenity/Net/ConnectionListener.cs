/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Serenity.Web;

namespace Serenity.Net
{
    /// <summary>
    /// Represents basic functionality used by types that listen for incoming
    /// connections over a network socket.
    /// </summary>
    /// <typeparam name="T">The type of the connection that is being listened
    /// for.</typeparam>
    public abstract class ConnectionListener<T> where T : Connection
    {
        #region Fields
        /// <summary>
        /// Backing field for the <see cref="ListenSocket"/> property.
        /// </summary>
        private Socket listenSocket;

        /// <summary>
        /// Backing field for the <see cref="State"/> property.
        /// </summary>
        private ConnectionListenerState state;
        #endregion
        #region Events
        /// <summary>
        /// Raised when the connection listener performs initialization.
        /// </summary>
        public event EventHandler Initializing;

        /// <summary>
        /// Raised when the connection listener is directed to start listening
        /// for incoming connections.
        /// </summary>
        public event EventHandler Starting;

        /// <summary>
        /// Raised when the connection listener is directed to stop listening
        /// for incoming connections.
        /// </summary>
        public event EventHandler Stopping;

        /// <summary>
        /// Raised when the connection listener accepts an incoming connection.
        /// </summary>
        public event EventHandler<ConnectionEventArgs<T>> ConnectionAccepted;

        /// <summary>
        /// Raised when an established connection that was previously accepted
        /// receives a request that should be handled.
        /// </summary>
        public event EventHandler<ResourceExecutionContextEventArgs> ContextPending;
        #endregion
        #region Properties
        /// <summary>
        /// Gets the default port number to bind the listener to.
        /// </summary>
        public abstract int DefaultPort
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Socket"/> on which to listen for incoming
        /// connections.
        /// </summary>
        public Socket ListenSocket
        {
            get
            {
                return this.listenSocket;
            }
        }

        /// <summary>
        /// Gets or sets an <see cref="EndPoint"/> that the listener binds to
        /// when listening for connections.
        /// </summary>
        public EndPoint LocalEndPoint
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="ConnectionListenerState"/> of the connection listener.
        /// </summary>
        public ConnectionListenerState State
        {
            get
            {
                return this.state;
            }
            protected set
            {
                this.state = value;
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Initializes the connection listener.
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            // Repeated initializations have no effect beyond the first.
            if (this.State >= ConnectionListenerState.Initializing)
                return true;

            try
            {
                this.State = ConnectionListenerState.Initializing;
                this.OnInitializing(EventArgs.Empty);
                this.State = ConnectionListenerState.Initialized;
                return true;
            }
            catch (Exception ex)
            {
                this.State = ConnectionListenerState.Faulted;
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Directs the listener to begin listening for connections.
        /// </summary>
        /// <returns>true if the listener is in a <see cref="ConnectionListenerState.Started"/> state when this method returns; otherwise, false.</returns>
        /// <remarks>
        /// If this method returns false but the value of the <see cref="State"/>
        /// property is something other than <see cref="ConnectionListenerState.Faulted"/>,
        /// it indicates that the attempt to start failed because the listener
        /// wasn't initialized and an attempt to initialize failed.
        /// </remarks>
        public bool Start()
        {
            // Repeated attempts to start have no effect beyond the first.
            if (this.State >= ConnectionListenerState.Starting)
                return true;

            try
            {
                // If we're not initialized yet, try to do that first.
                if (this.State < ConnectionListenerState.Initialized)
                    if (!this.Initialize())
                        return false; // Can't start because initialization failed.

                this.State = ConnectionListenerState.Starting;
                this.OnStarting(EventArgs.Empty);
                this.State = ConnectionListenerState.Started;

                return true;
            }
            catch
            {
                this.State = ConnectionListenerState.Faulted;
                return false;
            }
        }

        /// <summary>
        /// Directs the listener to stop listening for connections.
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            return false;
        }

        /// <summary>
        /// Creates a new <see cref="Socket"/> with customized options for the connection listener implementation.
        /// </summary>
        /// <returns>The new <see cref="Socket"/> instance.</returns>
        protected virtual Socket CreateSocket()
        {
            // This implementation should work for most connection listeners.
            try
            {
                // Try to make a dual-mode (IPv4 + IPv6) listen socket. This will fail pre-Vista.
                var sock = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                sock.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
                return sock;
            }
            catch (SocketException)
            {
                // Fallback to a plain, boring IPv4 listener.
                return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
        }

        protected abstract T CreateConnection(Socket socket);

        /// <summary>
        /// Raises the <see cref="Initializing"/> event.
        /// </summary>
        /// <param name="e">Event data associated with the event.</param>
        protected virtual void OnInitializing(EventArgs e)
        {
            if (this.Initializing != null)
                this.Initializing(this, e ?? EventArgs.Empty);

            this.listenSocket = this.CreateSocket();

            if (this.LocalEndPoint == null)
                this.LocalEndPoint = new IPEndPoint(IPAddress.IPv6Any, this.DefaultPort);

            this.listenSocket.Bind(this.LocalEndPoint);
        }

        /// <summary>
        /// Raises the <see cref="Starting"/> event.
        /// </summary>
        /// <param name="e">Event data associated with the event.</param>
        protected virtual void OnStarting(EventArgs e)
        {
            if (this.Starting != null)
                this.Starting(this, e ?? EventArgs.Empty);

            this.ListenSocket.Listen(10);
            this.ListenSocket.BeginAccept(this.ListenerAcceptCallback, null);
        }

        /// <summary>
        /// Raises the <see cref="Stopping"/> event.
        /// </summary>
        /// <param name="e">Event data associated with the event.</param>
        protected virtual void OnStopping(EventArgs e)
        {
            if (this.Stopping != null)
                this.Stopping(this, e ?? EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="ConnectionAccepted"/> event.
        /// </summary>
        /// <param name="e">Event data associated with the event.</param>
        protected virtual void OnConnectionAccepted(ConnectionEventArgs<T> e)
        {
            if (this.ConnectionAccepted != null)
                this.ConnectionAccepted(this, e);

            e.Connection.ContextPending += new EventHandler<ResourceExecutionContextEventArgs>(Connection_ContextPending);
            e.Connection.Run();
        }

        void Connection_ContextPending(object sender, ResourceExecutionContextEventArgs e)
        {
            this.OnContextPending(e);
        }

        /// <summary>
        /// Raises the <see cref="ContextPending"/> event.
        /// </summary>
        /// <param name="e">Event data associated with the event.</param>
        protected virtual void OnContextPending(ResourceExecutionContextEventArgs e)
        {
            if (this.ContextPending != null)
                this.ContextPending(this, e);
        }

        /// <summary>
        /// Provides a basic implementation for the listener's async socket accept callback.
        /// </summary>
        /// <param name="result"></param>
        protected virtual void ListenerAcceptCallback(IAsyncResult result)
        {
            Contract.Requires(result != null);

            var socket = this.ListenSocket.EndAccept(result);
            this.ListenSocket.BeginAccept(this.ListenerAcceptCallback, null);

            var connection = this.CreateConnection(socket);
            this.OnConnectionAccepted(new ConnectionEventArgs<T>
            {
                Connection = connection
            });
        }
        #endregion
    }
}
