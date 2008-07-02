/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Serenity.Logging;
using Serenity.Web.Resources;
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
        #region Methods - Public
        /// <summary>
        /// Handles an incoming CommonContext.
        /// </summary>
        /// <param name="context">The incoming CommonContext to be handled.</param>
        public virtual void HandleRequest(Request request, Response response)
        {
            Resource resource;
            ResourcePath path = ResourcePath.Create(request.Url);

            if (SerenityServer.Resources.Contains(path))
            {
                resource = SerenityServer.Resources[path];
            }
            else
            {
                path.IsSchemeUsed = false;
                if (SerenityServer.Resources.Contains(path))
                {
                    resource = SerenityServer.Resources[path];
                }
                else
                {
                    path.IsDomainUsed = false;
                    if (SerenityServer.Resources.Contains(path))
                    {
                        resource = SerenityServer.Resources[path];
                    }
                    else
                    {
                        ErrorHandler.Handle(StatusCode.Http404NotFound);
                        return;
                    }
                }
            }

            if (resource != null)
            {
                resource.PreRequest(request, response);
                resource.OnRequest(request, response);
                resource.PostRequest(request, response);
            }
            else
            {
                ErrorHandler.Handle(StatusCode.Http500InternalServerError);
                SerenityServer.ErrorLog.Write("The resource was null", LogMessageLevel.Error);
            }
        }
        #endregion
    }
}