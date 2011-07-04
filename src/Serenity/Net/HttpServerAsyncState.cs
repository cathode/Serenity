/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Serenity.Web;
using System.Diagnostics.Contracts;

namespace Serenity.Net
{
    /// <summary>
    /// Provides a simple data structure used to pass objects to and from async
    /// callback methods.
    /// </summary>
    public class HttpServerAsyncState : IDisposable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServerAsyncState"/>
        /// class.
        /// </summary>
        /// <param name="connection">The <see cref="Socket"/> representing the connection with the remote client.</param>
        public HttpServerAsyncState(Socket connection)
        {
            Contract.Requires(connection != null);

            this.connection = connection;
            this.buffer = new NetworkBuffer();
            this.rawRequest = new StringBuilder();
            this.currentToken = new StringBuilder();
            this.stage = HttpRequestParseStep.Method;
            this.request = new Request()
            {
                Connection = this.Connection
            };
            this.response = new Response()
            {
                Connection = this.Connection
            };
        }
        #endregion
        #region Fields
        private  NetworkBuffer buffer;
        private Socket connection;
        private StringBuilder currentToken;
        private bool isDisposed;
        private string previousToken;
        private StringBuilder rawRequest;
        private Timer receiveTimer;
        private Request request;
        private Response response;
        private HttpRequestParseStep stage;
        private readonly object syncLock = new object();
        #endregion
        #region Methods
        /// <summary>
        /// Disposes the current <see cref="HttpServerAsyncState"/>.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    this.connection.Close();
                    this.receiveTimer.Dispose();
                }
                this.isDisposed = true;
            }
        }
        /// <summary>
        /// Disposes the current <see cref="HttpServerAsyncState"/>.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the <see cref="NetworkBuffer"/>.
        /// </summary>
        public NetworkBuffer Buffer
        {
            get
            {
                return this.buffer;
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="Socket"/> that represents the
        /// connection to the client.
        /// </summary>
        public Socket Connection
        {
            get
            {
                return this.connection;
            }
        }
        public StringBuilder CurrentToken
        {
            get
            {
                return this.currentToken;
            }
            set
            {
                this.currentToken = value ?? new StringBuilder();
            }
        }
        
        public bool IsDisposed
        {
            get
            {
                return this.isDisposed;
            }
        }
        public string PreviousToken
        {
            get
            {
                return this.previousToken ?? string.Empty;
            }
            set
            {
                this.previousToken = value;
            }
        }
        public StringBuilder RawRequest
        {
            get
            {
                return this.rawRequest;
            }
            set
            {
                this.rawRequest = value ?? new StringBuilder();
            }
        }
        public Timer ReceiveTimer
        {
            get
            {
                return this.receiveTimer;
            }
            set
            {
                this.receiveTimer = value;
            }
        }
        public Request Request
        {
            get
            {
                return this.request;
            }
        }
        public Response Response
        {
            get
            {
                return this.response;
            }
        }
        public HttpRequestParseStep Stage
        {
            get
            {
                return this.stage;
            }
            set
            {
                this.stage = value;
            }
        }
        public object SyncLock
        {
            get
            {
                return this.syncLock;
            }
        }
        #endregion
    }
}
