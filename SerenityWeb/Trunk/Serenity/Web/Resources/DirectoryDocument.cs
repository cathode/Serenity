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
using System.Linq;
using System.Text;

using Serenity.Web;
using Serenity.Web.Forms;

namespace Serenity.Web.Resources
{
    public class DirectoryDocument : Document
    {
        public DirectoryDocument(ResourcePath path)
        {
            this.Body.Controls.Add(new Heading()
            {
                Style = "main_heading",
                Id = "main_heading",
            });
            this.Doctype = Doctype.XHTML11;
            SortedDictionary<string, List<Resource>> groupedResources = new SortedDictionary<string, List<Resource>>();

            foreach (Resource resource in SerenityServer.Resources.GetChildren(path, true))
            {
                if (!groupedResources.ContainsKey(resource.Grouping.PluralForm))
                {
                    groupedResources.Add(resource.Grouping.PluralForm, new List<Resource>());
                }
                groupedResources[resource.Grouping.PluralForm].Add(resource);
            }

            foreach (var group in groupedResources)
            {
                Division section = new Division();
                Heading h = new Heading(HeadingLevel.H2);
                h.Controls.Add(new TextControl(group.Key));
                section.Controls.Add(h);

                Table tbl = new Table();
                tbl.Controls.Add(new TableRow(new TableHeader()
                {
                    Classification = "icon",
                },
                    new TableHeader(new TextControl("Name"))
                    {
                        Id = "name"
                    }, new TableHeader(new TextControl("Size"))
                    {
                        Id = "size"
                    }, new TableHeader(new TextControl("Description"))
                    {
                        Id = "description"
                    }, new TableHeader(new TextControl("Timestamp"))
                    {
                        Id = "timestamp",
                    }));
                foreach (var item in group.Value)
                {
                    TableRow row = new TableRow();

                    row.Controls.Add(new TableCell(new Image(new Uri(FileTypeRegistry.GetIcon(System.IO.Path.GetExtension(item.Name)), UriKind.Relative))));

                    tbl.Controls.Add(row);
                }
                section.Controls.Add(tbl);

                this.Body.Controls.Add(section);
            }
        }
    }
}
