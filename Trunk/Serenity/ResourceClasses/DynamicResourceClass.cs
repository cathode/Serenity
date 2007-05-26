/*
Serenity - The next evolution of web server technology
Serenity/ResourceClasses/DynamicResourceClass.cs
Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
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
            ContentPage page;
            InstanceManager<DomainSettings> manager = InstanceManager<DomainSettings>.Default;
            if (context.Request.Url.Segments.Length > 1)
            {
                int n = 3;
                if (manager.CurrentInstance.OmitEnvironment.Value)
                {
                    n--;
                }
                if (manager.CurrentInstance.OmitResourceClass.Value)
                {
                    n--;
                }
                string[] nameParts = new string[context.Request.Url.Segments.Length - n];
                Array.Copy(context.Request.Url.Segments, n, nameParts, 0, nameParts.Length);
                page = SerenityModule.CurrentInstance.GetPage(string.Join("", nameParts).ToLower());
            }
            else
            {
                page = SerenityModule.CurrentInstance.DefaultPage;
            }
           
            if (page != null)
            {
                ContentPage newPage = page.CreateInstance();
                newPage.OnRequest(context);
            }
            else
            {
                //404 Not Found response
                context.Response.Write("404 Not Found");
                context.Response.Status = StatusCode.Http404NotFound;
            }
        }
    }
}
