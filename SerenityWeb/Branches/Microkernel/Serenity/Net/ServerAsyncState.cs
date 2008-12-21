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
using System.Net.Sockets;
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
        /// Initializes a new instance of the <see cref="AsyncServerState"/>
        /// class using the default buffer size.
        /// </summary>
        public ServerAsyncState()
            : this(ServerAsyncState.DefaultBufferSize)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncServerState"/>
        /// class using the specified buffer size.
        /// </summary>
        /// <param name="bufferSize"></param>
        public ServerAsyncState(int bufferSize)
        {
            if (bufferSize > ServerAsyncState.MaxBufferSize || bufferSize < ServerAsyncState.MinBufferSize)
            {
                throw new ArgumentOutOfRangeException(string.Format("Paramater 'bufferSize' must be between {0} and {1}", ServerAsyncState.MinBufferSize, ServerAsyncState.MaxBufferSize));
            }
            this.receiveBuffer = new byte[bufferSize];
        }
        #endregion
        #region Fields
        private byte[] receiveBuffer;
        private Queue<byte> dataBuffer = new Queue<byte>();
        private Socket client;
        private Socket listener;
        private Request request = new Request();
        private Response response = new Response();
        private bool isDisposed;
        private Server owner;
        /// <summary>
        /// Holds the maximum buffer size.
        /// </summary>
        public const int MaxBufferSize = 65536;
        /// <summary>
        /// Holds the minimum buffer size.
        /// </summary>
        public const int MinBufferSize = 1;
        /// <summary>
        /// Holds the default (optimal) buffer size.
        /// </summary>
        public const int DefaultBufferSize = 512;
        #endregion
        #region Methods
        /// <summary>
        /// Disposes the current <see cref="ServerAsyncState"/>
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            this.client = null;
            this.listener = null;
            this.dataBuffer = null;
            this.receiveBuffer = null;
        }
        public void Dispose()
        {
            if (!this.IsDisposed)
            {
                this.Dispose(true);
                this.isDisposed = true;
            }
        }
        public void SwapBuffers()
        {
            for (int i = 0; i < this.ReceiveBuffer.Length; i++)
            {
                this.dataBuffer.Enqueue(this.ReceiveBuffer[i]);
            }
            this.ReceiveBuffer = new byte[this.ReceiveBuffer.Length];
        }
        public virtual void Reset()
        {
            this.request = new Request()
            {
                Connection = this.Client,
                Owner = this.Owner
            };
            this.response = new Response()
            {
                Connection = this.Client,
                Owner = this.Owner
            };
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the data buffer associated with the current
        /// <see cref="ServerAsyncState"/>.
        /// </summary>
        public byte[] ReceiveBuffer
        {
            get
            {
                return this.receiveBuffer;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else if (value.Length > ServerAsyncState.MaxBufferSize || value.Length < ServerAsyncState.MinBufferSize)
                {
                    throw new ArgumentOutOfRangeException("Argument 'value' must be a byte[] with a length between "
                        + ServerAsyncState.MinBufferSize + " and " + ServerAsyncState.MaxBufferSize + ".", "value");
                }
                this.receiveBuffer = value;
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
        /// Gets a queue of bytes that represents data that has not been processed.
        /// </summary>
        public Queue<byte> DataBuffer
        {
            get
            {
                return this.dataBuffer;
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
        public Request Request
        {
            get
            {
                return this.request;
            }
            set
            {
                this.request = value;
            }
        }
        public Response Response
        {
            get
            {
                return this.response;
            }
            set
            {
                this.response = value;
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
        #endregion
    }
}
