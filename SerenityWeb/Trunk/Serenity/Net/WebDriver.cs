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
    public abstract class WebDriver : IDisposable
    {
        #region Constructors - Protected
        /// <summary>
        /// Initializes a new instance of the WebDriver class.
        /// </summary>
        protected WebDriver()
        {
        }
        #endregion
        #region Fields - Private
        private DriverInfo info;
        private bool isDisposed;
        private Socket listeningSocket;
        private bool isRunning;
        private ushort listeningPort;
        private bool isInitialized;
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

            ;

            if (this.RecieveRequest(socket))
            {
                SerenityServer.ContextHandler.HandleContext();
            }
            else
            {
                ErrorHandler.Handle(StatusCode.Http500InternalServerError);
            }
            this.SendResponse(socket);
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
        public virtual void EndRecieveRequest(IAsyncResult result)
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
        public virtual WebDriverInitializationResult Initialize()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
            if (this.IsInitialized)
            {
                return WebDriverInitializationResult.AlreadyInitialized;
            }
            this.isInitialized = true;
            this.ListeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            //TODO: Try and add some input-checking to avoid try/catch usage.
            try
            {
                this.ListeningSocket.Bind(new IPEndPoint(IPAddress.Any, this.ListeningPort));
                SerenityServer.OperationLog.Write("Listening socket bound to port " + this.ListeningPort.ToString(), Serenity.Logging.LogMessageLevel.Info);
                return WebDriverInitializationResult.Suceeded;
            }
            catch (SocketException socketEx)
            {
                SerenityServer.ErrorLog.Write("Could not bind listening socket to desired port.\r\n" + socketEx.ToString(), Serenity.Logging.LogMessageLevel.Warning);
                return WebDriverInitializationResult.FailedSocketBinding;
            }
        }
        public abstract bool RecieveRequest(Socket socket);
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
        public abstract bool SendResponse(Socket socket);
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
        #endregion
    }
}
