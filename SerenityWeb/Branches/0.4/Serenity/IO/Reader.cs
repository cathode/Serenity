/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Serenity.IO
{
    /// <summary>
    /// Provides an abstract base class for objects that read objects of a specific type from a stream.
    /// </summary>
    /// <typeparam name="T">The type of object that will be read.</typeparam>
    public abstract class Reader<T> : IDisposable
    {
        #region Fields - Protected
        private bool isReusable;
        #endregion
        #region Methods - Public
		/// <summary>
		/// Reads and returns a T from the supplied array of bytes.
		/// </summary>
		/// <param name="bytes">bytes to read a T from.</param>
		/// <returns>A T read from the supplied byte array.</returns>
		/// <remarks>The supplied stream must support reading.</remarks>
		public T Read(byte[] bytes)
		{
			bool b;
			return this.Read(bytes, out b);
		}
		/// <summary>
		/// Reads and returns a T from the supplied array of bytes,
		/// and indicates if the reading operation succeeded.
		/// </summary>
		/// <param name="bytes">The bytes to read a T from.</param>
		/// <param name="result">A boolean that indicates if the read operation suceeded (true) or failed (false).</param>
		/// <returns>A T read from the supplied byte array.</returns>
		/// <remarks>The supplied stream must support reading.</remarks>
		public virtual T Read(byte[] bytes, out bool result)
		{
			T value;
			if (bytes != null)
			{
				using (MemoryStream ms = new MemoryStream(bytes))
				{
					value = this.Read(ms, out result);
				}
				return value;
			}
			else
			{
				result = false;
				return default(T);
			}
		}
        /// <summary>
        /// Reads and returns a T from the supplied stream.
        /// </summary>
        /// <param name="stream">The stream to read the T from.</param>
		/// <returns>A T read from the supplied Stream.</returns>
		/// <remarks>The supplied stream must support reading.</remarks>
		public T Read(Stream stream)
		{
			bool b;
			return this.Read(stream, out b);
		}
		/// <summary>
		/// When overridden in a derived class, reads and returns a T from the supplied Stream.
		/// </summary>
		/// <param name="stream">The stream to read the T from.</param>
		/// <returns>A T read from the supplied Stream.</returns>
		/// <remarks>The supplied stream must support reading.</remarks>
		public abstract T Read(Stream stream, out bool result);
        /// <summary>
        /// When overridden in a derived class, performs cleanup and freeing
        /// of any unmanaged resources that may have been used by the current Reader.
        /// </summary>
        public virtual void Dispose()
        {
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets or sets a value which determines if the current Reader is reusable,
        /// e.g., multiple calls to the Read method can be made to the same Reader.
        /// </summary>
        public virtual bool IsReusable
        {
            get
            {
                return this.isReusable;
            }
            protected set
            {
                this.isReusable = value;
            }
        }
        #endregion
    }
}