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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Serenity.Web;

namespace Serenity.Net
{
    /// <summary>
    /// Provides a mechanism for recieving and responding to requests from clients (browsers).
    /// </summary>
    public abstract class ProtocolDriver : IDisposable
    {
        #region Constructors - Protected
        /// <summary>
        /// Initializes a new instance of the <see cref="Serenity.Net.ProtocolDriver"/> class.
        /// </summary>
        protected ProtocolDriver()
        {
            this.ProviderName = this.DefaultProviderName;
        }
        #endregion
        #region Fields - Private
        private string providerName;
        private bool isDisposed;
        private Socket listeningSocket;
        private bool isRunning;
        private ushort listeningPort;
        private bool isInitialized;
        private string schemaName;
        private string description;
        private Version version;
        #endregion
        #region Methods - Protected
        /// <summary>
        /// Provides a callback method to use for an async socket accept.
        /// </summary>
        /// <param name="ar"></param>
        /// <exception cref="System.NotSupportedException">Thrown if the
        /// current WebDriver does not support async operations.</exception>
        /// <exception cref="System.ObjectDisposedException">Thrown if the
        /// current WebDriver is already disposed.</exception>
        protected virtual void AcceptCallback(IAsyncResult ar)
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
            else if (ar == null)
            {
                throw new ArgumentNullException("ar");
            }
            else if (ar.AsyncState.GetType().TypeHandle.Equals(typeof(ProtocolDriverState).TypeHandle))
            {
                ProtocolDriverState state = (ProtocolDriverState)ar.AsyncState;
                Socket workSocket = state.WorkSocket;
                Socket socket = workSocket.EndAccept(ar);
                workSocket.BeginAccept(new AsyncCallback(this.AcceptCallback), state);

                this.HandleAcceptedConnection(socket);
            }
            else
            {
                return;
            }
        }
        /// <summary>
        /// Provides a callback method to use for an async disconnect operation.
        /// </summary>
        /// <param name="ar"></param>
        protected virtual void DisconnectCallback(IAsyncResult ar)
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            if (ar.AsyncState.GetType().TypeHandle.Equals(typeof(ProtocolDriverState).TypeHandle))
            {
                ProtocolDriverState state = ar.AsyncState as ProtocolDriverState;
                state.WorkSocket.EndDisconnect(ar);
            }
        }
        /// <summary>
        /// When overridden in a derived class, performs the actual releasing
        /// and cleaning up of unmanaged resources used by the current object.
        /// </summary>
        /// <param name="disposing">Indicates if disposal of the current
        /// object is actually desired.</param>
        protected virtual void Dispose(bool disposing)
        {
        }
        /// <summary>
        /// Handles a connection that has been accepted.
        /// </summary>
        /// <param name="socket"></param>
        protected void HandleAcceptedConnection(object socket)
        {
            if (socket == null)
            {
                throw new ArgumentNullException("socket");
            }

            Socket s = socket as Socket;
            if (s == null)
            {
                throw new ArgumentException("Specified socket object must be of type System.Net.Sockets.Socket", "socket");
            }

            this.HandleAcceptedConnection(s);
        }
        /// <summary>
        /// Handles a connection that has been accepted.
        /// </summary>
        /// <param name="socket"></param>
        protected virtual void HandleAcceptedConnection(Socket socket)
        {
            if (socket == null)
            {
                throw new ArgumentNullException("socket");
            }

            var request = this.RecieveRequest(socket);
            var response = new Response();
            if (request != null)
            {
                SerenityServer.ContextHandler.HandleRequest(request, response);
            }
            else
            {
                ErrorHandler.Handle(StatusCode.Http500InternalServerError);
            }
            this.SendResponse(socket, response);
            socket.Disconnect(false);
            socket.Close();
        }
        /// <summary>
        /// Provides a callback method to use for an async recieve operation.
        /// </summary>
        /// <param name="ar"></param>
        protected virtual void RecieveCallback(IAsyncResult ar)
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            if (ar.AsyncState.GetType().TypeHandle.Equals(typeof(ProtocolDriverState).TypeHandle))
            {
                ProtocolDriverState state = ar.AsyncState as ProtocolDriverState;
                state.WorkSocket.EndReceive(ar);
            }
        }
        /// <summary>
        /// Provides a callback method to use for an async send operation.
        /// </summary>
        /// <param name="ar"></param>
        protected virtual void SendCallback(IAsyncResult ar)
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            if (ar.AsyncState.GetType().TypeHandle.Equals(typeof(ProtocolDriverState).TypeHandle))
            {
                ProtocolDriverState state = ar.AsyncState as ProtocolDriverState;
                state.WorkSocket.EndSend(ar);
            }
        }
        #endregion
        #region Methods - Public
        /// <summary>
        /// Begins an asynchronous context recieve operation.
        /// </summary>
        /// <param name="socket">The socket to recieve the context from.</param>
        /// <param name="callback">The System.AsyncCallback delegate.</param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual IAsyncResult BeginRecieveRequest(Socket socket, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// Begins an asynchronous context send operation.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="context"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual IAsyncResult BeginSendResponse(Socket socket, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// Begins an asynchronous operation to start the current WebDriver.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual IAsyncResult BeginStart(AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// Begins an asynchronous operation to stop the current WebDriver.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual IAsyncResult BeginStop(AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// Releases any unmanaged resources used by the current object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            this.isDisposed = true;
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Ends a pending asynchronous context recieve operation.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public virtual Request EndRecieveRequest(IAsyncResult result)
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// Ends a pending asynchronous context send operation.
        /// </summary>
        /// <param name="result"></param>
        public virtual void EndSendResponse(IAsyncResult result)
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// Ends a pending asynchronous WebDriver start operation.
        /// </summary>
        /// <param name="result"></param>
        public virtual void EndStart(IAsyncResult result)
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// Ends a pending asynchronous WebDriver stop operation.
        /// </summary>
        /// <param name="result"></param>
        public virtual void EndStop(IAsyncResult result)
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// Creates and binds the listening socket, preparing the WebDriver so that it can be started.
        /// </summary>
        public virtual ProtocolDriverInitializationResult Initialize()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
            if (this.IsInitialized)
            {
                return ProtocolDriverInitializationResult.AlreadyInitialized;
            }
            this.isInitialized = true;
            this.ListeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            //TODO: Try and add some input-checking to avoid try/catch usage.
            try
            {
                this.ListeningSocket.Bind(new IPEndPoint(IPAddress.Any, this.ListeningPort));
                SerenityServer.OperationLog.Write("Listening socket bound to port " + this.ListeningPort.ToString(), Serenity.Logging.LogMessageLevel.Info);
                return ProtocolDriverInitializationResult.Suceeded;
            }
            catch (SocketException socketEx)
            {
                SerenityServer.ErrorLog.Write("Could not bind listening socket to desired port.\r\n" + socketEx.ToString(), Serenity.Logging.LogMessageLevel.Warning);
                return ProtocolDriverInitializationResult.FailedSocketBinding;
            }
        }
        public abstract Request RecieveRequest(Socket socket);
        /// <summary>
        /// Starts the WebDriver.
        /// </summary>
        public virtual bool Start()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            if (!this.IsInitialized)
            {
                return false;
            }
            else if (this.IsRunning)
            {
                return true;
            }

            this.isRunning = true;

            this.ListeningSocket.Listen(10);

            while (this.IsRunning)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.HandleAcceptedConnection), this.ListeningSocket.Accept());
            }
            return true;
        }
        /// <summary>
        /// Stops the WebDriver.
        /// </summary>
        public virtual void Stop()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            this.isRunning = false;
        }
        /// <summary>
        /// When overridden in a derived class, sends the supplied response to
        /// the client.
        /// </summary>
        public abstract bool SendResponse(Socket socket, Response response);
        #endregion
        #region Properties - Protected
        /// <summary>
        /// Gets or sets the socket that the current WebDriver uses to listen
        /// for incoming connections on.
        /// </summary>
        protected Socket ListeningSocket
        {
            get
            {
                return this.listeningSocket;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.listeningSocket = value;
            }
        }
        protected abstract ushort DefaultListeningPort
        {
            get;
        }
        protected abstract string DefaultProviderName
        {
            get;
        }
        protected abstract string DefaultSchemaName
        {
            get;
        }
        protected abstract string DefaultDescription
        {
            get;
        }
        protected abstract Version DefaultVersion
        {
            get;
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets an indication of whether the current object has already been
        /// disposed (cleaned up) or not.
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                return this.isDisposed;
            }
        }
        /// <summary>
        /// Gets a value that indicates whether the current WebDriver has been
        /// initialized yet.
        /// </summary>
        public bool IsInitialized
        {
            get
            {
                return this.isInitialized;
            }
            protected set
            {
                this.isInitialized = value;
            }
        }
        /// <summary>
        /// Gets a boolean value that indicates whether the current WebDriver
        /// is in a started status.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
        }
        /// <summary>
        /// Gets the port number that the current WebDriver is listening on
        /// for incoming connections.
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
        public string SchemaName
        {
            get
            {
                return this.schemaName;
            }
            set
            {
                this.schemaName = value;
            }
        }
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
        public Version Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }
        #endregion
    }
}
