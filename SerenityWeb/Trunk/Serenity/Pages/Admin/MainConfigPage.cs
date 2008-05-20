using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Serenity.Web;
using Serenity.Web.Drivers;
using Serenity.Web.Resources;

namespace Serenity.Pages.Admin
{
    /// <summary>
    /// Provides a dynamic page for configuring general server options.
    /// </summary>
    public sealed class MainConfigPage : DynamicResource
    {
        #region Constructors - Public
        public MainConfigPage()
        {
            this.Name = "MainConfig";
            this.ContentType = MimeType.ApplicationXml;
        }
        #endregion
        #region Fields - Private
        internal const string XslStylesheetUrl = "/resource/serenity/admin/admin.xslt";
        #endregion
        #region Methods - Public
        public override void OnRequest(CommonContext context)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(ms))
                {
                    writer.WriteStartDocument();
                    //<?xml-stylesheet type="text/xsl" href="cdcatalog.xsl"?>
                    writer.WriteProcessingInstruction("xsl-stylesheet", "type=\"text/xsl\" href=\""
                        + XslStylesheetUrl + "\"");
                    writer.WriteStartElement("document");
                    writer.WriteEndDocument();
                }
                context.Response.Write(ms.ToArray());
            }
        }
        #endregion
    }
}
