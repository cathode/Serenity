/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Web
{
    /// <summary>
    /// Represents a web cookie used to maintain state with http.
    /// </summary>
    public sealed class Cookie
    {
        #region Fields
        private string comment;
        private Uri commentUri;
        private bool httpOnly;
        private Uri path;
        private string port; //TODO: Refactor to numeric type.
        private bool isSecure;
        private string name;
        private Uri domain;
        private DateTime expiresOn;
        private DateTime issuedOn;
        private bool isExpired;
        private bool isTemporary;
        private string value;
        private Version version;
        #endregion
        #region Methods
        /// <summary>
        /// Converts the current <see cref="Cookie"/> to it's string
        /// representation.
        /// </summary>
        /// <returns>A string containing a representation of the current
        /// <see cref="Cookie"/>.</returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder(this.Name + "=\"" + this.Value + "\"");

            if (!string.IsNullOrEmpty(this.Comment))
                result.AppendFormat(";Comment=\"{0}", this.Comment);
            if (this.CommentUri != null)
                result.AppendFormat(";CommentURL=\"{0}\"", this.CommentUri);
            if (this.IsTemporary)
                result.Append(";Discard");
            if (this.Domain != null)
                result.AppendFormat(";Domain=\"{0}\"", this.Domain);
            if (this.IsExpired)
                result.Append(";Max-Age=0");
            else
                result.AppendFormat(";Max-Age={0}", (this.ExpiresOn - DateTime.Now).Seconds);
            if (this.Path != null)
                result.AppendFormat(";Path=\"{0}\"", this.Path);
            if (this.Port != null)
                if (this.Port.Length == 0)
                    result.Append(";Port");
                else
                    result.AppendFormat(";Port=\"{0}\"", this.Port);
            if (this.IsSecure)
                result.Append(";Secure");

            result.Append(";Version=1");
            return result.ToString();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the comment associated with the cookie.
        /// </summary>
        public string Comment
        {
            get
            {
                return this.comment;
            }
            set
            {
                this.comment = value;
            }
        }

        /// <summary>
        /// Gets or sets the comment <see cref="Uri"/> associated with the cookie.
        /// </summary>
        public Uri CommentUri
        {
            get
            {
                return this.commentUri;
            }
            set
            {
                this.commentUri = value;
            }
        }

        /// <summary>
        /// Gets or sets the domain of the cookie.
        /// </summary>
        public Uri Domain
        {
            get
            {
                return this.domain;
            }
            set
            {
                this.domain = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> that determines the point when the browser should consider the cookie to be expired.
        /// </summary>
        public DateTime ExpiresOn
        {
            get
            {
                return this.expiresOn;
            }
            set
            {
                this.expiresOn = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cookie should only be used for HTTP communications.
        /// </summary>
        public bool HttpOnly
        {
            get
            {
                return this.httpOnly;
            }
            set
            {
                this.httpOnly = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cookie is expired.
        /// </summary>
        public bool IsExpired
        {
            get
            {
                return this.isExpired;
            }
            set
            {
                this.isExpired = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cookie is secure.
        /// </summary>
        public bool IsSecure
        {
            get
            {
                return this.isSecure;
            }
            set
            {
                this.isSecure = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> when the cookie was issued.
        /// </summary>
        public DateTime IssuedOn
        {
            get
            {
                return this.issuedOn;
            }
            set
            {
                this.issuedOn = value;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates if the client should discard the cookie when the session ends.
        /// </summary>
        public bool IsTemporary
        {
            get
            {
                return this.isTemporary;
            }
            set
            {
                this.isTemporary = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the cookie.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets the path of the cookie.
        /// </summary>
        public Uri Path
        {
            get
            {
                return this.path;
            }
            set
            {
                this.path = value;
            }
        }

        public string Port
        {
            get
            {
                return this.port;
            }
            set
            {
                this.port = value;
            }
        }
        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
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
