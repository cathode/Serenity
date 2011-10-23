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
    /// Represents a connection to a remote host, and provides some common
    /// high-level networking operations such as managing the sending and
    /// receiving of data.
    /// </summary>
    public abstract class Connection : IDisposable
    {
        #region Fields
        /// <summary>
        /// Holds the underlying socket used for transmission of request/response data.
        /// </summary>
        private readonly Socket socket;

        /// <summary>
        /// Backing field for the <see cref="IsDisposed"/> property.
        /// </summary>
        private bool isDisposed;

        private NetworkBuffer buffer;
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        /// <param name="socket"></param>
        protected Connection(Socket socket)
        {
            Contract.Requires(socket != null);

            this.socket = socket;
            this.buffer = new NetworkBuffer();
        }
        #endregion
        #region Events
        public event EventHandler<ResourceExecutionContextEventArgs> ContextPending;
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether the current <see cref="Connection"/> is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                return this.isDisposed;
            }
            protected set
            {
                this.isDisposed = value;
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Releases managed and unmanaged resources held by the current <see cref="Connection"/>,
        /// including the underlying network socket associated with this instance.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void ProcessBufferFrame(NetworkBufferFrame frame)
        {
            Contract.Requires(frame != null);

            try
            {
                if (frame.ContentSize > 0)
                    this.ProcessBufferContents(frame.Content, 0, frame.ContentSize);
            }
            finally
            {
                frame.Release();
            }
        }

        public void Run()
        {
            this.BeginReceiveNextFrame();
        }

        /// <summary>
        /// Performs protocol-specific processing of the entire buffer contents.
        /// </summary>
        /// <param name="buffer">The array of bytes to process. The entire array is processed.</param>
        protected virtual void ProcessBufferContents(byte[] buffer)
        {
            Contract.Assume(buffer != null);
            Contract.Assume(buffer.Length > 0);

            // Nothing to do with an empty buffer.
            if (buffer.Length > 0)
                this.ProcessBufferContents(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Performs protocol-specific processing of the buffer contents.
        /// </summary>
        /// <param name="buffer">The array of bytes to process.</param>
        /// <param name="startIndex">The index in <paramref name="buffer"/> at which to start processing.</param>
        /// <param name="count">The number of bytes from <paramref name="buffer"/> to process.</param>
        protected abstract void ProcessBufferContents(byte[] buffer, int startIndex, int count);

        /// <summary>
        /// Queues a <see cref="ResourceExecutionContext"/> that is ready to be processed.
        /// </summary>
        /// <param name="context">The <see cref="ResourceExecutionContext"/> to add to the pending queue.</param>
        protected void QueueNewPendingContext(ResourceExecutionContext context)
        {
            Contract.Requires(context != null);

            this.OnContextPending(new ResourceExecutionContextEventArgs
            {
                Context = context
            });
        }

        protected virtual void OnContextPending(ResourceExecutionContextEventArgs e)
        {
            if (this.ContextPending != null)
                this.ContextPending(this, e);
        }

        /// <summary>
        /// Releases unmanaged, and optionally, managed resources held by the current <see cref="Connection"/>,
        /// including the underlying network socket associated with this instance.
        /// </summary>
        /// <param name="disposing">true to release managed resources;
        /// otherwise only unmanaged resources will be released.</param>
        protected virtual void Dispose(bool disposing)
        {
        }

        protected void BeginReceiveNextFrame()
        {
            var frame = this.buffer.CheckOut();
            var buffer = frame.Content;
            this.socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, this.ReceiveCallback, frame);
        }

        protected virtual void ReceiveCallback(IAsyncResult result)
        {
            Contract.Requires(result != null);

            int recvd = this.socket.EndReceive(result);
            var frame = (NetworkBufferFrame)result.AsyncState;
            this.BeginReceiveNextFrame();
            if (recvd > 0)
            {
                frame.ContentSize = recvd;
                this.ProcessBufferFrame(frame);
            }
            else
                frame.Release();
        }
        #endregion

    }
}
