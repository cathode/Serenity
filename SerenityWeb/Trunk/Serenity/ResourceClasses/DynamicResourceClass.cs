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

namespace Serenity.ResourceClasses
{
	internal class DynamicResourceClass : ResourceClass
	{
		public DynamicResourceClass() : base("dynamic")
		{
		}
		public override void HandleContext(CommonContext context)
		{
			ContentPage page = null;

			// http://localhost/dynamic/system/default

			DomainSettings settings = DomainSettings.GetBestMatch(context.Request.Url);
			int n = ((settings.OmitResourceClass) ? 1 : 2);
			if (context.Request.Url.Segments.Length > n)
			{
				Module module = Module.GetModule(context.Request.Url.Segments[n].TrimEnd('/'));
				if (module != null)
				{
					if (context.Request.Url.Segments.Length > n + 1)
					{
						string pageName = string.Join("", context.Request.Url.Segments, n + 1, context.Request.Url.Segments.Length - (n + 1));
						page = module.GetPage(pageName);
					}
					else
					{
						page = module.DefaultPage;
					}
				}
				else
				{
					page = null;
				}
			}
			else
			{
				Module module = Module.GetModule(settings.DefaultResourceName);
				if (module != null)
				{
					page = module.DefaultPage;
				}
				else
				{
					page = null;
				}
			}

			if (page != null)
			{
				ContentPage newPage = page.CreateInstance();
				newPage.OnRequest(context);
			}
			else
			{
				//404 Not Found response
				ErrorHandler.Handle(context, StatusCode.Http404NotFound, context.Request.Url.ToString());
			}
		}
	}
}
