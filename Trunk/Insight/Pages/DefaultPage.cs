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
using Serenity.Web.Drivers;
using Serenity.Xml;
using Serenity.Xml.Html;

namespace Insight.Pages
{
    public class DefaultPage : SerenityPage
    {
        public override void OnInitialization()
        {
            //Nothing
        }

        public override void OnRequest(CommonContext Context)
        {
            HtmlDocument Doc = new HtmlDocument();
            //Doc.Doctype = Doctype.XHTML11;
            HtmlElement P = Doc.BodyElement;
            P.AppendText("This is the index page for Insight, a wiki module for Serenity.");
            P.AppendChild(Doc.CreateBreak());
            P.AppendText("This is a temporary demonstration of the capabilities of Insight/Serenity.");


            Context.Response.Write(Doc.SaveMarkup());
            Context.Response.MimeType = "text/html";
        }

        public override void OnShutdown()
        {
            //Nothing
        }

        protected override string NameHelper
        {
            get
            {
                return "default";
            }
        }

        public override SerenityPage CreateSafeInstanceHelper()
        {
            return new DefaultPage();
        }
    }
}
