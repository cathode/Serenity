/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;

namespace Serenity.Data
{
    /// <summary>
    /// Used to determine which database is used for any data operations.
    /// </summary>
    [Flags]
    public enum DataScope
    {
        /// <summary>
        /// Indicates data that has no scope, e.g. it is available to the entire server.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates data that is relative to the web application that requests it.
        /// </summary>
        Application = 1,

        /// <summary>
        /// Indicates data that is relative to the domain (hostname) of the request being processed.
        /// </summary>
        Domain = 2,

        /// <summary>
        /// Indicates data that is relative to the user who initiated the request that is being processed.
        /// </summary>
        User = 4,
    }
}
