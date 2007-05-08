/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://serenityproject.net/)

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

namespace Serenity
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
        /// When overridden in a derived class, reads and returns a T from the supplied Stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>A T object read from the supplied Stream.</returns>
        /// <remarks>
        /// The supplied stream must support reading.
        /// </remarks>
        public abstract T Read(Stream stream);
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
        #region IDisposable Members
        /// <summary>
        /// When overridden in a derived class, performs cleanup and freeing
        /// of any unmanaged resources that may have been used by the current Reader.
        /// </summary>
        public abstract void Dispose();
        #endregion
    }
}