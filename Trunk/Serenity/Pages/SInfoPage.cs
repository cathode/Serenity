/*
Serenity - The next evolution of web server technology
Serenity/Pages/SInfoPage.cs
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

using Serenity;
using Serenity.Web;
using Serenity.Xml;
using Serenity.Xml.Html;

namespace Serenity.Pages
{
    internal class SInfoPage : SerenityPage
    {
        public override void OnInitialization()
        {
            
        }

        public override void OnRequest(CommonContext Context)
        {
            CommonResponse Response = Context.Response;

            XmlDocument Doc = new XmlDocument();
            XmlElement E = Doc.DocumentElement = Doc.CreateElement("Serenity");

            XmlElement envs = Doc.CreateElement("Environments");
            foreach (SerenityEnvironment env in SerenityEnvironment.Instances)
            {
                XmlElement envElement = Doc.CreateElement("Environment");
                //WS: AppendMarkup shouldnt be used like this, oh well.
                envElement.AppendMarkup("<Name>" + env.Key + "</Name>");
                envElement.AppendMarkup("<DefaultModule>" + env.Key + "</DefaultModule>");
                envElement.AppendMarkup("<Theme>" + env.Theme.Key + "</Theme>");
            }

            Response.Write(Doc.SaveMarkup());
            Response.MimeType = "application/xml";
        }

        public override void OnShutdown()
        {
            
        }

        protected override string NameHelper
        {
            get
            {
                return "SInfo";
            }
        }

        public override SerenityPage CreateSafeInstanceHelper()
        {
            return new SInfoPage();
        }
    }
}
