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
using System.Text;

using Serenity.Web;

namespace Serenity
{
    public struct FileTypeEntry
    {
        #region Constructors - Internal
        internal FileTypeEntry(string description, MimeType mimeType, bool compress)
        {
            this.useCompression = compress;
            this.description = description;
            this.mimeType = mimeType;
        }
        #endregion
        #region Fields - Private
        private readonly string description;
        private readonly MimeType mimeType;
        private readonly bool useCompression;
        #endregion
        #region Properties - Public
        public string Description
        {
            get
            {
                return this.description;
            }
        }
        public MimeType MimeType
        {
            get
            {
                return this.mimeType;
            }
        }
        public bool UseCompression
        {
            get
            {
                return this.useCompression;
            }
        }
        #endregion
    }
}
