/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Serenity.Themes;
using Serenity.Web;
using Serenity.Xml.Html;

namespace Serenity.Web
{
    /// <summary>
    /// Defines a method which handle incoming CommonContexts by directing them to the proper resource class.
    /// </summary>
    public class ContextHandler
    {
        #region Methods - Public
        /// <summary>
        /// Handles an incoming CommonContext.
        /// </summary>
        /// <param name="context">The incoming CommonContext to be handled.</param>
        public virtual void HandleContext(CommonContext context)
        {
            Uri url = context.Request.Url;
			if (DomainSettings.Current == null)
			{
				DomainSettings.Current = DomainSettings.GetBestMatch(url);
			}
			DomainSettings settings = DomainSettings.Current;


            ResourceClass resourceClass;
            if ((settings.OmitResourceClass) || (url.Segments.Length < 2))
            {
                resourceClass = ResourceClass.GetResourceClass(settings.DefaultResourceClass);
            }
            else
            {
                resourceClass = ResourceClass.GetResourceClass(url.Segments[1].TrimEnd('/').ToLower());
            }
            if (resourceClass != null)
            {
                resourceClass.HandleContext(context);
            }
            else
            {
                //generate 404 not found response.
                ErrorHandler.Handle(context, StatusCode.Http404NotFound, url.ToString());
            }
        }
        #endregion
    }
}