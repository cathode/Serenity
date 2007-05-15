/*
Serenity - The next evolution of web server technology
Serenity/Pages/DomainsPage.cs
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
using Serenity.Xml;

namespace Serenity.Pages
{
    internal sealed class DomainsPage : SerenityPage
    {
        private IEnumerable<XmlNode> GetItemList(string[] items, XmlDocument doc)
        {
            if (items != null)
            {
                foreach (string item in items)
                {
                    yield return doc.CreateElement("Item", item);
                }
            }
        }
        public override void OnInitialization()
        {
            
        }
        public override void OnRequest(Serenity.Web.CommonContext context)
        {
            DomainSettings settings;
            RequestDataStream domainNameStream = context.Request.RequestData["name"];
            if (domainNameStream != null)
            {
                string domainName = domainNameStream.ReadAllText();
                settings = DomainSettings.GetBestMatch(domainName);
            }
            else
            {
                settings = DomainSettings.CurrentInstance;
            }

            XmlDocument doc = new XmlDocument();
            doc.DocumentElement = doc.CreateElement("DomainSettings");
            XmlElement e = doc.DocumentElement;
            e.AppendAttribute("Key", settings.Key);
            
            XmlElement active = doc.CreateElement("Active");
            e.AppendChild(active);
            XmlElement activeEnvironments = doc.CreateElement("Environments");
            active.AppendChild(activeEnvironments);
            activeEnvironments.AppendChildren(this.GetItemList(settings.ActiveEnvironments.Value, doc));
            XmlElement activeModules = doc.CreateElement("Modules");
            active.AppendChild(activeModules);
            activeModules.AppendChildren(this.GetItemList(settings.ActiveModules.Value, doc));

            XmlElement defaults = doc.CreateElement("Defaults");
            e.AppendChild(defaults);
            defaults.AppendChild(doc.CreateElement("Environment", settings.DefaultEnvironment.Value));
            defaults.AppendChild(doc.CreateElement("Module", settings.DefaultModule.Value));
            defaults.AppendChild(doc.CreateElement("ResourceClass", settings.DefaultResourceClass.Value));
            defaults.AppendChild(doc.CreateElement("ResourceName", settings.DefaultResourceName.Value));
            defaults.AppendChild(doc.CreateElement("Theme", settings.DefaultTheme.Value));

            context.Response.Write(doc.SaveMarkup());
            context.Response.MimeType = "application/xml";
        }
        public override void OnShutdown()
        {
            
        }
        [Obsolete]
        protected override string NameHelper
        {
            get
            {
                return "Domains";
            }
        }

        public override SerenityPage CreateSafeInstanceHelper()
        {
            return new DomainsPage();
        }
    }
}
