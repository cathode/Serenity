/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Serenity.Web.Forms
{
    public sealed class RenderingContext
    {
        #region Constructors - Public
        public RenderingContext()
            : this(new MemoryStream(), Encoding.UTF8)
        {
        }
        public RenderingContext(Stream outputStream)
            : this(outputStream, Encoding.UTF8)
        {
        }
        public RenderingContext(Stream outputStream, Encoding outputEncoding)
        {
            this.outputEncoding = outputEncoding;
            this.outputStream = outputStream;
        }
        #endregion
        #region Fields - Private
        private Stream outputStream;
        private Encoding outputEncoding;
        #endregion
        #region Properties - Public
        public Stream OutputStream
        {
            get
            {
                return this.outputStream;
            }
            set
            {
                this.outputStream = value;
            }
        }
        public Encoding OutputEncoding
        {
            get
            {
                return this.outputEncoding;
            }
            set
            {
                this.outputEncoding = value;
            }
        }
        #endregion
    }
}
