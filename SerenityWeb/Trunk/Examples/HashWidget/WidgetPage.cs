using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Serenity.Resources;
using Serenity.Web;

namespace HashWidget
{
    public class WidgetPage : DynamicResource
    {
        public override void OnRequest(CommonContext context)
        {
            StringBuilder builder = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(builder))
            {
                writer.WriteStartDocument();
                writer.WriteDocType(Doctype.XHTML11.RootElement, Doctype.XHTML11.PublicIdentifier, Doctype.XHTML11.SystemIdentifier, null);

                writer.WriteStartElement("html");
                writer.WriteStartElement("head");
                writer.WriteElementString("title", "Hash Widget Page");
                writer.WriteEndElement();

                writer.WriteStartElement("body");

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            context.Response.Write(builder.ToString());
        }
        public override MimeType ContentType
        {
            get
            {
                return MimeType.TextHtml;
            }
            protected set
            {
                throw new InvalidOperationException();
            }
        }
    }
}
