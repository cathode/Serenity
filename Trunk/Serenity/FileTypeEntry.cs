/*
Serenity - The next evolution of web server technology

Copyright � 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity
{
    public struct FileTypeEntry
    {
        internal FileTypeEntry(string description, string mimeType, bool compress)
        {
            this.Compress = compress;
            this.Description = description;
            this.MimeType = mimeType;
        }
        public readonly string Description;
        public readonly string MimeType;
        public readonly bool Compress;
    }
}
