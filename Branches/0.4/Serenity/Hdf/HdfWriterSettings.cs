/*
Serenity - The next evolution of web server technology
Serenity/Hdf/IO/HdfWriterSettings.cs
Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
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
