/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Serenity.Web.Themes;
using System.Diagnostics.Contracts;

namespace Serenity.Web
{
    /// <summary>
    /// Provides a dynamic resource that represents a virtual directory and can
    /// provide indexing services for virtual directories.
    /// </summary>
    public class DirectoryResource : Resource
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryResource"/> class.
        /// </summary>
        public DirectoryResource()
        {
            this.ContentType = MimeType.ApplicationXhtmlPlusXml;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryResource"/> class.
        /// </summary>
        /// <param name="name"></param>
        public DirectoryResource(string name)
        {
            this.Name = name;
        }
        #endregion
        #region Fields
        /// <summary>
        /// Holds the relative URI to the stylesheet used to style the directory index.
        /// </summary>
        public const string StylesheetUrl = "/Content/DirectoryResource.css";
        #endregion
        #region Methods
        /// <summary>
        /// Overridden. Renders the dynamic content of the current DirectoryResource.
        /// </summary>
        /// <param name="request">The incoming <see cref="Request"/>.</param>
        /// <param name="response">The outgoing <see cref="Response"/>.</param>
        public override void OnRequest(Request request, Response response)
        {
            if (response.IsComplete)
                return;

            Uri uri = this.GetRelativeUri();

            // collect data for index generation
            SortedDictionary<ResourceGrouping, List<Resource>> groupedResources = new SortedDictionary<ResourceGrouping, List<Resource>>();
            if (this.Locations.Count > 0)
                foreach (var res in from n in this.Locations[0]
                                    where n.Resource != null
                                    select n.Resource)
                {
                    if (!groupedResources.ContainsKey(res.Grouping))
                    {
                        groupedResources.Add(res.Grouping, new List<Resource>());
                    }
                    groupedResources[res.Grouping].Add(res);
                }

            string host = request.UserHostName ?? "localhost";

            //string host = uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped);

            var doc = new XDocument(new XElement("html",
                new XElement("head",
                    new XElement("title", string.Format(Theme.DirectoryTitle, uri.ToString())),
                    new XElement("link",
                        new XAttribute("rel", "stylesheet"),
                        new XAttribute("type", "text/css"),
                        new XAttribute("href", DirectoryResource.StylesheetUrl))),
                new XElement("body",
                    new XElement("div",
                        new XAttribute("class", "main_heading"),
                        string.Format(Theme.DirectoryTitle, string.Empty),
                        new XElement("a",
                            new XAttribute("href", host), host),
                            "/"),
                        from g in groupedResources
                        orderby g.Key
                        select new XElement("div",
                            new XElement("div",
                                new XAttribute("class", "group_heading"),
                                g.Key.PluralForm),
                                new XElement("table", new XAttribute("class", "group"),
                                    new XElement("tr",
                                        new XElement("th",
                                            new XAttribute("class", "icon")),
                                            new XElement("th",
                                                new XAttribute("class", "name"),
                                                "Name"),
                                            new XElement("th",
                                                new XAttribute("class", "size"),
                                                "Size"),
                                            new XElement("th",
                                                new XAttribute("class", "description"),
                                                "Description"),
                                            new XElement("th",
                                                new XAttribute("class", "modified"),
                                                "Modified")),
                                from r in g.Value
                                orderby r.Name
                                select new XElement("tr",
                                    new XElement("td",
                                        new XElement("img",
                                            new XAttribute("src", "/serenity/icons/page_white.png"))),
                                    new XElement("td",
                                        new XElement("a",
                                            new XAttribute("href", r.GetAbsoluteUri(request.Url)),
                                            (r.Name.Length > 0) ? r.Name : "default")),
                                            new XElement("td",
                                                (r.Size < 0) ? "N/A" :
                                                (r.Size < 1024) ? r.Size.ToString("G") + "B" :
                                                (r.Size < 1048576) ? (r.Size / 1024F).ToString("G2") + "KB" :
                                                (r.Size < 1073741824) ? (r.Size / 1048576F).ToString("G2") + "MB" :
                                                (r.Size / 1073741824F).ToString("G2") + "GB"),
                                    new XElement("td",
                                        g.Key.SingularForm),
                                    new XElement("td",
                                        r.Modified.ToString("yyyy-MM-dd HH:mm:ss"))))))));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.Encoding = Encoding.UTF8;
            settings.Indent = false;

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

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(this.Locations != null);
        }
        #endregion
    }
}
