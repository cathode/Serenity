/* Serenity - The next evolution of web server technology.
 * Copyright � 2006-2009 Serenity Project - http://SerenityProject.net/
 * 
 * This software is released under the terms and conditions of the Microsoft Public License (MS-PL),
 * a copy of which should have been included with this distribution as License.txt.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Serenity.Properties;

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
            this.ContentType = MimeType.TextHtml;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryResource"/> class.
        /// </summary>
        /// <param name="name"></param>
        public DirectoryResource(string name)
            : this()
        {
            this.Name = name;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryResource"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="children"></param>
        public DirectoryResource(string name, params Resource[] children)
            : this(name)
        {
            this.Add(children);
        }
        #endregion
        #region Fields
        /// <summary>
        /// Holds the relative URI to the stylesheet used to style the directory index.
        /// </summary>
        public const string StylesheetUrl = "/serenity/index.css";
        #endregion
        #region Methods
        /// <summary>
        /// Overridden. Renders the dynamic content of the current DirectoryResource.
        /// </summary>
        /// <param name="request">The incoming <see cref="Request"/>.</param>
        /// <param name="response">The outgoing <see cref="Response"/>.</param>
        public override void OnRequest(Request request, Response response)
        {
            if (request == null)
                throw new ArgumentNullException("request");
            else if (response == null)
                throw new ArgumentNullException("response");
            else if (response.IsComplete)
                return;
            
            Resource child = this.GetChild(request.Url);

            Uri uri = this.GetAbsoluteUri(request.Url);

            if (child != null)
            {
                child.OnRequest(request, response);
                return;
            }
            else if (!request.Url.AbsolutePath.Equals(uri.AbsolutePath, StringComparison.OrdinalIgnoreCase))
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

            string host = uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped);

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
                            new XAttribute("href", host), host),
                            "/",
                            from i in Enumerable.Range(1, uri.Segments.Length - 1)
                            select new XElement("span", new XElement("a",
                                new XAttribute("href", host
                                    + string.Concat((from s in Enumerable.Range(0, i + 1)
                                                     select uri.Segments[s]).ToArray())), uri.Segments[i].TrimEnd('/')), "/"),
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
                                orderby r.Name
                                select new XElement("tr",
                                    new XElement("td",
                                        new XElement("img",
                                            new XAttribute("src", "/serenity/icons/page_white.png"))),
                                    new XElement("td",
                                        new XElement("a",
                                            new XAttribute("href", r.GetAbsoluteUri(request.Url)),
                                            (r.Name.Length > 0) ? r.Name : AppResources.DirectoryItemDefaultName)),
                                            new XElement("td",
                                                (r.Size < 0) ? "N/A" :
                                                (r.Size < 1024) ? r.Size.ToString("G") + "B" :
                                                (r.Size < 1048576) ? (r.Size / 1024F).ToString("G2") + "KB" :
                                                (r.Size < 1073741824) ? (r.Size / 1048576F).ToString("G2") + "MB" :
                                                (r.Size / 1073741824F).ToString("G2") + "GB"),
                                    new XElement("td",
                                        g.Key.SingularForm),
                                    new XElement("td",
                                        r.Modified.ToString("yyyy-MM-dd HH:mm:ss")))))))));

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
        
        #endregion
        #region Properties
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
        /// <summary>
        /// Overridden. Returns true, indicating <see cref="DirectResource">DirectoryResources</see> support child resources.
        /// </summary>
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