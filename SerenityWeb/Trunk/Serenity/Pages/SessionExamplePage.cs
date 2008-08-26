using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serenity.Web;
using Serenity.Web.Resources;
using Serenity.Web.Forms;
using System.Xml;

namespace Serenity.Pages
{
    public class SessionExamplePage : DynamicResource 
    {
        public SessionExamplePage()
        {
            this.Name = "SessionExample";
            this.ContentType = MimeType.TextHtml;
        }
        public override void OnRequest(Request request, Response response)
        {
            using (System.IO.StringWriter stringWriter = new System.IO.StringWriter())
            {
                XmlTextWriter writer = new XmlTextWriter(stringWriter);

                writer.WriteStartDocument();
                writer.WriteStartElement("html");
                writer.WriteStartElement("head");
                writer.WriteElementString("title", "Session Example");
                writer.WriteEndElement();
                writer.WriteStartElement("body");

                writer.WriteElementString("h1", "Session Example");

                writer.WriteEndDocument();

                writer.Flush();
                response.Write(stringWriter.ToString());
            }
        }
        public override void PostRequest(Request request, Response response)
        {
            base.PostRequest(request, response);

            
        }
    }
}
