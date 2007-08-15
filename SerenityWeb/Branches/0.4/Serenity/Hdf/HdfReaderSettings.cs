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
using System.Text;

namespace Serenity.Hdf
{
    /// <summary>
    /// Contains settings that are used by a HdfReader to determine how the HdfReader processes input.
    /// </summary>
    public sealed class HdfReaderSettings
    {
        #region Fields - Private
        private Encoding encoding;
        private int bufferSize;
        #endregion
        #region Fields - Public
        /// <summary>
        /// Holds the default buffer size.
        /// </summary>
        public const int DefaultBufferSize = 128;
        /// <summary>
        /// Holds a value that indicates if HDF "copies" are resolved during reading (true) or if they should be linked (false).
        /// </summary>
        public bool ResolveCopies;
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets or sets the buffer size to use for reading.
        /// </summary>
        public int BufferSize
        {
            get
            {
                //WS: Prevents infinite loops; if the buffer size would be less than 1 then the HdfReader would never process any data
                if (this.bufferSize < 1)
                {
                    return HdfReaderSettings.DefaultBufferSize;
                }
                else
                {
                    return this.bufferSize;
                }
            }
            set
            {
                this.bufferSize = value;
            }
        }
        /// <summary>
        /// Gets or sets the encoding used for reading.
        /// </summary>
        public Encoding Encoding
        {
            get
            {
                if (this.encoding == null)
                {
                    return Encoding.UTF8;
                }
                else
                {
                    return this.encoding;
                }
            }
            set
            {
                this.encoding = value;
            }
        }
        #endregion
    }
}
