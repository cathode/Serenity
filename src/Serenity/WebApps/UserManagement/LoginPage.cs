/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serenity.Web;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace Serenity.WebApps.UserManagement
{
    public class LoginPage : Resource
    {
        #region Constructors
        public LoginPage()
        {
            this.UniqueID = new Guid("{1134C6F2-09E4-47E3-AFF9-F698AFA22407}");
        }
        #endregion
        #region Methods
        public override void OnRequest(Request request, Response response)
        {
            base.OnRequest(request, response);


            var doc = new XDocument(
                new XElement("html",
                 new XElement("head",
                     new XElement("title", "Serenity: Login"),
                //new XElement("link",
                //    new XAttribute("rel", "stylesheet"),
                //    new XAttribute("type", "text/css"),
                //    new XAttribute("href", DirectoryResource.StylesheetUrl))),
                 new XElement("body",
                     new XElement("form",
                         new XAttribute("method", "get"),
                         //new XAttribute("action", this.Binding.Path),
                         new XAttribute("enctype", "multipart/form-data"),
                         new XText("Username: "),
                         new XElement("input",
                             new XAttribute("type", "username"),
                             new XAttribute("name", "username")),
                             new XText("Password: "),
                         new XElement("input",
                             new XAttribute("type", "password"),
                             new XAttribute("name", "password")),
                            new XElement("input",
                                new XAttribute("type", "submit"),
                                new XAttribute("name", "submit"),
                                new XAttribute("value", "Login")))))));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;

            // output data
            using (MemoryStream ms = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(ms, settings))
                {
                    doc.Save(writer);
                    writer.Flush();
                    writer.Close();
                }
                response.Write(ms.ToArray());
            }
            response.ContentType = MimeType.TextHtml;
            response.IsComplete = true;

        }
        #endregion
    }
}
