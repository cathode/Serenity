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
    /// Provides an abstract base class for objects that write objects of a specific type to a stream.
    /// </summary>
    /// <typeparam name="T">The type of object that will be written.</typeparam>
    public abstract class Writer<T> : IDisposable
    {
        #region Fields - Protected
        private bool isReusable;
        #endregion
        #region Methods - Public
        /// <summary>
        /// When overridden in a derived class, writes the supplied T to the supplied Stream.
        /// </summary>
        /// <param name="stream">The stream to be written to.</param>
        /// <param name="obj">The object to write to the stream.</param>
        /// <remarks>
        /// The supplied Stream must support writing.
        /// </remarks>
        public abstract void Write(Stream stream, T obj);
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets or sets a value which determines if the current Writer is reusable,
        /// e.g., multiple calls to the Write method can be made to the same Reader.
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
        /// of any unmanaged resources that may have been used by the current Writer.
        /// </summary>
        public abstract void Dispose();
        #endregion
    }
}
