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
using System.Text;
using System.Xml;

using Serenity.Collections;
using Serenity.Resources;
using Serenity.Web;

namespace Serenity
{
    public class DirectoryResource : Resource
    {
        #region Constructors - internal
        internal DirectoryResource(ResourceNode node)
        {
            this.node = node;
            this.Name = node.Name;
            this.WebPath = node.Path.ToString();
        }
        #endregion
        #region Fields - Private
        private ResourceNode node;
        #endregion
        #region Fields - Public
        public const string XsltStylesheetUrl = "/resource/serenity/index.xslt";
        #endregion
        #region Methods - Public
        public override void OnRequest(CommonContext context)
        {
            Domain domain = context.Domain;

            if (domain == null)
            {
                ErrorHandler.Handle(context, StatusCode.Http500InternalServerError);
                return;
            }

            // collect data
            Dictionary<string, List<Resource>> groupedResources = new Dictionary<string, List<Resource>>();

            foreach (Resource resource in this.node.Resources)
            {
                if (!groupedResources.ContainsKey(resource.Grouping.PluralForm))
                {
                    groupedResources.Add(resource.Grouping.PluralForm, new List<Resource>());
                }
                groupedResources[resource.Grouping.PluralForm].Add(resource);
            }

            foreach (ResourceNode resourceNode in this.node.Nodes)
            {
                if (!groupedResources.ContainsKey(resourceNode.DirectoryResource.Grouping.PluralForm))
                {
                    groupedResources.Add(resourceNode.DirectoryResource.Grouping.PluralForm, new List<Resource>());
                }
                groupedResources[resourceNode.DirectoryResource.Grouping.PluralForm].Add(resourceNode.DirectoryResource);
            }

            // output data
            StringBuilder output = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(output);
            // <?xml version="1.0" encoding="utf-8" ?>
            writer.WriteStartDocument();
            // <?xsl
            writer.WriteProcessingInstruction("xml-stylesheet", "type='text/xsl' href='" + DirectoryResource.XsltStylesheetUrl + "'");
            // <index>
            writer.WriteStartElement("index");
            //   <location>Node Path</location>
            writer.WriteStartElement("location");
            writer.WriteString(this.node.Path.ToString());
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
                    writer.WriteAttributeString("link", res.WebPath);
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

            context.Response.Write(output.ToString());
        }
        #endregion
        #region Properties - Public
        public override MimeType ContentType
        {
            get
            {
                return MimeType.ApplicationXml;
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
