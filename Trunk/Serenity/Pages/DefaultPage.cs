/*
Serenity - The next evolution of web server technology
Serenity/Pages/IndexPage.cs
Copyright � 2006-2007 Serenity Project (http://SerenityProject.net/)

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

namespace Serenity.Pages
{
    [Serenity.Attributes.PageName("Default")]
    internal class DefaultPage : SerenityPage
    {
        public override void OnInitialization()
        {
            
        }
        public override void OnRequest(CommonContext Context)
        {
            HtmlDocument Doc = new HtmlDocument();
            HtmlElement P = Doc.BodyElement.AppendParagraph();
            P.AppendText("Welcome to Serenity administrative index.");
            P.AppendBreak();
            P.AppendAnchor("http://serenityproject.net/", "Project Homepage");
            Context.Response.Write(Doc.SaveMarkup());
            Context.Response.MimeType = "text/html";
        }
        public override void OnShutdown()
        {
            
        }
        [Obsolete]
        protected override string NameHelper
        {
            get
            {
                return "Default";
            }
        }

        public override SerenityPage CreateSafeInstanceHelper()
        {
            return new DefaultPage();
        }
    }
}
