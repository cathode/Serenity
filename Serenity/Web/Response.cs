/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2015 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Serenity.Web
{
    /// <summary>
    /// Represents a response to a request from a client.
    /// </summary>
    public sealed class Response
    {
        #region Fields
        private Socket connection;
        private MimeType contentType;
        private HeaderCollection headers;
        private bool headersSent;
        private CookieCollection cookies;
        private List<byte> outputBuffer;
        private int sent;
        private StatusCode status;
        private bool useChunkedTransferEncoding;
        private bool useCompression;
        private bool isComplete;
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Response"/> class.
        /// </summary>
        public Response()
        {
            this.cookies = new CookieCollection();
            this.headers = new HeaderCollection();
            this.headersSent = false;
            this.contentType = MimeType.Default;
            this.outputBuffer = new List<byte>();
            this.sent = 0;
            this.status = StatusCode.Http500InternalServerError;
            this.useChunkedTransferEncoding = false;
            this.useCompression = false;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the <see cref="Socket"/> used to communicate the current <see cref="Response"/>.
        /// </summary>
        public Socket Connection
        {
            get
            {
                return this.connection;
            }
            set
            {
                this.connection = value;
            }
        }

        /// <summary>
        /// Gets or sets the mimetype associated with the content returned to the client.
        /// </summary>
        public MimeType ContentType
        {
            get
            {
                return this.contentType;
            }
            set
            {
                this.contentType = value;
            }
        }

        /// <summary>
        /// Gets a collection of cookies that will be sent with the request.
        /// </summary>
        public CookieCollection Cookies
        {
            get
            {
                return this.cookies;
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
        /// Gets or sets a value indicating whether header data has been sent to the client.
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
        /// Gets a value indicating whether the current <see cref="Response"/> is complete.
        /// </summary>
        public bool IsComplete
        {
            get
            {
                return this.isComplete;
            }
            set
            {
                this.isComplete = value;
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
        /// Gets or sets a value indicating whether the current <see cref="Response"/> should be sent using chunked transfer encoding.
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
        /// Gets or sets a value indicating whether the data sent back to the client with the response should be compressed or not.
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
        #region Methods - Public
        /// <summary>
        /// Clears the output buffer of the current <see cref="Response"/>.
        /// </summary>
        /// <remarks>
        /// If data has already been sent, this cannot un-send already sent data.
        /// </remarks>
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
            // TODO: Implement Response.Flush method.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes a series of bytes to the output buffer.
        /// </summary>
        /// <param name="value">The array of bytes to write.</param>
        /// <returns>The number of bytes written, or -1 if an error occurred.</returns>
        public int Write(byte[] value)
        {
            if (value != null)
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
        /// <returns>The number of bytes written.</returns>
        public int Write(string value)
        {
            if (value == null)
                return 0;

            return this.Write(Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// Writes a string followed by a newline to the output buffer.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The number of bytes written, including the newline characters.</returns>
        public int WriteLine(string value)
        {
            if (value == null)
                value = string.Empty;

            return this.Write(Encoding.UTF8.GetBytes(value + "\r\n"));
        }
        #endregion
    }
}
