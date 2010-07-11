/* Serenity - The next evolution of web server technology.
 * Copyright © 2006-2010 Will Shelley. All Rights Reserved. */
using System;
using System.Text;

namespace Serenity.Core
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
        private CookieVersion version;
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets a comment documenting how the server intends to use the <see cref="Cookie"/>.
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
        /// Gets or sets indicating whether the client should discard the current <see cref="Cookie"/> when it exits.
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
        /// <summary>
        /// Gets or sets the value of the <see cref="Cookie"/>.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the <see cref="CookieVersion"/> indicating which specification governs the current <see cref="Cookie"/>.
        /// </summary>
        public CookieVersion Version
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
        #region Methods
        /// <summary>
        /// Converts the current <see cref="Cookie"/> to it's string representation.
        /// </summary>
        /// <returns>A <see cref="string"/> that is the string representation of the current <see cref="Cookie"/>.</returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder(this.Name ?? string.Empty + "=\"" + this.Value ?? string.Empty + "\"");

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

            if (this.Version == CookieVersion.RFC2965)
                result.Append(";Version=1");

            return result.ToString();
        }
        #endregion
    }
}
