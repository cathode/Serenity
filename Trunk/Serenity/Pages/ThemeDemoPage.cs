/*
Serenity - The next evolution of web server technology
Serenity/Pages/DomainsPage.cs
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
using Serenity.Attributes;
using Serenity.Themes;
using Serenity.Web;
using Serenity.Xml.Html;

namespace Serenity.Pages
{
    internal class ThemeDemoPage : SerenityPage
    {
        public override void OnInitialization()
        {
            
        }

        public override void OnRequest(CommonContext context)
        {
            CommonResponse response = context.Response;

            HtmlDocument Doc = new HtmlDocument();
            Doc.AddStylesheet(Theme.CurrentInstance.StylesheetUrl);
            HtmlElement e = (HtmlElement)Doc.DocumentElement;

            foreach (Style s in Theme.CurrentInstance.AllStyles)
            {
                e.AppendParagraph("This text is styled using the " + s.Class + " style.", s);
            }

            response.Write(Doc.SaveMarkup());
            response.MimeType = "text/html";
            response.UseCompression = true;
        }

        public override void OnShutdown()
        {
            
        }

        protected override string NameHelper
        {
            get
            {
                return "ThemeDemo";
            }
        }

        public override SerenityPage CreateSafeInstanceHelper()
        {
            return new ThemeDemoPage();
        }
    }
}
