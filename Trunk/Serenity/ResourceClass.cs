/*
Serenity - The next evolution of web server technology

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

using Serenity.Web;
using Serenity.Web.Drivers;

//WS: This code probably needs LOTS of work in the future.

namespace Serenity
{
    public abstract class ResourceClass : Multiton<string, ResourceClass>
    {
        protected ResourceClass(string name) : base(name.ToLower())
        {

        }
        public abstract void HandleContext(CommonContext context);
    }
}
