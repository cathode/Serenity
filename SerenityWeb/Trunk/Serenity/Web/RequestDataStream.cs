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
using System.IO;
using System.Text;

namespace Serenity.Web
{
	/// <summary>
	/// Represents a request data field as a read-only stream. This class is read-only.
	/// </summary>
	public sealed class RequestDataStream : Stream
	{
		#region Constructors - Internal
		internal RequestDataStream(string name) : this(name, null)
		{
		}
		internal RequestDataStream(string name, byte[] contents)
		{
			this.name = name;
			if (contents != null)
			{
				this.contents = contents;
			}
			else
			{
				this.contents = new byte[0];
			}
		}
		#endregion
		#region Fields - Private
		private byte[] contents;
		private MimeType mimeType;
		private string name;
		private long position = 0;
		#endregion
		#region Methods - Public
		/// <summary>
		/// Not supported.
		/// </summary>
		/// <exception cref="NotSupportedException">Cannot flush a RequestDataStream.</exception>
		public override void Flush()
		{
			throw new NotSupportedException("Cannot flush a RequestDataStream.");
		}
		/// <summary>
		/// Reads a number of bytes into Buffer
		/// </summary>
		/// <param name="Buffer">An array of bytes. When this method returns, Buffer contains the specified
		///     Byte array with the values between Offset and (Offset + Count - 1) replaced
		///     by the bytes read from the current stream.</param>
		/// <param name="Offset">The zero-based index in Buffer at which to begin storing the read bytes.</param>
		/// <param name="Count">The maximum of bytes to read from the current stream.</param>
		/// <returns></returns>
		public override int Read(byte[] Buffer, int Offset, int Count)
		{
			int ReadCount = 0;
			int Index = Offset;
			while ((Index < Buffer.Length) || (ReadCount <= Count))
			{
				Buffer[ReadCount] = this.contents[Index];
				Index++;
				ReadCount++;
			}
			this.position += ReadCount;
			return ReadCount;
		}
		/// <summary>
		/// Reads the entire stream, starting at the beginning.
		/// </summary>
		/// <returns></returns>
		public byte[] ReadAll()
		{
			this.position = this.contents.Length - 1;
			byte[] Result = new byte[this.contents.Length];
			this.contents.CopyTo(Result, 0);
			return Result;
		}
		/// <summary>
		/// Reads the entire stream into a string, starting at the beginning and using the default text encoding.
		/// </summary>
		/// <returns></returns>
		public string ReadAllText()
		{
			this.position = this.contents.Length - 1;
			return Encoding.Default.GetString(this.contents);
		}
		/// <summary>
		/// Reads the entire stream into a string, starting at the beginning and using the specified text encoding.
		/// </summary>
		/// <param name="ReadEncoding">The encoding to read with.</param>
		/// <returns></returns>
		public string ReadAllText(Encoding ReadEncoding)
		{
			this.position = this.contents.Length - 1;
			return ReadEncoding.GetString(this.contents);
		}
		/// <summary>
		/// Reads a number of bytes and returns them as a string, using the default text encoding.
		/// </summary>
		/// <param name="Count">The number of bytes to read on the current stream.</param>
		/// <returns></returns>
		public string ReadText(int Count)
		{
			return this.ReadText(Count, Encoding.Default);
		}
		/// <summary>
		/// Reads a number of bytes and returns them as a string, using ReadEncoding as the text encoding.
		/// </summary>
		/// <param name="Count">The number of bytes to read on the current stream.</param>
		/// <param name="ReadEncoding">The text encoding to encode the read bytes with.</param>
		/// <returns></returns>
		public string ReadText(int Count, Encoding ReadEncoding)
		{
			byte[] Data = new byte[Count];
			this.Read(Data, 0, Count);
			return ReadEncoding.GetString(Data);
		}
		/// <summary>
		/// Reads all the remaining bytes and returns them as an array.
		/// </summary>
		/// <returns></returns>
		public byte[] ReadToEnd()
		{
			byte[] Result = new byte[this.Length - this.Position];
			for (int I = (int)this.Position, N = 0; I < this.Length; I++, N++)
			{
				Result[N] = this.contents[I];
			}
			return Result;
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
		/// string using ReadEncoding as the encoding.
		/// </summary>
		/// <param name="ReadEncoding">The encoding to use.</param>
		/// <returns>A string containing the remainder of the current RequestDataStream.</returns>
		public string ReadToEndText(Encoding ReadEncoding)
		{
			return ReadEncoding.GetString(this.ReadToEnd());
		}
		/// <summary>
		/// Sets the position within the current stream.
		/// </summary>
		/// <param name="Offset">A Byte offset relative to the origin parameter.</param>
		/// <param name="Origin">A Input of type System.IO.SeekOrigin indicating
		/// the reference point used to obtain the new position.</param>
		/// <returns>The new position within the current stream.</returns>
		public override long Seek(long Offset, SeekOrigin Origin)
		{
			switch (Origin)
			{
				case SeekOrigin.Begin:
					this.position = Offset;
					break;

				case SeekOrigin.Current:
					this.position += Offset;
					break;

				case SeekOrigin.End:
					this.position = this.contents.Length - Offset;
					break;
			}
			return 0;
		}
		/// <summary>
		/// Not supported.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// Cannot modify a RequestDataStream.
		/// </exception>
		public override void SetLength(long value)
		{
			throw new NotSupportedException("Cannot modify a RequestDataStream.");
		}
		/// <summary>
		/// Not supported.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// Cannot modify a RequestDataStream.
		/// </exception>
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("Cannot modify a RequestDataStream.");
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
		/// <remarks>Always returns false (RequestDataReader does not support writing).</remarks>
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