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

namespace Serenity.Themes
{
    /// <summary>
    /// Represents a theme; a collection of styling properties and display information
    /// that can be applied universally to any themeable resource.
    /// </summary>
    public sealed class Theme
    {
        #region Fields - Private
        private string author;
        private Uri url;
        private Version version;
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets or sets the name of the author of the current Theme.
        /// </summary>
        public string Author
        {
            get
            {
                return this.author;
            }
            set
            {
                this.author = value;
            }
        }
        /// <summary>
        /// Gets or sets the URL at which more information about the current
        /// Theme can be found.
        /// </summary>
        public Uri Url
        {
            get
            {
                return this.url;
            }
            set
            {
                this.url = value;
            }
        }
        /// <summary>
        /// Gets or sets the version information of the current Theme.
        /// </summary>
        public Version Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }
        #endregion
    }
}
