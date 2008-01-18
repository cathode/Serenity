﻿/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.IO;
using System.Text;

namespace Serenity.Web
{
    /// <summary>
    /// Represents a request data field as a read-only stream. This class is read-only.
    /// </summary>
    public sealed class RequestDataStream : Stream
    {
        #region Constructors - Public
        [Obsolete]
        public RequestDataStream(string name)
            : this(name, new byte[0])
        {
        }
        public RequestDataStream(string name, byte[] contents)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else if (name == string.Empty)
            {
                throw new ArgumentException(__Strings.ArgumentCannotBeEmpty, "name");
            }
            else if (contents == null)
            {
                throw new ArgumentNullException("contents");
            }
            this.name = name;
            this.contents = contents;
        }
        #endregion
        #region Fields - Private
        private byte[] contents;
        private MimeType mimeType;
        private string name;
        private long position = 0;
        private RequestMethod method = RequestMethod.GET;
        #endregion
        #region Methods - Public
        /// <summary>
        /// Not supported.
        /// </summary>
        /// <exception cref="NotSupportedException">Cannot flush a RequestDataStream.</exception>
        public override void Flush()
        {
            throw new NotSupportedException(__Strings.CannotFlushRequestDataStream);
        }
        /// <summary>
        /// Reads a number of bytes into Buffer
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns,
        /// buffer contains the values between offset and (offset + count - 1) replaced
        /// by the bytes read from the current stream.</param>
        /// <param name="offset">The zero-based index in Buffer at which to begin storing the read bytes.</param>
        /// <param name="count">The maximum of bytes to read from the current stream.</param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            else if (offset < 0 || offset > this.Length)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            else if (count + offset > this.Length)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            int readCount = 0;
            int index = offset;
            while ((index < buffer.Length) || (readCount <= count))
            {
                buffer[readCount] = this.contents[index];
                index++;
                readCount++;
            }
            this.position += readCount;
            return readCount;
        }
        /// <summary>
        /// Reads the entire stream, starting at the beginning.
        /// </summary>
        /// <returns></returns>
        public byte[] ReadAll()
        {
            this.position = this.contents.Length - 1;
            byte[] result = new byte[this.contents.Length];
            this.contents.CopyTo(result, 0);
            return result;
        }
        /// <summary>
        /// Reads the entire stream into a string, starting at the beginning
        /// and using the default text encoding.
        /// </summary>
        /// <returns></returns>
        public string ReadAllText()
        {
            this.position = this.contents.Length - 1;
            return Encoding.Default.GetString(this.contents);
        }
        /// <summary>
        /// Reads the entire stream into a string, starting at the beginning
        /// and using the specified text encoding.
        /// </summary>
        /// <param name="encoding">The encoding to use.</param>
        /// <returns></returns>
        public string ReadAllText(Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            this.position = this.contents.Length - 1;
            return encoding.GetString(this.contents);
        }
        /// <summary>
        /// Reads a number of bytes and returns them as a string, using the default text encoding.
        /// </summary>
        /// <param name="count">The number of bytes to read on the current stream.</param>
        /// <returns></returns>
        public string ReadText(int count)
        {
            return this.ReadText(count, Encoding.Default);
        }
        /// <summary>
        /// Reads a number of bytes and returns them as a string, using the specified encoding.
        /// </summary>
        /// <param name="count">The number of bytes to read on the current stream.</param>
        /// <param name="encoding">The encoding to use.</param>
        /// <returns></returns>
        public string ReadText(int count, Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            else if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            byte[] result = new byte[count];
            this.Read(result, 0, count);
            return encoding.GetString(result);
        }
        /// <summary>
        /// Reads all the remaining bytes and returns them as an array.
        /// </summary>
        /// <returns></returns>
        public byte[] ReadToEnd()
        {
            byte[] result = new byte[this.Length - this.Position];
            for (int I = (int)this.Position, N = 0; I < this.Length; I++, N++)
            {
                result[N] = this.contents[I];
            }
            return result;
        }
        /// <summary>
        /// Reads to the end of the current RequestDataStream,
        /// from the current position, and returns everything read as a
        /// string using the default encoding.
        /// </summary>
        /// <returns>A string containing the remainder of the current RequestDataStream.</returns>
        public string ReadToEndText()
        {
            return this.ReadToEndText(Encoding.Default);
        }
        /// <summary>
        /// Reads to the end of the current RequestDataStream,
        /// from the current position, and returns everything read as a
        /// string using the specified encoding.
        /// </summary>
        /// <param name="encoding">The encoding to use.</param>
        /// <returns>A string containing the remainder of the current RequestDataStream.</returns>
        public string ReadToEndText(Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            return encoding.GetString(this.ReadToEnd());
        }
        /// <summary>
        /// Sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A Byte offset relative to the origin parameter.</param>
        /// <param name="origin">A Input of type System.IO.SeekOrigin indicating
        /// the reference point used to obtain the new position.</param>
        /// <returns>The new position within the current stream.</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    this.position = offset;
                    break;

                case SeekOrigin.Current:
                    this.position += offset;
                    break;

                case SeekOrigin.End:
                    this.position = this.contents.Length - offset;
                    break;
            }
            return this.position;
        }
        /// <summary>
        /// Not supported.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// Cannot modify a RequestDataStream.
        /// </exception>
        public override void SetLength(long value)
        {
            throw new NotSupportedException(__Strings.CannotModifyRequestDataStream);
        }
        /// <summary>
        /// Not supported.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// Cannot modify a RequestDataStream.
        /// </exception>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException(__Strings.CannotModifyRequestDataStream);
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets a Input that indicates whether the current RequestDataReader supports reading.
        /// </summary>
        /// <remarks>Always returns true.</remarks>
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// Gets a Input that indicates whether the current RequestDataReader supports seeking.
        /// </summary>
        /// <remarks>Always returns true.</remarks>
        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// Gets a Input that indicates whether the current RequestDataReader supports writing.
        /// </summary>
        /// <remarks>Always returns false.</remarks>
        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// Gets the length in bytes of the current stream.
        /// </summary>
        public override long Length
        {
            get
            {
                return this.contents.LongLength;
            }
        }
        /// <summary>
        /// Gets or sets the mime-type associated with the current RequestDataStream.
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
        /// Gets the name of the request data field which the current RequestDataReader provides access to.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
            internal set
            {
                this.name = value;
            }
        }
        public RequestMethod Method
        {
            get
            {
                return this.method;
            }
            internal set
            {
                this.method = value;
            }
        }
        /// <summary>
        /// Gets or sets the position within the current stream.
        /// </summary>
        public override long Position
        {
            get
            {
                return this.position;
            }
            set
            {
                if (value >= 0)
                {
                    this.position = value;
                }
            }
        }
        #endregion
    }
}