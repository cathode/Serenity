/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *----------------------------------------------------------------------------*
 * Authors:                                                                   *
 * - Will 'AnarkiNet' Shelley (AnarkiNet@gmail.com): Original Author          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Serenity.Properties;
using System.Xml.Linq;
using System.Linq;

using Serenity.Web.Resources;
using Serenity.Web;
using Serenity.Web.Forms;

namespace Serenity.Web.Resources
{
    /// <summary>
    /// Provides a dynamic resource that represents a virtual directory and can
    /// provide indexing services for virtual directories.
    /// </summary>
    [SuppressLoadCreation(true)]
    public class DirectoryResource : Resource
    {
        #region Constructors - internal
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryResource"/>
        /// class.
        /// </summary>
        public DirectoryResource()
        {
            this.ContentType = MimeType.TextHtml;
        }
        #endregion
        #region Fields
        /// <summary>
        /// Holds the path of the XSLT stylesheet used to render the XML index output.
        /// </summary>
        public const string XsltStylesheetUrl = "/serenity/resource/index.xslt";
        public const string StylesheetUrl = "/serenity/resource/index.css";
        #endregion
        #region Methods - Public
        /// <summary>
        /// Overridden. Renders the dynamic content of the current DirectoryResource.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public override void OnRequest(Request request, Response response)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            else if (response == null)
            {
                throw new ArgumentNullException("response");
            }
            else if (response.IsComplete)
            {
                return;
            }
            Resource child = this.GetChild(request.Url);

            if (child != null)
            {
                child.OnRequest(request, response);
                return;
            }
            else if (!request.Url.AbsolutePath.Equals(this.Uri.AbsolutePath, StringComparison.OrdinalIgnoreCase))
            {
                response.Status = StatusCode.Http404NotFound;
                response.Write("HTTP 404 Not Found");
                response.ContentType = MimeType.TextPlain;
                return;
            }

            // collect data for index generation
            SortedDictionary<ResourceGrouping, List<Resource>> groupedResources = new SortedDictionary<ResourceGrouping, List<Resource>>();

            foreach (Resource resource in this.Children)
            {
                if (!groupedResources.ContainsKey(resource.Grouping))
                {
                    groupedResources.Add(resource.Grouping, new List<Resource>());
                }
                groupedResources[resource.Grouping].Add(resource);
            }
            Uri uri = this.Uri;

            string baseUri = uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped);

            var doc = new XDocument(new XElement("html",
                new XElement("head",
                    new XElement("title", string.Format(AppResources.DirectoryTitle, uri.ToString())),
                    new XElement("link",
                        new XAttribute("rel", "stylesheet"),
                        new XAttribute("type", "text/css"),
                        new XAttribute("href", DirectoryResource.StylesheetUrl))),
                new XElement("body",
                    new XElement("div",
                        new XAttribute("class", "main_heading"),
                        string.Format(AppResources.DirectoryTitle, string.Empty),
                        new XElement("a",
                            new XAttribute("href", baseUri), baseUri),
                            "/",
                            from i in Enumerable.Range(1, uri.Segments.Length - 1)
                            let seg = uri.Segments[i]
                            select new XElement("span", new XElement("a",
                                new XAttribute("href", baseUri
                                    + string.Concat((from s in Enumerable.Range(0, i + 1)
                                                     select uri.Segments[s]).ToArray())), seg.TrimEnd('/')), "/"),
                        from g in groupedResources
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
                                                AppResources.DirectoryNameColumn),
                                            new XElement("th",
                                                new XAttribute("class", "size"),
                                                AppResources.DirectorySizeColumn),
                                            new XElement("th",
                                                new XAttribute("class", "description"),
                                                AppResources.DirectoryDescriptionColumn),
                                            new XElement("th",
                                                new XAttribute("class", "modified"),
                                                AppResources.DirectoryModifiedColumn)),
                                from r in g.Value
                                select new XElement("tr",
                                    new XElement("td",
                                        new XElement("a",
                                            new XAttribute("href", "/serenity/resource/page_white.png"))),
                                    new XElement("td",
                                        new XElement("a",
                                            new XAttribute("href", r.Uri),
                                            (r.Name.Length > 0) ? r.Name : AppResources.DirectoryItemDefaultName)),
                                            new XElement("td",
                                                (r.Size < 0) ? "N/A" :
                                                (r.Size < 1024) ? r.Size.ToString("G") + "B" :
                                                (r.Size < 1048576) ? (r.Size / 1024F).ToString("G2") + "KB" :
                                                (r.Size < 1073741824) ? (r.Size / 1048576F).ToString("G2") + "MB":
                                                (r.Size / 1073741824F).ToString("G2") + "GB"),
                                    new XElement("td",
                                        g.Key.SingularForm),
                                    new XElement("td",
                                        DateTime.Now.ToString("o")))))))));

            // output data
            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.ConformanceLevel = ConformanceLevel.Document;
                settings.Encoding = Encoding.UTF8;
                settings.Indent = false;
                
                using (XmlWriter writer = XmlWriter.Create(ms, settings))
                {
                    doc.Save(writer);

                    /*
                    // <?xml version="1.0" encoding="utf-8" ?>
                    writer.WriteStartDocument();
                    // <?xsl
                    writer.WriteProcessingInstruction("xml-stylesheet", "type='text/xsl' href='" + DirectoryResource.XsltStylesheetUrl + "'");
                    // <index>
                    writer.WriteStartElement("index");
                    //   <location>Node Path</location>
                    writer.WriteStartElement("location");
                    writer.WriteString(this.Uri.ToString());
                    writer.WriteEndElement();
                    foreach (KeyValuePair<ResourceGrouping, List<Resource>> pair in groupedResources)
                    {
                        // <group name="name">
                        writer.WriteStartElement("group");
                        writer.WriteAttributeString("name", pair.Key.PluralForm);
                        writer.WriteStartElement("field");
                        writer.WriteAttributeString("id", "name");
                        writer.WriteAttributeString("name", AppResources.DirectoryNameColumn);
                        writer.WriteEndElement();

                        writer.WriteStartElement("field");
                        writer.WriteAttributeString("id", "size");
                        writer.WriteAttributeString("name", AppResources.DirectorySizeColumn);
                        writer.WriteEndElement();

                        writer.WriteStartElement("field");
                        writer.WriteAttributeString("id", "description");
                        writer.WriteAttributeString("name", AppResources.DirectoryDescriptionColumn);
                        writer.WriteEndElement();

                        writer.WriteStartElement("field");
                        writer.WriteAttributeString("id", "timestamp");
                        writer.WriteAttributeString("name", AppResources.DirectoryModifiedColumn);
                        writer.WriteEndElement();

                        foreach (Resource res in pair.Value)
                        {
                            writer.WriteStartElement("item");
                            //writer.WriteAttributeString("icon", FileTypeRegistry.GetIcon(System.IO.Path.GetExtension(res.Name)));
                            writer.WriteAttributeString("icon", "page_white");
                            writer.WriteStartElement("value");
                            writer.WriteAttributeString("link", res.Uri.ToString());
                            writer.WriteString((res.Name.Length > 0) ? res.Name : AppResources.DirectoryItemDefaultName);
                            writer.WriteEndElement();
                            if (res.IsSizeKnown)
                            {
                                writer.WriteElementString("value", res.Size.ToString());
                            }
                            else
                            {
                                writer.WriteElementString("value", "---");
                            }
                            //writer.WriteElementString("value", FileTypeRegistry.GetDescription(System.IO.Path.GetExtension(res.Name)));
                            writer.WriteElementString("value", res.Grouping.SingularForm);
                            writer.WriteElementString("value", "---");
                            writer.WriteEndElement();
                        }
                        // </group>
                        writer.WriteEndElement();
                    }
                    writer.WriteEndDocument();
                    */
                    writer.Flush();
                    writer.Close();
                }
                response.Write(ms.ToArray());
                response.IsComplete = true;
            }
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets the resource grouping of the current DirectoryResource.
        /// </summary>
        public override ResourceGrouping Grouping
        {
            get
            {
                return ResourceGrouping.Directories;
            }
        }
        public override bool CanHaveChildren
        {
            get
            {
                return true;
            }
        }
        #endregion
    }
}
