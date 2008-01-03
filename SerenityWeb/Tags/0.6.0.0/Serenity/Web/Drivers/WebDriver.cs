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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Serenity.Web.Drivers
{
    public delegate void HeaderHandlerCallback(CommonContext context, Header header);

    /// <summary>
    /// Provides a mechanism for recieving and responding to requests from clients (browsers).
    /// </summary>
    public abstract class WebDriver : IDisposable
    {
        #region Constructors - Protected
        /// <summary>
        /// Initializes a new instance of the WebDriver class.
        /// </summary>
        /// <param name="settings">The WebDriverSettings which control the
        /// behaviour of the new WebDriver instance.</param>
        protected WebDriver(WebDriverSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            this.settings = settings;
            this.acceptDelegate = new AsyncCallback(this.AcceptCallback);
            this.disconnectDelegate = new AsyncCallback(this.DisconnectCallback);
            this.headerHandlers = new Dictionary<string, HeaderHandlerCallback>();
            this.recieveDelegate = new AsyncCallback(this.RecieveCallback);
            this.sendDelegate = new AsyncCallback(this.SendCallback);
        }
        #endregion
        #region Fields - Private
        private readonly AsyncCallback acceptDelegate;
        private readonly AsyncCallback disconnectDelegate;
        private readonly Dictionary<string, HeaderHandlerCallback> headerHandlers;
        private DriverInfo info;
        private bool isDisposed = false;
        private Socket listeningSocket;
        private readonly AsyncCallback recieveDelegate;
        private readonly AsyncCallback sendDelegate;
        private readonly WebDriverSettings settings;
        private OperationStatus status = OperationStatus.None;
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
            else if (ar.AsyncState.GetType().TypeHandle.Equals(typeof(WebDriverState).TypeHandle))
            {
                WebDriverState state = (WebDriverState)ar.AsyncState;
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

            if (ar.AsyncState.GetType().TypeHandle.Equals(typeof(WebDriverState).TypeHandle))
            {
                WebDriverState state = ar.AsyncState as WebDriverState;
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
        /// <param name="socketObject"></param>
        protected void HandleAcceptedConnection(object socketObject)
        {
            if (socketObject is Socket)
            {
                this.HandleAcceptedConnection((Socket)socketObject);
            }
            else
            {
                throw new ArgumentException("socketObject must be of type System.Net.Sockets.Socket!", "socketObject");
            }
        }
        /// <summary>
        /// Handles a connection that has been accepted.
        /// </summary>
        /// <param name="socket"></param>
        protected virtual void HandleAcceptedConnection(Socket socket)
        {
            CommonContext context = this.RecieveContext(socket);
            if (context != null)
            {
                SerenityServer.ContextHandler.HandleContext(context);
            }
            else
            {
                context = new CommonContext(this);
                ErrorHandler.Handle(context, StatusCode.Http500InternalServerError);
            }
            this.SendContext(socket, context);
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

            if (ar.AsyncState.GetType().TypeHandle.Equals(typeof(WebDriverState).TypeHandle))
            {
                WebDriverState state = ar.AsyncState as WebDriverState;
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

            if (ar.AsyncState.GetType().TypeHandle.Equals(typeof(WebDriverState).TypeHandle))
            {
                WebDriverState state = ar.AsyncState as WebDriverState;
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
        public virtual IAsyncResult BeginRecieveContext(Socket socket, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }
        public virtual IAsyncResult BeginSendContext(Socket socket, CommonContext context, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }
        public virtual IAsyncResult BeginStart(AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }
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
        public virtual CommonContext EndRecieveContext(IAsyncResult result)
        {
            throw new NotSupportedException();
        }
        public virtual void EndSendContext(IAsyncResult result)
        {
            throw new NotSupportedException();
        }
        public virtual void EndStart(IAsyncResult result)
        {
            throw new NotSupportedException();
        }
        public virtual void EndStop(IAsyncResult result)
        {
            throw new NotSupportedException();
        }
        public bool HandleHeader(CommonContext context, Header header)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            else if (header == null)
            {
                throw new ArgumentNullException("header");
            }
            else if (this.headerHandlers.ContainsKey(header.Name))
            {
                this.headerHandlers[header.Name].Invoke(context, header);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Creates and binds the listening socket, preparing the WebDriver so that it can be started.
        /// </summary>
        public virtual bool Initialize()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            if (this.status < OperationStatus.Initialized)
            {
                this.ListeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

                foreach (ushort port in this.Settings.Ports)
                {
                    //TODO: Try and add some input-checking to avoid try/catch usage.
                    try
                    {
                        this.ListeningSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                        break;
                    }
                    catch
                    {
                    }
                }
                if (this.ListeningSocket.IsBound)
                {
                    //TODO: Log "socket bind succeded" informative message.
                    this.status = OperationStatus.Initialized;
                    return true;
                }
                else
                {
                    //TODO: Log "socket bind failed" error message.
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool IsHeaderHandlerRegistered(string headerName)
        {
            return this.headerHandlers.ContainsKey(headerName);
        }
        public abstract CommonContext RecieveContext(Socket socket);
        public void RegisterHeaderHandler(string headerName, HeaderHandlerCallback callback)
        {
            if (headerName == null)
            {
                throw new ArgumentNullException("headerName");
            }
            else if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            else if (this.IsHeaderHandlerRegistered(headerName))
            {
                throw new InvalidOperationException("A header handler has already been registered for that header name.");
            }

            this.headerHandlers.Add(headerName, callback);
        }
        /// <summary>
        /// Starts the WebDriver.
        /// </summary>
        public virtual bool Start()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            if (this.Status >= OperationStatus.Initialized)
            {
                this.Status = OperationStatus.Started;
                this.ListeningSocket.Listen(10);

                while (this.Status == OperationStatus.Started)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.HandleAcceptedConnection), this.ListeningSocket.Accept());
                }
                return true;
            }
            else
            {
                return false;
            }
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

            this.status = OperationStatus.Stopped;
        }
        /// <summary>
        /// When overridden in a derived class, sends the supplied response to
        /// the client.
        /// </summary>
        public abstract bool SendContext(Socket socket, CommonContext context);
        #endregion
        #region Properties - Protected
        /// <summary>
        /// Gets a delegate which allows invocation of the callback method
        /// used when performing an asynchronous accept operation.
        /// </summary>
        protected AsyncCallback AcceptDelegate
        {
            get
            {
                return this.acceptDelegate;
            }
        }
        /// <summary>
        /// Gets a delegate which allows invocation of the callback method
        /// used when performing an asynchronous disconnect operation.
        /// </summary>
        protected AsyncCallback DisconnectDelegate
        {
            get
            {
                return this.disconnectDelegate;
            }
        }
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
        /// <summary>
        /// Gets a delegate which allows invocation of the callback method
        /// used when performing an asynchronous recieve operation.
        /// </summary>
        protected AsyncCallback RecieveDelegate
        {
            get
            {
                return this.recieveDelegate;
            }
        }
        /// <summary>
        /// Gets a delegate which allows invocation of the callback method
        /// used when performing an asynchronous send operation.
        /// </summary>
        protected AsyncCallback SendDelegate
        {
            get
            {
                return this.sendDelegate;
            }
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets an indicator of the current WebDriver's level of support for
        /// recieving in an asynchronous manner.
        /// </summary>
        /// <remarks>Returns false always unless overridden in a derived class.
        /// </remarks>
        public virtual bool CanRecieveAsync
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// Gets an indicator of the current WebDriver's level of support for
        /// sending in an asynchronous manner.
        /// </summary>
        /// <remarks>Returns false always unless overridden in a derived class.
        /// </remarks>
        public virtual bool CanSendAsync
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// Gets a DriverInfo object which contains information about the
        /// current WebDriver.
        /// </summary>
        public DriverInfo Info
        {
            get
            {
                return this.info;
            }
            protected set
            {
                this.info = value;
            }
        }
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
                if (this.status >= OperationStatus.Initialized)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Gets a boolean value that indicates whether the current WebDriver
        /// is in a started status.
        /// </summary>
        public bool IsStarted
        {
            get
            {
                if (this.status == OperationStatus.Started)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Gets a boolean value that indicates whether the current WebDriver
        /// is in a stopped status.
        /// </summary>
        public bool IsStopped
        {
            get
            {
                if (this.status == OperationStatus.Stopped)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Gets the port number that the current WebDriver is listening on
        /// for incoming connections.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        /// Thrown if the current WebDriver has already been disposed.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown if the current WebDriver has not been initialized.
        /// </exception>
        public ushort ListeningPort
        {
            get
            {
                if (this.IsDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }
                else if (!this.ListeningSocket.IsBound)
                {
                    throw new InvalidOperationException("The current WebDriver is not initialized.");
                }
                //TODO: Maybe try and improve performance or reliability here.
                return (ushort)((IPEndPoint)this.listeningSocket.LocalEndPoint).Port;
            }
        }
        /// <summary>
        /// Gets the WebDriverSettings which determine the behaviour of the
        /// current WebDriver.
        /// </summary>
        public WebDriverSettings Settings
        {
            get
            {
                return this.settings;
            }
        }
        /// <summary>
        /// Gets the status of the current WebDriver.
        /// </summary>
        public OperationStatus Status
        {
            get
            {
                return this.status;
            }
            protected set
            {
                this.status = value;
            }
        }
        #endregion
    }
}
