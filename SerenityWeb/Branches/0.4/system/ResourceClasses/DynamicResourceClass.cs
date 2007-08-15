/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
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

			if (context.Request.Url.Segments.Length > 1)
			{
				int n = 3;
				if (settings.OmitResourceClass)
				{
					n--;
				}
				string[] nameParts = new string[Math.Max(context.Request.Url.Segments.Length - n, 0)];
				if (nameParts.Length > 0)
				{
					Array.Copy(context.Request.Url.Segments, n, nameParts, 0, nameParts.Length);
					//page = null; //SerenityModule.CurrentInstance.GetPage(string.Join("", nameParts).ToLower());
					ErrorHandler.Handle(context, StatusCode.Http501NotImplemented);
					return;
				}
				else
				{
					//page = null; //SerenityModule.CurrentInstance.DefaultPage;
					ErrorHandler.Handle(context, StatusCode.Http501NotImplemented);
					return;
				}
			}
			else
			{
				Module module = Module.GetModule(settings.DefaultResource);
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
