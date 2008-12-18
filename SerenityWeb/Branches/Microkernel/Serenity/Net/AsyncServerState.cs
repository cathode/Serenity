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
using System.Threading;
using SerenityProject.Common;

namespace Serenity.Net
{
    /// <summary>
    /// Provides a simple data structure used to pass objects to and from async callback methods.
    /// </summary>
    public sealed class AsyncServerState : Disposable
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncServerState"/>
        /// class using the default buffer size.
        /// </summary>
        public AsyncServerState()
            : this(AsyncServerState.DefaultBufferSize)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncServerState"/>
        /// class using the specified buffer size.
        /// </summary>
        /// <param name="bufferSize"></param>
        public AsyncServerState(int bufferSize)
        {
            if (bufferSize > AsyncServerState.MaxBufferSize || bufferSize < AsyncServerState.MinBufferSize)
            {
                throw new ArgumentOutOfRangeException(string.Format("Paramater 'bufferSize' must be between {0} and {1}", AsyncServerState.MinBufferSize, AsyncServerState.MaxBufferSize));
            }
            this.buffer = new byte[bufferSize];
        }
        #endregion
        #region Fields - Private
        private byte[] buffer;
        private ManualResetEvent signal = new ManualResetEvent(false);
        private Socket client;
        private Socket listener;
        #endregion
        #region Fields - Public
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
        public const int DefaultBufferSize = 256;
        #endregion
        #region Methods
        /// <summary>
        /// Disposes the current <see cref="AsyncServerState"/>
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            (this.signal as IDisposable).Dispose();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the data buffer associated with the current <see cref="AsyncServerState"/>.
        /// </summary>
        public byte[] Buffer
        {
            get
            {
                return this.buffer;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else if (value.Length > AsyncServerState.MaxBufferSize || value.Length < AsyncServerState.MinBufferSize)
                {
                    throw new ArgumentOutOfRangeException("Argument 'value' must be a byte[] with a length between "
                        + AsyncServerState.MinBufferSize + " and " + AsyncServerState.MaxBufferSize + ".", "value");
                }
                this.buffer = value;
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="TcpClient"/> that represents the connection to the client.
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
        /// Gets or sets the <see cref="TcpListener"/> that accepted the client connection.
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
        #endregion
    }
}
