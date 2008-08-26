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
        #region Fields - Private
        private string comment;
        private Uri commentUri;
        private bool httpOnly;
        private Uri path;
        private string port;
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
        #region Methods - Public
        public override string ToString()
        {
            string result = this.Name + "=\"" + this.Value + "\"";
            if (!string.IsNullOrEmpty(this.Comment))
            {
                result += ";Comment=\"" + this.Comment + "\"";
            }
            if (this.CommentUri != null)
            {
                result += ";CommentURL=\"" + this.CommentUri.ToString() + "\"";
            }
            if (this.IsTemporary)
            {
                result += ";Discard";
            }
            if (this.Domain != null)
            {
                result += ";Domain=\"" + this.Domain.ToString() + "\"";
            }
            if (this.IsExpired)
            {
                result += ";Max-Age=0";
            }
            else
            {
                result += ";Max-Age=" + (DateTime.Now - this.ExpiresOn).Seconds;
            }
            if (this.Path != null)
            {
                result += ";Path=\"" + this.Path.ToString() + "\"";
            }
            if (this.Port != null)
            {
                if (this.Port.Length == 0)
                {
                    result += ";Port";
                }
                else
                {
                    result += ";Port=\"" + this.Port + "\"";
                }
            }
            if (this.IsSecure)
            {
                result += ";Secure";
            }
            result += ";Version=1";
            return result;
        }
        #endregion
        #region Properties - Public
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
        /// Gets or sets a value that indicates if the client should discard the current <see cref="Cookie"/> when it exits.
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
