/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

using Serenity.Web.Resources;
using Serenity.Web;
using Serenity.Web.Forms;

namespace Serenity
{
    /// <summary>
    /// Provides a dynamic resource that represents a virtual directory and can
    /// provide indexing services for virtual directories.
    /// </summary>
    [SuppressLoadCreation(true)]
    public sealed class DirectoryResource : Resource
    {
        #region Constructors - internal
        public DirectoryResource(ResourcePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("uri");
            }
            this.Path = path;

            string temp = path.Path.TrimEnd('/');
            if (temp.Length > 0)
            {
                this.Name = temp.Substring(temp.LastIndexOf('/') + 1);
            }
            else
            {
                this.Name = "";
            }
            this.ContentType = MimeType.ApplicationXml;
        }
        #endregion
        #region Fields - Public
        /// <summary>
        /// Holds the path of the XSLT stylesheet used to render the XML index output.
        /// </summary>
        public const string XsltStylesheetUrl = "/resource/serenity/index.xslt";
        #endregion
        //#region Methods - Protected
        //protected override Document CreateForm()
        //{
        //    return new DirectoryDocument(this.Path);
        //}
        //#endregion
        #region Methods - Public

        /// <summary>
        /// Overridden. Renders the dynamic content of the current DirectoryResource.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public override void OnRequest(Request request, Response response)
        {
            // collect data
            SortedDictionary<string, List<Resource>> groupedResources = new SortedDictionary<string, List<Resource>>();

            foreach (Resource resource in SerenityServer.Resources.GetChildren(this.Path, true))
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
                    writer.WriteString(this.Path.ToString());
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
                            writer.WriteAttributeString("icon", FileTypeRegistry.GetIcon(System.IO.Path.GetExtension(res.Name)));
                            writer.WriteStartElement("value");
                            writer.WriteAttributeString("link", res.Path.ToString());
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
                            writer.WriteElementString("value", FileTypeRegistry.GetDescription(System.IO.Path.GetExtension(res.Name)));
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
        #endregion

    }
}
