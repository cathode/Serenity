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
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Serenity.Web;

namespace Serenity.Net
{
    /// <summary>
    /// Provides a simple data structure used to pass objects to and from async
    /// callback methods.
    /// </summary>
    public class ServerAsyncState : IDisposable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerAsyncState"/>
        /// class.
        /// </summary>
        /// <param name="connection">The <see cref="Socket"/> representing the connection with the remote client.</param>
        public ServerAsyncState(Socket connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            this.connection = connection;
            this.buffer = new NetworkBuffer(); 
        }
        #endregion
        #region Fields
        private readonly NetworkBuffer buffer;
        private readonly Socket connection;
        private StringBuilder currentToken;
        private bool isDisposed;
        private Server owner;
        
    
        private string previousToken;
        private StringBuilder rawRequest;
        private Timer receiveTimer;
        
        private Request request;
        private Response response;
        private RequestStep stage;
        private readonly object syncLock = new object();
        #endregion
        #region Methods
        /// <summary>
        /// Disposes the current <see cref="ServerAsyncState"/>.
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
                    this.owner = null;
                }
                this.isDisposed = true;
            }
        }
        /// <summary>
        /// Disposes the current <see cref="ServerAsyncState"/>.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Resets the current <see cref="ServerAsyncState"/>, preparing it for a new request from a client.
        /// </summary>
        public void Reset()
        {
            this.rawRequest = new StringBuilder();
            this.currentToken = new StringBuilder();
            this.stage = RequestStep.Method;
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
        public Server Owner
        {
            get
            {
                return this.owner;
            }
            set
            {
                this.owner = value;
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
        public RequestStep Stage
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
