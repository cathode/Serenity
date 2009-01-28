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
        public ServerAsyncState()
        {
            this.buffer = new NetworkBuffer();
            this.Reset();
        }
        #endregion
        #region Fields
        private readonly NetworkBuffer buffer;
        private Socket client;
        private Socket listener;
        private bool isDisposed;
        private Server owner;
        private Timer receiveTimer;
        private RequestStep stage;
        private StringBuilder rawRequest;
        private StringBuilder currentToken;
        private Request request;
        private Response response;
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
                    this.client = null;
                    this.listener = null;
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
            this.request = new Request()
            {
                Connection = this.Client
            };
            this.response = new Response()
            {
                Connection = this.Client
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
        public Socket Client
        {
            get
            {
                return this.client;
            }
            set
            {
                this.client = value;
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="Socket"/> that accepted the client
        /// connection.
        /// </summary>
        public Socket Listener
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
        #endregion
    }
}
