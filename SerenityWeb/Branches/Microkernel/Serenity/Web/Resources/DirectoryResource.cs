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
            this.ContentType = MimeType.ApplicationXml;
        }
        #endregion
        #region Fields
        /// <summary>
        /// Holds the path of the XSLT stylesheet used to render the XML index output.
        /// </summary>
        public const string XsltStylesheetUrl = "/serenity/resource/index.xslt";
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
            SortedDictionary<string, List<Resource>> groupedResources = new SortedDictionary<string, List<Resource>>();
            
            foreach (Resource resource in this.Children)
            {
                if (!groupedResources.ContainsKey(resource.Grouping.PluralForm))
                {
                    groupedResources.Add(resource.Grouping.PluralForm, new List<Resource>());
                }
                groupedResources[resource.Grouping.PluralForm].Add(resource);
            }
            
            // output data
            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.ConformanceLevel = ConformanceLevel.Document;
                settings.Encoding = Encoding.UTF8;
                using (XmlTextWriter writer = new XmlTextWriter(ms, Encoding.UTF8))
                {
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
                    foreach (KeyValuePair<string, List<Resource>> pair in groupedResources)
                    {
                        // <group name="name">
                        writer.WriteStartElement("group");
                        writer.WriteAttributeString("name", pair.Key);
                        //   <field id="name" name="Thing Name" />
                        writer.WriteStartElement("field");
                        writer.WriteAttributeString("id", "name");
                        writer.WriteAttributeString("name", pair.Value[0].Grouping.SingularForm + " Name");
                        writer.WriteEndElement();

                        writer.WriteStartElement("field");
                        writer.WriteAttributeString("id", "size");
                        writer.WriteAttributeString("name", "Size");
                        writer.WriteEndElement();

                        writer.WriteStartElement("field");
                        writer.WriteAttributeString("id", "description");
                        writer.WriteAttributeString("name", "Description");
                        writer.WriteEndElement();

                        writer.WriteStartElement("field");
                        writer.WriteAttributeString("id", "timestamp");
                        writer.WriteAttributeString("name", "Timestamp");
                        writer.WriteEndElement();

                        foreach (Resource res in pair.Value)
                        {
                            writer.WriteStartElement("item");  
                            //writer.WriteAttributeString("icon", FileTypeRegistry.GetIcon(System.IO.Path.GetExtension(res.Name)));
                            writer.WriteAttributeString("icon", "FIXME");
                            writer.WriteStartElement("value");
                            writer.WriteAttributeString("link", res.Uri.ToString());
                            writer.WriteString(res.Name);
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
                            writer.WriteElementString("value", "FIXME");
                            writer.WriteElementString("value", "---");
                            writer.WriteEndElement();
                        }
                        // </group>
                        writer.WriteEndElement();
                    }
                    writer.WriteEndDocument();
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
