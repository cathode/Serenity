/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
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
		private bool lockFlushes = false;
		private bool lockWrites = false;
        private MimeType mimeType = MimeType.Default;
		private List<byte> outputBuffer = new List<byte>();
		private bool useChunkedTransferEncoding = false;
        private bool useCompression = false;
        #endregion
        #region Fields - Public
        public StatusCode Status;
        #endregion
		#region Methods - Internal
		/// <summary>
		/// Prevents the output buffer from being flushed.
		/// </summary>
		/// <param name="lockWrites">Determines if writes are prevented in addition to flushes.</param>
		internal void LockOutputBuffer(bool lockWrites)
		{
			this.lockFlushes = true;
			this.lockWrites = lockWrites;
		}
		/// <summary>
		/// Performs the opposite of CommonResponse.LockOutputBuffer(bool).
		/// </summary>
		internal void UnlockOutputBuffer()
		{
			this.lockFlushes = false;
			this.lockWrites = false;
		}
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
				//if (this.context.Driver.WriteContext(this.context.Socket, this.context))
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