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

                if (request.Cookies.Contains("session_id"))
                {
                    writer.WriteElementString("p", "Welcome back, " + request.Cookies["session_id"].Value + ".");
                }
                else
                {
                    writer.WriteElementString("p", "Welcome, anonymous.");

                    Session s = Session.NewSession();

                    Cookie c = new Cookie();
                    c.Name = "session_id";
                    c.Value = s.SessionID.ToString("d");
                    c.ExpiresOn = DateTime.Now + TimeSpan.FromMinutes(1);

                    response.Cookies.Add(c);

                    writer.WriteElementString("p", "Your new session id is " + c.Value + ", and will expire at " + c.ExpiresOn.ToString() + ".");
                }

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
