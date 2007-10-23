/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
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
    /// <summary>
    /// Represents a master page, which allows Pages to offload shared work.
    /// </summary>
    public abstract class MasterPage
    {
        #region Methods - Public
        /// <summary>
        /// Invoked when a request is recieved, but before the Page handles the request.
        /// </summary>
        /// <param name="context"></param>
        public virtual void PreRequest(CommonContext context)
        {
        }
        /// <summary>
        /// Invoked when a request is recieved, but after the Page has handled the request.
        /// </summary>
        /// <param name="context"></param>
        public virtual void PostRequest(CommonContext context)
        {
        }
        #endregion
    }
}
