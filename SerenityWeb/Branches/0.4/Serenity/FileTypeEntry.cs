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
        #region Fields - Private
        private string description;
		private string icon;
        private MimeType mimeType;
        private bool useCompression;
        #endregion
        #region Properties - Public
        public string Description
        {
            get
            {
                return this.description;
            }
			internal set
			{
				this.description = value;
			}
        }
		public string Icon
		{
			get
			{
				return this.icon;
			}
			internal set
			{
				this.icon = value;
			}
		}
        public MimeType MimeType
        {
            get
            {
                return this.mimeType;
            }
			internal set
			{
				this.mimeType = value;
			}
        }
        public bool UseCompression
        {
            get
            {
                return this.useCompression;
            }
			internal set
			{
				this.useCompression = value;
			}
        }
        #endregion
    }
}
