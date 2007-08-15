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

using Serenity.Web;
using Serenity.Xml;

namespace Serenity.Pages
{
    public sealed class DomainsPage : ContentPage
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
            DomainSettings settings = DomainSettings.GetBestMatch(context.Request.Url);

            CommonResponse response = context.Response;

            if (context.Request.RequestData.Contains("domain"))
            {
                settings = DomainSettings.GetBestMatch(context.Request.RequestData["domain"].ReadAllText());
            }

            response.Write("Working with domainsettings: " + settings.Name);

            response.MimeType = MimeType.ApplicationXml;
        }
        public override void OnShutdown()
        {
            
        }

        public override string Name
        {
            get
            {
                return "Domains";
            }
        }

        public override ContentPage CreateInstance()
        {
            return new DomainsPage();
        }
        public override MasterPage CreateMasterPageInstance()
        {
            return new SystemMaster();
        }
    }
}
