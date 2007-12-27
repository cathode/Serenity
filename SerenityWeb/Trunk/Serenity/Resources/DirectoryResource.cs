/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
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

using Serenity.Resources;
using Serenity.Web;

namespace Serenity
{
    public class DirectoryResource : Resource
    {
        #region Constructors - internal
        internal DirectoryResource(ResourcePath path)
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
        }
        #endregion
        #region Fields - Public
        public const string XsltStylesheetUrl = "/resource/serenity/index.xslt";
        #endregion
        #region Methods - Public
        public override void OnRequest(CommonContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

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
                context.Response.Write(ms.ToArray());
            }
        }
        #endregion
        #region Properties - Public
        public override MimeType ContentType
        {
            get
            {
                return new MimeType("text", "xml");
            }
            protected internal set
            {
                throw new InvalidOperationException("Cannot set MimeType for DirectoryResource objects.");
            }
        }
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
