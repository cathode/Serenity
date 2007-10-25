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
using System.Reflection;
using System.Resources;
using System.Text;

using Serenity.Web;

namespace Serenity.ResourceClasses
{
    internal class ResourceResourceClass : ResourceClass
    {
        public ResourceResourceClass()
            : base("resource")
        {

        }
        public override void HandleContext(Serenity.Web.CommonContext context)
        {
            byte[] readBuffer = null;
            DomainSettings settings = DomainSettings.GetBestMatch(context.Request.Url);
            int n = ((settings.OmitResourceClass) ? 1 : 2);
            if (context.Request.Url.Segments.Length > n)
            {
                Module module = Module.GetModule(context.Request.Url.Segments[n].TrimEnd('/'));
                if (module != null)
                {
                    string resourceName = module.ResourceNamespace + string.Join("", context.Request.Url.Segments, n + 1, context.Request.Url.Segments.Length - (n + 1)).Replace('/', '.');
                    string foundResourceName = "";
                    foreach (string existingResourceName in module.Assembly.GetManifestResourceNames())
                    {
                        if (existingResourceName.ToLower() == resourceName.ToLower())
                        {
                            foundResourceName = existingResourceName;
                            break;
                        }
                    }
                    if (foundResourceName != "")
                    {
                        Stream resourceStream = null;
                        try
                        {
                            resourceStream = module.Assembly.GetManifestResourceStream(foundResourceName);
                            readBuffer = new byte[resourceStream.Length];
                            resourceStream.Read(readBuffer, 0, readBuffer.Length);
                            string ext = Path.GetExtension(foundResourceName).TrimStart('.');
                            context.Response.MimeType = FileTypeRegistry.GetMimeType(ext);
                        }
                        catch
                        {
                            ErrorHandler.Handle(context, StatusCode.Http500InternalServerError);
                        }
                        finally
                        {
                            if (resourceStream != null)
                            {
                                resourceStream.Close();
                                resourceStream.Dispose();
                            }
                        }
                    }
                }
            }

            if (readBuffer != null)
            {
                context.Response.Write(readBuffer);
            }
            else
            {
                ErrorHandler.Handle(context, StatusCode.Http404NotFound);
            }
        }
    }
}
