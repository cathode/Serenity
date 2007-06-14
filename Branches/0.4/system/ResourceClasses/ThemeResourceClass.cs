/*
Serenity - The next evolution of web server technology
Serenity/ResourceClasses/ThemeResourceClass.cs
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
    internal class ThemeResourceClass : ResourceClass
    {
        public ThemeResourceClass() : base("theme")
        {

        }
        public override void HandleContext(Serenity.Web.CommonContext context)
        {
            CommonResponse response = context.Response;
            response.Write(SerenityEnvironment.CurrentInstance.Theme.StylesheetContent);
            response.Status = StatusCode.Http200Ok;
            response.MimeType = MimeType.TextCss;
            response.UseCompression = true;
        }
    }
}