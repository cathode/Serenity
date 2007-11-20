using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

using Serenity;
using Serenity.Resources;
using Serenity.Web;

namespace Serenity.ExampleModule
{
    public class HashGenerator : DynamicResource
    {
        #region Constructors - Public
        public HashGenerator()
        {
            this.Name = "HashGenerator";
        }
        #endregion
        #region Methods - Public
        public override void OnRequest(CommonContext context)
        {
            StringBuilder builder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.NewLineChars = "\r\n";
            settings.Encoding = Encoding.UTF8;
            using (XmlWriter writer = XmlWriter.Create(builder, settings))
            {
                writer.WriteStartDocument();
                writer.WriteDocType(Doctype.XHTML11.RootElement, Doctype.XHTML11.PublicIdentifier, Doctype.XHTML11.SystemIdentifier, null);

                writer.WriteStartElement("html");
                writer.WriteStartElement("head");
                writer.WriteElementString("title", this.Name + " - Generates MD5 and SHA1 hashes");
                writer.WriteEndElement();

                writer.WriteStartElement("body");

                if (context.Request.RequestData.Contains("s"))
                {
                    writer.WriteElementString("p", "Your hashes are:");

                    string s = context.Request.RequestData["s"].ReadAllText();

                    string sha1Hash = HexEncoder.Convert(SHA1Managed.Create().ComputeHash(Encoding.UTF8.GetBytes(s)));
                    string md5Hash = HexEncoder.Convert(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(s)));
                    writer.WriteStartElement("ul");
                    writer.WriteElementString("li", md5Hash + " (MD5)");
                    writer.WriteElementString("li", sha1Hash + " (SHA1)");
                    writer.WriteEndElement();

                    writer.WriteRaw("<a href=\"" + this.Name + "\">Hash another string.</a>");

                    writer.WriteEndElement();
                }
                else
                {
                    writer.WriteElementString("p", "Enter some text:");

                    writer.WriteStartElement("form");
                    writer.WriteAttributeString("action", this.Name);
                    writer.WriteAttributeString("method", "GET");

                    writer.WriteStartElement("input");
                    writer.WriteAttributeString("type", "text");
                    writer.WriteAttributeString("name", "s");
                    writer.WriteEndElement();

                    writer.WriteStartElement("input");
                    writer.WriteAttributeString("type", "submit");
                    writer.WriteAttributeString("value", "Get Hashes");
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            context.Response.Write(builder.ToString());
        }
        #endregion
        #region Properties - Public
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
        #endregion
    }
}
