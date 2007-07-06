/*
Serenity - The next evolution of web server technology
Serenity/ResourceClasses/staticResourceClass.cs
Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Serenity.Themes;
using Serenity.Web;
using Serenity.Xml.Html;

namespace Serenity.ResourceClasses
{
    internal sealed class staticResourceClass : ResourceClass
    {
        public staticResourceClass() : base("static")
        {

        }
        public override void HandleContext(CommonContext context)
        {
            string resourceName;
            string[] segments = context.Request.Url.Segments;
            int n = 1;

            if (!DomainSettings.Current.OmitEnvironment.Value)
            {
                n++;
            }
            if (!DomainSettings.Current.OmitResourceClass.Value)
            {
                n++;
            }
            if (segments.Length > n)
            {
                string[] nameParts = new string[segments.Length - n];
                Array.Copy(segments, n, nameParts, 0, nameParts.Length);
                resourceName = string.Join("", nameParts).ToLower();
            }
            else if (segments.Length == 3)
            {
                resourceName = "";
            }
            else
            {
                resourceName = SerenityEnvironment.CurrentInstance.DefaultResourceName;
            }

            if (resourceName == null)
            {
                // 500 Internal Server Error
                context.Response.Status = StatusCode.Http500InternalServerError;
                context.Response.Write("500 Internal Server Error");
                return;
            }

            string resourcePath = Path.Combine(SerenityEnvironment.CurrentInstance.StaticFilesDirectory,
                SPath.SanitizePath(resourceName.TrimStart('/')));

            Theme theme = SerenityEnvironment.CurrentInstance.Theme;

            if (resourceName == "")
            {
                resourceName = "/";
            }
            
            if (resourceName.EndsWith("/") == true)
            {
                //Directory request
                if (Directory.Exists(resourcePath) == true)
                {
                    HtmlDocument Doc = new HtmlDocument();
                    Doc.BodyElement.Class = Theme.CurrentInstance.ContentA.Class;

                    Doc.AddStylesheet(Theme.CurrentInstance.StylesheetUrl);
                    Doc.AddStylesheet("/system/static/index.css");
                    Doc.Title = "Index of " + resourceName;
                    HtmlElement Div = Doc.BodyElement.AppendDivision("Index of " + resourceName, Theme.CurrentInstance.HeadingA);
                    string[] SubDirs = Directory.GetDirectories(resourcePath);
                    if ((SubDirs.Length > 0) || (resourceName != "/"))
                    {
                        Div = Doc.BodyElement.AppendDivision("Directories:", Theme.CurrentInstance.HeadingB);
                        Div = Doc.BodyElement.AppendDivision();
                        Div.AddClass("List");
                        string[] Columns = new string[3] { "", "Directory Name", "Last Modified" };
                        HtmlElement Table = Div.AppendTable(Columns);
                        Table.Class = Theme.CurrentInstance.AccentA.Class;
                        if (resourceName != "/")
                        {
                            HtmlElement Row = Table.AppendTableRow();

                            HtmlElement E = Row.AppendTableCell();
                            E.AddClass("Icon");
                            E.AppendImage("/System/static/icons/folder.png");

                            E = Row.AppendTableCell();
                            string[] Parts = resourceName.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                            string Name = "/" + SerenityEnvironment.CurrentInstance.Key + "/static/" + string.Join("/", Parts, 0, Parts.Length - 1);
                            if (Name.EndsWith("/") == false)
                            {
                                Name += "/";
                            }
                            E.AppendAnchor(Name, "Parent Directory", Theme.CurrentInstance.Link);
                            E.AddClass("Name");
                            Row.AppendTableCell("- - -").AddClass("Time");
                        }
                        foreach (string DirPath in SubDirs)
                        {
                            HtmlElement Row = Table.AppendTableRow();

                            HtmlElement E = Row.AppendTableCell();
                            E.AddClass("Icon");
                            E.AppendImage("/System/static/icons/folder.png");

                            E = Row.AppendTableCell();
                            E.AppendAnchor(Path.GetFileName(DirPath) + "/", Path.GetFileName(DirPath), Theme.CurrentInstance.Link);
                            E.AddClass("Name");
                            Row.AppendTableCell(Directory.GetLastWriteTimeUtc(DirPath).ToString("s")).AddClass("Time");
                        }
                    }
                    string[] Files = Directory.GetFiles(resourcePath);
                    if (Files.Length > 0)
                    {
                        Div = Doc.BodyElement.AppendDivision("Files:", Theme.CurrentInstance.HeadingB);
                        string[] Columns = new string[5] { "", "File Name", "File Size", "File Type", "Last Modified" };
                        Div = Doc.BodyElement.AppendDivision();
                        Div.AddClass("List");
                        HtmlElement Table = Div.AppendTable(Columns);
                        foreach (string FilePath in Files)
                        {
                            string Extension = Path.GetExtension(FilePath).TrimStart('.');
                            string Type = Extension.ToUpper() + " File";
                            string icon = "page_white";
                            switch (Extension)
                            {
                                case "c":
                                    icon = "page_white_c";
                                    Type = "C Code File";
                                    break;
                                case "cpp":
                                    icon = "page_white_cplusplus";
                                    Type = "C++ Code File";
                                    break;
                                case "cs":
                                    icon = "page_white_csharp";
                                    Type = "C# Code File";
                                    break;
                                case "dll":
                                    icon = "page_white_gear";
                                    Type = "Dynamic Link Library";
                                    break;
                                case "exe":
                                    icon = "page_white_gear";
                                    Type = "Executable";
                                    break;
                                case "pdf":
                                    icon = "page_white_acrobat";
                                    Type = "PDF Document";
                                    break;
                                case "png":
                                    icon = "page_white_picture";
                                    Type = "PNG Image";
                                    break;
                                case "sql":
                                    icon = "page_white_database";
                                    Type = "SQL Script";
                                    break;
                                case "rar":
                                    icon = "page_white_compressed";
                                    Type = "RAR Archive";
                                    break;
                                case "zip":
                                    icon = "page_white_zip";
                                    Type = "ZIP Archive";
                                    break;
                            }
                            HtmlElement row = Table.AppendTableRow();

                            HtmlElement cell = row.AppendTableCell();
                            cell.Class = "Icon";
                            cell.AppendImage("/System/static/Icons/" + icon + ".png");

                            cell = row.AppendTableCell();
                            cell.AppendAnchor(Path.GetFileName(FilePath), Path.GetFileName(FilePath), Theme.CurrentInstance.Link);
                            cell.Class = "Name";

                            row.AppendTableCell("Unknown").Class = "Size";
                            row.AppendTableCell(Type).Class = "Type";
                            row.AppendTableCell(File.GetLastWriteTimeUtc(FilePath).ToString("s")).Class = "Time";
                        }
                    }
                    HtmlElement FooterDiv = Doc.BodyElement.AppendDivision();
                    FooterDiv.AppendText("Powered by " + SerenityInfo.Name + " - " + SerenityInfo.Copyright);
                    FooterDiv.AppendBreak();
                    FooterDiv.AppendText("Icons by Fam - ");
                    FooterDiv.AppendAnchor("http://famfamfam.com/", "FamFamFam.com");
                    FooterDiv.AddClass("Footer");

                    context.Response.Status = StatusCode.Http200Ok;
                    context.Response.MimeType = MimeType.TextHtml;
                    context.Response.UseCompression = true;
					//BR: Moved things around to make sure that the mimetype and others
					//are getting set before we write anything out.
					context.Response.Write(Doc.SaveMarkup());
                }
                else
                {
                    context.Response.Status = StatusCode.Http404NotFound;
                    context.Response.Write("Requested resource does not exist.");
                }
            }
            else
            {
                //Resource request
                if (File.Exists(resourcePath) == true)
                {
                    //AJ: Cache check goes here
					//BR: As above, rearranged things to make sure that mimetype and other
					//header-related things are going out first.
					MimeType mimeType = MimeType.TextHtml;
					
					bool useCompression = false;
                    context.Response.UseCompression = useCompression;
					
                    context.Response.MimeType = mimeType;
                    context.Response.Status = StatusCode.Http200Ok;
					
                    context.Response.Write(File.ReadAllBytes(resourcePath));
                }
                else
                {
                    context.Response.Status = StatusCode.Http404NotFound;
                    context.Response.Write("Requested resource does not exist.");
                }
            }
        }
    }
}
