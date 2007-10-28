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
using Serenity.Web;

namespace Serenity.Pages
{
    public sealed class IndexerPage : ContentPage
    {
        public const string XsltStylesheetUrl = "/resource/serenity/index.xslt";

        public override void OnRequest(CommonContext context)
        {
            if (context.Request.RequestData.Contains("url"))
            {
                //TODO: Add url input validation.
                string rawUrl = context.Request.RequestData["url"].ReadAllText();
                Uri indexUrl = new Uri(rawUrl, UriKind.Relative);

                Domain domain;
                if (indexUrl.IsAbsoluteUri)
                {
                    domain = this.Server.Domains[indexUrl.Host];
                }
                else
                {
                    domain = context.Domain;
                    indexUrl = new Uri(new Uri("http://" + domain.HostName + "/"), indexUrl);

                }

                if (indexUrl.Segments[indexUrl.Segments.Length - 1].EndsWith("/"))
                {
                    
                    // collect data
                    ResourceTreeBranch branch = domain.Resources.GetBranch(indexUrl);
                    Dictionary<string, List<Resource>> groupedResources = new Dictionary<string,List<Resource>>();

                    foreach (Resource resource in branch.Resources)
                    {
                        if (!groupedResources.ContainsKey(resource.Grouping.PluralForm))
                        {
                            groupedResources.Add(resource.Grouping.PluralForm, new List<Resource>());
                        }
                        groupedResources[resource.Grouping.PluralForm].Add(resource);
                    }

                    // output data
                    StringBuilder output = new StringBuilder();
                    XmlWriter writer = XmlWriter.Create(output);
                    writer.WriteStartDocument();
                    writer.WriteProcessingInstruction("xml-stylesheet", "type='text/xsl' href='" + IndexerPage.XsltStylesheetUrl + "'");
                    writer.WriteStartElement("index");
                    writer.WriteStartElement("location");
                    writer.WriteString(indexUrl.AbsolutePath);
                    writer.WriteEndElement();
                    foreach (KeyValuePair<string, List<Resource>> pair in groupedResources)
                    {
                        writer.WriteStartElement("group");
                        writer.WriteAttributeString("name", pair.Key);

                        writer.WriteStartElement("field");
                    }
                    writer.WriteEndDocument();
                    writer.Flush();
                    writer.Close();

                    context.Response.Write(output.ToString());
                }
            }
        }
        public override MimeType MimeType
        {
            get
            {
                return MimeType.ApplicationXml;
            }
            protected internal set
            {
            }
        }
        public override string Name
        {
            get
            {
                return "Indexer";
            }
            protected internal set
            { 
            }
        }
    }
}
