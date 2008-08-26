using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Web
{
    public sealed class Cookie
    {
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
    }
}
