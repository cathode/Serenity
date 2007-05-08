/*
Insight - The Intelligent Wiki Engine

Copyright © 2006-2007 Serenity Project (http://serenityproject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.Text;

using Serenity;
using Serenity.Web;

namespace Insight.ResourceClasses
{
    public class WikiResourceClass : ResourceClass
    {
        public WikiResourceClass() : base("wiki")
        {

        }
        public override void HandleContext(CommonContext context)
        {
            context.Response.Write("Not implemented");
            context.Response.Status = Serenity.Web.StatusCode.Http501NotImplemented;
        }
    }
}
