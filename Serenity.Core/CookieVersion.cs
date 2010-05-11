using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Core
{
    /// <summary>
    /// Enumerates supported HTTP cookie specification versions.
    /// </summary>
    public enum CookieVersion
    {
        /// <summary>
        /// Indicates the older Set-Cookie specification based on "RFC 2109: HTTP State Management Mechanism".
        /// </summary>
        RFC2109,

        /// <summary>
        /// Indicates the newer (current) Set-Cookie2 specification based on "RFC 2965: HTTP State Management Mechanism".
        /// </summary>
        RFC2965,
    }
}
