/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
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
				resourceName = string.Join(string.Empty, nameParts).ToLower();
			}
			else if (segments.Length == 2)
			{
				resourceName = string.Empty;
			}
			else
			{
				resourceName = DomainSettings.Current.DefaultResourceName;
			}

			if (resourceName == null)
			{
				// 500 Internal Server Error
				ErrorHandler.Handle(context, StatusCode.Http500InternalServerError);
				return;
			}

			string resourcePath = Path.GetFullPath(Path.Combine(DomainSettings.Current.DocumentRoot,
				SPath.SanitizePath(resourceName.TrimStart('/'))));


			if (resourceName == string.Empty)
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
					CommonResponse response = context.Response;

					response.WriteLine(Doctype.XHTML11.ToString());
					response.WriteLine(@"<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
	<title>Index of " + resourceName + @"</title>
	<link rel='stylesheet' type='text/css' href='/static/index.css' />
</head>
<body>
	<div class='TitleMain'>Index of " + resourceName + @"</div>
	<div class='Title'>Directories:</div>
	<div class='List'>
		<table>
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
				<td class='Icon'><img src='/static/icons/folder.png' alt='x' /></td>
				<td class='Name'><a href='" + parentPath + @"'>Parent Directory</a></td>
				<td class='Time'>- - -</td>
			</tr>");

					}
					string[] dirs = Directory.GetDirectories(resourcePath);
					if (dirs.Length > 0)
					{
						foreach (string dirPath in dirs)
						{
							string dirName = Path.GetFileName(dirPath);
							response.Write(@"			<tr>
				<td class='Icon'><img src='/static/icons/folder.png' alt='x' /></td>
				<td class='Name'><a href='/static" + (resourceName.EndsWith("/") ? resourceName : resourceName + "/") + dirName + "'>" + dirName + @"</a></td>
				<td class='Time'>" + Directory.GetLastWriteTimeUtc(Path.GetFullPath(SPath.Combine(resourcePath, dirName))).ToString() + @"</td>
			</tr>");
						}
					}
					response.WriteLine(@"		</table>
	</div>
	<div class='Title'>Files:</div>
	<div class='List'>
		<table>
			<tr>
				<th></th>
				<th>File Name</th>
				<th>File Size</th>
				<th>File Type</th>
				<th>Last Modified</th>
			</tr>");
					string[] files = Directory.GetFiles(resourcePath);
					foreach (string filePath in files)
					{
						string fileName = Path.GetFileName(filePath);
						FileInfo info = new FileInfo(filePath);
						long fileSize = info.Length;
						response.Write(@"
			<tr>
				<td class='Icon'><img src='/static/icons/" + FileTypeRegistry.GetIcon(Path.GetExtension(fileName)) + @".png' alt='x' /></td>
				<td class='Name'><a href='/static" + resourceName + fileName + "'>" + fileName + @"</a></td>
				<td class='Size'>" + fileSize.ToString() +@"</td>
				<td class='Type'>" + FileTypeRegistry.GetDescription(Path.GetExtension(fileName)) + @"</td>
				<td class='Time'>" + File.GetLastWriteTimeUtc(filePath).ToString() + @"</td>
			</td>
			</tr>");
					}
					response.Write(@"		</table>
	</div>
</body>
</html>");
					response.MimeType = MimeType.TextHtml;
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
