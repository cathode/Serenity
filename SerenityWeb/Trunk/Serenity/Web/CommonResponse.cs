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

using Serenity.Web.Drivers;

namespace Serenity.Web
{
    /// <summary>
    /// Represents a universal response to a request made by a client.
    /// </summary>
    public sealed class CommonResponse
    {
        #region Constructors - Internal
        internal CommonResponse(CommonContext context)
        {
            this.context = context;
        }
        #endregion
        #region Fields - Private
        private CommonContext context;
        private HeaderCollection headers = new HeaderCollection();
        private bool headersSent = false;
        private bool lockFlushes = false;
        private bool lockWrites = false;
        private MimeType mimeType = MimeType.Default;
        private List<byte> outputBuffer = new List<byte>();
        private int sent = 0;
        private StatusCode status = StatusCode.Http500InternalServerError;
        private bool useChunkedTransferEncoding = false;
        private bool useCompression = false;
        #endregion
        #region Methods - Public
        public void ClearOutputBuffer()
        {
            this.outputBuffer.Clear();
        }
        /// <summary>
        /// Causes the currently buffered data to be written to the underlying client socket, then clears the Buffer.
        /// Note: The underlying Socket is unaffected if the current CommonResponse does not support chunked transmission.
        /// </summary>
        /// <returns>The number of bytes flushed, or -1 if an error occurred.</returns>
        public int Flush()
        {
            if (!this.lockFlushes)
            {
                //if (this.context.Driver.SendContext(this.context.Socket, this.context))
                //{

                //}
                //not implemented yet.
                return -1;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// Writes a series of bytes to the output buffer.
        /// </summary>
        /// <param name="value">The array of bytes to write.</param>
        /// <returns>The number of bytes written, or -1 if an error occurred.</returns>
        public int Write(byte[] value)
        {
            if ((value != null) && (!this.lockWrites))
            {
                this.outputBuffer.AddRange(value);
                return value.Length;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// Writes a string to the output buffer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <returns></returns>
        public int Write(string value)
        {
            return this.Write(Encoding.UTF8.GetBytes(value));
        }
        /// <summary>
        /// Writes a string followed by a newline to the output buffer.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int WriteLine(string value)
        {
            return this.Write(Encoding.UTF8.GetBytes(value + "\r\n"));
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets or sets the mimetype associated with the content returned to the client.
        /// </summary>
        public MimeType ContentType
        {
            get
            {
                return this.mimeType;
            }
            set
            {
                this.mimeType = value;
            }
        }
        /// <summary>
        /// Gets the HeaderCollection containing the headers returned to the client.
        /// </summary>
        public HeaderCollection Headers
        {
            get
            {
                return this.headers;
            }
        }
        /// <summary>
        /// Gets an indication of whether or not header data has already been
        /// sent to the client.
        /// </summary>
        public bool HeadersSent
        {
            get
            {
                return this.headersSent;
            }
            set
            {
                this.headersSent = value;
            }
        }
        /// <summary>
        /// Gets the buffer of data that has not yet been sent.
        /// </summary>
        public List<byte> OutputBuffer
        {
            get
            {
                return this.outputBuffer;
            }
        }
        /// <summary>
        /// Gets the CommonContext associated with the current CommonResponse.
        /// </summary>
        public CommonContext Context
        {
            get
            {
                return this.context;
            }
        }
        /// <summary>
        /// Gets or sets a value which indicates how much data has actually
        /// been sent to the client.
        /// </summary>
        public int Sent
        {
            get
            {
                return this.sent;
            }
            set
            {
                this.sent = value;
            }
        }
        /// <summary>
        /// Gets or sets the StatusCode associated with the current
        /// CommonResponse.
        /// </summary>
        public StatusCode Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }
        /// <summary>
        /// Gets or sets a value which determines if the current CommonResponse
        /// should be sent using chunked transfer encoding.
        /// </summary>
        public bool UseChunkedTransferEncoding
        {
            get
            {
                return this.useChunkedTransferEncoding;
            }
            set
            {
                this.useChunkedTransferEncoding = value;
            }
        }
        /// <summary>
        /// Gets or sets a value used to determine if the data sent back
        /// to the client with the response should be compressed or not.
        /// </summary>
        /// <remarks>
        /// Compression can decrease the amount of data to be sent by a large
        /// amount if used on highly compressable data such as text. For other
        /// types of data, like images, compression should not be used.
        /// </remarks>
        public bool UseCompression
        {
            get
            {
                return this.useCompression;
            }
            set
            {
                this.useCompression = value;
            }
        }
        
        #endregion
    }
}