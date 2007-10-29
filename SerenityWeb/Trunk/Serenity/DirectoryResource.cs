/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright � 2006-2007 Serenity Project - http://SerenityProject.net/       *
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

namespace Serenity
{
    public class DirectoryResource : Resource
    {
        #region Constructors - Public
        public DirectoryResource(string location)
        {
            if (location.EndsWith("/"))
            {
                this.location = location;
            }
            else
            {
                this.location = location + "/";
            }
        }
        #endregion
        #region Fields - Private
        private string location = "/";
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
            ResourceTreeBranch branch = domain.Resources.GetBranch(this.location);
            Dictionary<string, List<Resource>> groupedResources = new Dictionary<string, List<Resource>>();

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
            writer.WriteProcessingInstruction("xml-stylesheet", "type='text/xsl' href='" + DirectoryResource.XsltStylesheetUrl + "'");
            writer.WriteStartElement("index");
            writer.WriteStartElement("location");
            writer.WriteString(this.location);
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
        #endregion
    }
}
