/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/

namespace Serenity.Web
{
    /// <summary>
    /// Represents supported request methods.
    /// </summary>
    public enum RequestMethod
    {
        /// <summary>
        /// Indicates an unknown, invalid, or otherwise not supported request method.
        /// </summary>
        Unknown,

        /// <summary>
        /// Indicates the HTTP GET method.
        /// </summary>
        GET,

        /// <summary>
        /// Indicates the HTTP POST method.
        /// </summary>
        POST,

        /// <summary>
        /// Indicates the HTTP HEAD method.
        /// </summary>
        HEAD,

        PUT,

        DELETE,

        TRACE,

        /// <summary>
        /// The OPTIONS method represents a request for information about the communication options available on the
        /// request/response chain identified by the Request-URI.
        /// </summary>
        OPTIONS,

        CONNECT,

        PROPFIND,

        PROPPATCH,

        MKCOL,

        COPY,

        MOVE,

        LOCK,

        UNLOCK,
    }
}