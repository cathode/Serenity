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
    public enum HdfFormat
    {
        Simple,
        Nested,
        Hybrid,
        OptimizedHybrid,
    }
    public sealed class HdfWriterSettings
    {
        #region Fields - Private
        private Encoding encoding = Encoding.UTF8;
        private HdfFormat format = HdfFormat.Simple;
        #endregion
        #region Properties - Public
        public Encoding Encoding
        {
            get
            {
                return this.encoding;
            }
            set
            {
                this.encoding = value;
            }
        }
        public HdfFormat Format
        {
            get
            {
                return this.format;
            }
            set
            {
                this.format = value;
            }
        }
        #endregion
    }
}
