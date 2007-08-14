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
	internal sealed class StaticResourceClass : ResourceClass
	{
		#region Constructors - Public
		public StaticResourceClass()
			: base("static")
		{

		}
		#endregion
		#region Methods - Public
		public override void HandleContext(CommonContext context)
		{
			string resourceName;
			string[] segments = context.Request.Url.Segments;
			int n = 1;

			DomainSettings settings = DomainSettings.GetBestMatch(context.Request.Url);

			if (!settings.OmitResourceClass)
			{
				n++;
			}
			if (segments.Length > n)
			{
				string[] nameParts = new string[segments.Length - n];
				Array.Copy(segments, n, nameParts, 0, nameParts.Length);
				resourceName = string.Join("", nameParts).ToLower();
			}
			else if (segments.Length == 2)
			{
				resourceName = "";
			}
			else
			{
				resourceName = DomainSettings.Current.DefaultResource;
			}

			if (resourceName == null)
			{
				// 500 Internal Server Error
				ErrorHandler.Handle(context, StatusCode.Http500InternalServerError);
				return;
			}

			string resourcePath = Path.GetFullPath(Path.Combine(DomainSettings.Current.DocumentRoot,
				SPath.SanitizePath(resourceName.TrimStart('/'))));


			if (resourceName == "")
			{
				resourceName = "/";
			}
			else if (!resourceName.EndsWith("/") && !File.Exists(resourcePath))
			{
				resourceName += "/";
			}

			if (resourceName.EndsWith("/"))
			{
				//Directory request
				if (Directory.Exists(resourcePath) == true)
				{
#if !RAW
					CommonResponse response = context.Response;

					response.WriteLine(Doctype.XHTML11.ToString());
					response.WriteLine(@"<html xmlns='http://www.w3.org/1999/xhtml\'>
<head>
	<title>Index of " + resourceName + @"</title>
	<link rel='stylesheet' type='text/css' href='/static/index.css' />
	<link rel='stylesheet' type='text/css' href='/theme/stylesheet' />
</head>
<body>
	<div class='HeadingA'>Index of " + resourceName + @"</div>
	<div class='HeadingB'>Directories:</div>
	<div class='List'>
		<table class='AccentA'>
			<tr>
				<th></th>
				<th>Directory Name</th>
				<th>Last Modified</th>
			</tr>");
					if (resourceName != "/")
					{
						string[] parts = resourceName.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
						string parentPath = "/static/" + string.Join("/", parts, 0, parts.Length - 1);
						if (!parentPath.EndsWith("/"))
						{
							parentPath += "/";
						}
						response.WriteLine(@"			<tr>
				<td><img src='/static/icons/folder.png' alt='x' /></td>
				<td><a href='" + parentPath + @"'>Parent Directory</a></td>
				<td>- - -</td>
			</tr>");

					}
					string[] dirs = Directory.GetDirectories(resourcePath);
					if (dirs.Length > 0)
					{
						foreach (string dirPath in dirs)
						{
							string dirName = Path.GetFileName(dirPath);
							response.Write(@"			<tr>
				<td><img src='/static/icons/folder.png' alt='x' /></td>
				<td><a href='/static" + (resourceName.EndsWith("/") ? resourceName : resourceName + "/") + dirName + "'>" + dirName + @"</a></td>
				<td>" + Directory.GetLastWriteTimeUtc(Path.GetFullPath(SPath.Combine(resourcePath, dirName))).ToString() + @"</td>
			</tr>");
						}
					}
					response.WriteLine(@"		</table>
	</div>
	<div class='HeadingB'>Files:</div>
	<div class='List'>
		<table>
			<tr>
				<th></th>
				<th>File Name</th>
				<th>File Size</th>
				<th>File Type</th>
				<th>Last Modified</th>
			</tr>");
					
					response.Write(@"		</table>
	</div>
</body>
</html>");
					response.MimeType = MimeType.TextHtml;
#else
					Theme theme = Theme.GetInstance(DomainSettings.Current.Theme);

					HtmlDocument Doc = new HtmlDocument();
					Doc.BodyElement.Class = Theme.CurrentInstance.ContentA.Class;

					Doc.AddStylesheet(Theme.CurrentInstance.StylesheetUrl);
					Doc.AddStylesheet("/static/index.css");
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
							E.AppendImage("/static/icons/folder.png");

							E = Row.AppendTableCell();
							string[] Parts = resourceName.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
							string Name = "/static/" + string.Join("/", Parts, 0, Parts.Length - 1);
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
							E.AppendImage("/static/icons/folder.png");

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
							string ext = Path.GetExtension(FilePath).TrimStart('.');
							string fileType = FileTypeRegistry.GetDescription(ext);
							string fileIcon = "page_white";
							
							HtmlElement row = Table.AppendTableRow();

							HtmlElement cell = row.AppendTableCell();
							cell.Class = "Icon";
							cell.AppendImage("/static/Icons/" + fileIcon + ".png");

							cell = row.AppendTableCell();
							cell.AppendAnchor(Path.GetFileName(FilePath), Path.GetFileName(FilePath), Theme.CurrentInstance.Link);
							cell.Class = "Name";

							row.AppendTableCell("Unknown").Class = "Size";
							row.AppendTableCell(fileType).Class = "Type";
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
#endif
				}
				else
				{
					ErrorHandler.Handle(context, StatusCode.Http404NotFound);
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
					MimeType mimeType = FileTypeRegistry.GetMimeType(Path.GetExtension(resourcePath).Substring(1));

					bool useCompression = false;
					context.Response.UseCompression = useCompression;

					context.Response.MimeType = mimeType;
					context.Response.Status = StatusCode.Http200Ok;

					context.Response.Write(File.ReadAllBytes(resourcePath));
				}
				else
				{
					ErrorHandler.Handle(context, StatusCode.Http404NotFound, resourceName);
				}
			}
		}
		#endregion
	}
}
