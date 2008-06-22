using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Web
{
    public static class Response
    {       
        #region Fields - Private
        [ThreadStatic]
        private static HeaderCollection headers;
        [ThreadStatic]
        private static bool headersSent;
        [ThreadStatic]
        private static bool isInitialized;
        [ThreadStatic]
        private static MimeType mimeType;
        [ThreadStatic]
        private static List<byte> outputBuffer;
        [ThreadStatic]
        private static int sent;
        [ThreadStatic]
        private static StatusCode status;
        [ThreadStatic]
        private static bool useChunkedTransferEncoding;
        [ThreadStatic]
        private static bool useCompression;
        #endregion
        #region Methods - Public
        public static void ClearOutputBuffer()
        {
            Response.outputBuffer.Clear();
        }
        /// <summary>
        /// Causes the currently buffered data to be written to the underlying client socket, then clears the Buffer.
        /// Note: The underlying Socket is unaffected if the current CommonResponse does not support chunked transmission.
        /// </summary>
        /// <returns>The number of bytes flushed, or -1 if an error occurred.</returns>
        public static int Flush()
        {
            throw new NotImplementedException();
        }
        public static void Initialize()
        {
            if (!Response.isInitialized)
            {
                Response.headers = new HeaderCollection();
                Response.headersSent = false;
                Response.mimeType = MimeType.Default;
                Response.outputBuffer = new List<byte>();
                Response.sent = 0;
                Response.status = StatusCode.Http500InternalServerError;
                Response.useChunkedTransferEncoding = false;
                Response.useCompression = false;

                Response.isInitialized = true;
            }
        }
        public static void Reset()
        {
            Response.isInitialized = false;
            Response.Initialize();
        }
        /// <summary>
        /// Writes a series of bytes to the output buffer.
        /// </summary>
        /// <param name="value">The array of bytes to write.</param>
        /// <returns>The number of bytes written, or -1 if an error occurred.</returns>
        public static int Write(byte[] value)
        {
            if (value != null)
            {
                Response.outputBuffer.AddRange(value);
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
        public static int Write(string value)
        {
            return Response.Write(Encoding.UTF8.GetBytes(value));
        }
        /// <summary>
        /// Writes a string followed by a newline to the output buffer.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int WriteLine(string value)
        {
            return Response.Write(Encoding.UTF8.GetBytes(value + "\r\n"));
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets or sets the mimetype associated with the content returned to the client.
        /// </summary>
        public static MimeType ContentType
        {
            get
            {
                return Response.mimeType;
            }
            set
            {
                Response.mimeType = value;
            }
        }
        /// <summary>
        /// Gets the HeaderCollection containing the headers returned to the client.
        /// </summary>
        public static HeaderCollection Headers
        {
            get
            {
                return Response.headers;
            }
        }
        /// <summary>
        /// Gets an indication of whether or not header data has already been
        /// sent to the client.
        /// </summary>
        public static bool HeadersSent
        {
            get
            {
                return Response.headersSent;
            }
            set
            {
                Response.headersSent = value;
            }
        }
        /// <summary>
        /// Gets the buffer of data that has not yet been sent.
        /// </summary>
        public static List<byte> OutputBuffer
        {
            get
            {
                return Response.outputBuffer;
            }
        }
        /// <summary>
        /// Gets or sets a value which indicates how much data has actually
        /// been sent to the client.
        /// </summary>
        public static int Sent
        {
            get
            {
                return Response.sent;
            }
            set
            {
                Response.sent = value;
            }
        }
        /// <summary>
        /// Gets or sets the StatusCode associated with the current
        /// CommonResponse.
        /// </summary>
        public static StatusCode Status
        {
            get
            {
                return Response.status;
            }
            set
            {
                Response.status = value;
            }
        }
        /// <summary>
        /// Gets or sets a value which determines if the current CommonResponse
        /// should be sent using chunked transfer encoding.
        /// </summary>
        public static bool UseChunkedTransferEncoding
        {
            get
            {
                return Response.useChunkedTransferEncoding;
            }
            set
            {
                Response.useChunkedTransferEncoding = value;
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
        public static bool UseCompression
        {
            get
            {
                return Response.useCompression;
            }
            set
            {
                Response.useCompression = value;
            }
        }
        
        #endregion
    }
}
