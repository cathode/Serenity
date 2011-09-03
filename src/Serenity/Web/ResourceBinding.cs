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
    /// Represents a binding of a resource to the server
    /// </summary>
    public class ResourceBinding
    {
        #region Properties
        public Resource Resource
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the path that the resource is bound to.
        /// </summary>
        public string Path
        {
            get;
            internal set;
        }

        public bool IsAbsolutePath
        {
            get;
            internal set;
        }
        #endregion
    }
}
