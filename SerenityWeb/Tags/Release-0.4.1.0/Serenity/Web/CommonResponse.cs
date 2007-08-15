/*
Serenity - The next evolution of web server technology

Copyright � 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
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
        private List<byte> outputBuffer = new List<byte>();
        private MimeType mimeType = MimeType.Default;
        private HeaderCollection headers = new HeaderCollection();
        private CommonContext context;
        private bool useCompression = false;
        private bool useChunkedTransferEncoding = false;
        #endregion
        #region Fields - Public
        public StatusCode Status;
        #endregion
        #region Methods - Public
		public void ClearOutputBuffer()
		{
			this.outputBuffer = new List<byte>();
		}
        /// <summary>
        /// Causes the currently buffered data to be written to the underlying client socket, then clears the Buffer.
        /// Note: The underlying Socket is unaffected if the current CommonResponse does not support chunked transmission.
        /// </summary>
        /// <returns>The number of bytes flushed, or -1 if an error occurred.</returns>
        public int Flush()
        {
            //if (this.context.Driver.WriteContext(this.context.Socket, this.context))
            //{

            //}
            return -1;
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
        #region Properties - Internal
        internal byte[] OutputBuffer
        {
            get
            {
                return this.outputBuffer.ToArray();
            }
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets or sets the mimetype associated with the content returned to the client.
        /// </summary>
        public MimeType MimeType
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
        /// Gets the CommonContext associated with the current CommonResponse.
        /// </summary>
        public CommonContext Owner
        {
            get
            {
                return this.context;
            }
        }
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