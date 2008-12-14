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
