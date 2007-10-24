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
    /// <summary>
    /// Provides a mechanism for recieving and responding to requests from clients (browsers).
    /// </summary>
    public abstract class WebDriver : IDisposable
    {
        #region Constructors - Protected
        /// <summary>
        /// Initializes a new instance of the WebDriver class.
        /// </summary>
        /// <param name="contextHandler">A ContextHandler which handles
        /// incoming CommonContext objects.</param>
        protected WebDriver(WebDriverSettings settings)
        {
            this.settings = settings;
        }
        #endregion
        #region Fields - Private
        private bool isDisposed = false;
        private DriverInfo info;
        private Socket listeningSocket;
        private WebDriverSettings settings;
        private WebDriverStatus status = WebDriverStatus.None;
        #endregion
        #region Methods - Protected
        protected virtual void AcceptCallback(IAsyncResult ar)
        {
            if (ar.AsyncState.GetType().TypeHandle.Equals(typeof(WebDriverState).TypeHandle))
            {
                WebDriverState state = (WebDriverState)ar.AsyncState;
                Socket workSocket = state.WorkSocket;
                Socket socket = workSocket.EndAccept(ar);
                workSocket.BeginAccept(new AsyncCallback(this.AcceptCallback), state);

                //this.HandleAcceptedSocket(socket);
            }
            else
            {
                return;
            }

        }
        /// <summary>
        /// Provides a callback method to use for an async socket disconnection.
        /// </summary>
        /// <param name="ar"></param>
        protected virtual void DisconnectCallback(IAsyncResult ar)
        {
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
        protected virtual void RecieveCallback(IAsyncResult ar)
        {
            if (ar.AsyncState.GetType().TypeHandle.Equals(typeof(WebDriverState).TypeHandle))
            {
                WebDriverState state = ar.AsyncState as WebDriverState;
                state.WorkSocket.EndReceive(ar);
            }
        }
        protected virtual void SendCallback(IAsyncResult ar)
        {
            if (ar.AsyncState.GetType().TypeHandle.Equals(typeof(WebDriverState).TypeHandle))
            {
                WebDriverState state = ar.AsyncState as WebDriverState;
                state.WorkSocket.EndSend(ar);
            }
        }
        /// <summary>
        /// Checks if the current WebDriver is disposed and throws an
        /// ObjectDisposedException if necessary.
        /// </summary>
        protected void CheckDisposal()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }
        #endregion
        #region Methods - Public
        public virtual IAsyncResult BeginRecieveContext(Socket socket, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }
        public virtual IAsyncResult BeginSendContext(Socket socket, CommonContext context, AsyncCallback callback, object state)
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
        /// <summary>
        /// Creates and binds the listening socket, preparing the WebDriver so that it can be started.
        /// </summary>
        public virtual bool Initialize()
        {
            if (this.status < WebDriverStatus.Initialized)
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
                    this.status = WebDriverStatus.Initialized;
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
        public abstract CommonContext RecieveContext(Socket socket);
        /// <summary>
        /// Starts the WebDriver.
        /// </summary>
        public virtual bool Start()
        {
            if (this.Status >= WebDriverStatus.Initialized)
            {
                this.Status = WebDriverStatus.Started;
                this.ListeningSocket.Listen(10);


                while (this.Status == WebDriverStatus.Started)
                {
                    this.ListeningSocket.Accept();
                    //this.HandleAcceptedSocket();
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
        public virtual bool Stop()
        {
            if (this.status == WebDriverStatus.Started)
            {
                this.status = WebDriverStatus.Stopped;
                return true;
            }
            else
            {
                return false;
            }
        }
        public abstract bool SendContext(Socket socket, CommonContext context);
        #endregion
        #region Properties - Protected
        protected Socket ListeningSocket
        {
            get
            {
                return this.listeningSocket;
            }
            set
            {
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
                if (this.status >= WebDriverStatus.Initialized)
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
                if (this.status == WebDriverStatus.Started)
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
                if (this.status == WebDriverStatus.Stopped)
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
        public ushort ListeningPort
        {
            get
            {
                this.CheckDisposal();

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
        public WebDriverStatus Status
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
