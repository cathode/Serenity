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

using Serenity;
using Serenity.Web;
using Serenity.Web.Drivers;
using Serenity.Xml;
using Serenity.Xml.Html;

namespace Serenity.Pages
{
    public sealed class DefaultPage : ContentPage
    {
        public override void OnRequest(CommonContext context)
        {
			context.Response.WriteLine("This Page has not yet been implemented!");
        }
        public override ContentPage CreateInstance()
        {
            return new DefaultPage();
        }
        public override MasterPage CreateMasterPageInstance()
        {
            return new SystemMaster();
        }
        public override string Name
        {
            get
            {
                return "Default";
            }
        }
    }
}
