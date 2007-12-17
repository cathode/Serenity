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
using System.IO;
using System.Text;

using Serenity.Resources;
using Serenity.Themes;
using Serenity.Web;

namespace Serenity.Web
{
    /// <summary>
    /// Provides a handler for incoming CommonContexts by directing them to the proper resource class.
    /// </summary>
    public class ContextHandler
    {
        #region Constructors - Protected Internal
        /// <summary>
        /// Initializes a new instance of the ContextHandler class.
        /// </summary>
        protected internal ContextHandler()
        {
        }
        #endregion
        #region Fields - Private
        #endregion
        #region Methods - Public
        /// <summary>
        /// Handles an incoming CommonContext.
        /// </summary>
        /// <param name="context">The incoming CommonContext to be handled.</param>
        public virtual void HandleContext(CommonContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            Resource resource;
            if (context.HasDomain)
            {
                ResourcePath path = new ResourcePath(context.Request.Url.AbsolutePath);
                try
                {
                    resource = context.Domain.Resources.GetResource(path);
                }
                catch
                {
                    ErrorHandler.Handle(context, StatusCode.Http404NotFound);
                    return;
                }
            }
            else
            {
                try
                {
                    resource = SerenityServer.CommonDomain.Resources.GetResource(new ResourcePath(context.Request.Url.AbsolutePath));
                }
                catch
                {
                    ErrorHandler.Handle(context, StatusCode.Http404NotFound);
                    return;
                }
            }

            if (resource != null)
            {
                resource.PreRequest(context);
                resource.OnRequest(context);
                resource.PostRequest(context);
            }
            else
            {
                ErrorHandler.Handle(context, StatusCode.Http404NotFound);
            }
        }
        #endregion
        #region Properties - Public
        #endregion
    }
}