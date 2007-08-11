/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.Text;

using Serenity.Themes;

namespace Serenity.Xml.Html
{
    /// <summary>
    /// Represents an element of an HTML document.
    /// </summary>
    public class HtmlElement : XmlElement
    {
        #region Constructors - Internal
        internal HtmlElement(string name, HtmlDocument owningDocument) : base(name)
        {
            this.owningDocument = owningDocument;
            this.classAttribute = new XmlAttribute("class",
                (owningDocument.HasDefaultClass == true) ? owningDocument.DefaultClass : "");
            this.idAttribute = new XmlAttribute("id", "");
            this.styleAttribute = new XmlAttribute("style",
                (owningDocument.HasDefaultStyle == true) ? owningDocument.DefaultStyle : "");
            this.titleAttribute = new XmlAttribute("title",
                (owningDocument.HasDefaultTitle == true) ? owningDocument.DefaultTitle : "");
            this.AppendAttribute(this.classAttribute);
            this.AppendAttribute(this.idAttribute);
            this.AppendAttribute(this.styleAttribute);
            this.AppendAttribute(this.titleAttribute);
        }
        #endregion
        #region Fields - Private
        private XmlAttribute classAttribute;
        private XmlAttribute idAttribute;
        private XmlAttribute styleAttribute;
        private XmlAttribute titleAttribute;
        private HtmlDocument owningDocument;
        #endregion
        #region Methods - Public
        public void AddClass(string className)
        {
            this.Class += " " + className;
        }
        public void AddStyle(string styleInfo)
        {
            this.Style += " " + styleInfo;
        }
        /// <summary>
        /// Appends an anchor to the current HtmlElement.
        /// </summary>
        /// <param name="url">The Uniform Resource Locator of the new anchor</param>
        /// <param name="innerText">The text displayed for the anchor.
        /// If this is null or empty, url is used as the displayed text.</param>
        /// <returns>The appended element.</returns>
        public HtmlElement AppendAnchor(string url, string innerText)
        {
            HtmlElement e = this.owningDocument.CreateAnchor(url, innerText);
            this.AppendChild(e);

            return e;
        }
        public HtmlElement AppendAnchor(string url, string innerText, Style style)
        {
            HtmlElement e = this.owningDocument.CreateAnchor(url, innerText, style);
            this.AppendChild(e);

            return e;
        }
        /// <summary>
        /// Appends a break to the current HtmlElement.
        /// </summary>
        /// <returns>the appended element.</returns>
        public HtmlElement AppendBreak()
        {
            HtmlElement e = this.owningDocument.CreateBreak();
            this.AppendChild(e);

            return e;
        }
        /// <summary>
        /// Appends a division (div) to the current HtmlElement.
        /// </summary>
        /// <returns>The appended element.</returns>
        public HtmlElement AppendDivision()
        {
            HtmlElement e = this.owningDocument.CreateDivision();
            this.AppendChild(e);

            return e;
        }
        public HtmlElement AppendDivision(string innerMarkup)
        {
            HtmlElement e = this.owningDocument.CreateDivision(innerMarkup);
            this.AppendChild(e);

            return e;
        }
        public HtmlElement AppendDivision(string innerMarkup, Style style)
        {
            HtmlElement e = this.owningDocument.CreateDivision(innerMarkup, style);
            this.AppendChild(e);

            return e;
        }
        /// <summary>
        /// Appends a form to the current HtmlElement.
        /// </summary>
        /// <param name="action">An absolute or relative location where the contents of the form will be sent.</param>
        /// <returns></returns>
        public HtmlElement AppendForm(string action)
        {
            HtmlElement e = this.owningDocument.CreateForm(action);
            this.AppendChild(e);

            return e;
        }
        public HtmlElement AppendForm(string action, string method)
        {
            HtmlElement e = this.owningDocument.CreateForm(action, method);
            this.AppendChild(e);

            return e;
        }
        public HtmlElement AppendForm(string action, string method, string encodingType)
        {
            HtmlElement e = this.owningDocument.CreateForm(action, method, encodingType);
            this.AppendChild(e);

            return e;
        }
        public HtmlElement AppendImage(string url)
        {
            HtmlElement e = this.owningDocument.CreateImage(url);
            this.AppendChild(e);

            return e;
        }
        public HtmlElement AppendImage(string url, string alternateText)
        {
            HtmlElement e = this.owningDocument.CreateImage(url, alternateText);
            this.AppendChild(e);

            return e;
        }
        public HtmlElement AppendParagraph()
        {
            HtmlElement e = this.owningDocument.CreateParagraph();
            this.AppendChild(e);

            return e;
        }
        public HtmlElement AppendParagraph(string innerText)
        {
            HtmlElement e = this.owningDocument.CreateParagraph(innerText);
            this.AppendChild(e);

            return e;
        }
        public HtmlElement AppendParagraph(string innerText, Style style)
        {
            HtmlElement e = this.owningDocument.CreateParagraph(innerText, style);
            this.AppendChild(e);

            return e;
        }
        public HtmlElement AppendTable()
        {
            HtmlElement e = this.owningDocument.CreateTable();
            this.AppendChild(e);

            return e;
        }
        public HtmlElement AppendTable(IEnumerable<string> headerNames)
        {
            HtmlElement e = this.owningDocument.CreateTable(headerNames);
            this.AppendChild(e);

            return e;
        }
        public HtmlElement AppendTableCell()
        {
            HtmlElement e = this.owningDocument.CreateTableCell();
            this.AppendChild(e);

            return e;
        }
        public HtmlElement AppendTableCell(string innerText)
        {
            HtmlElement e = this.owningDocument.CreateTableCell(innerText);
            this.AppendChild(e);

            return e;
        }
        public HtmlElement AppendTableHeader()
        {
            HtmlElement e = this.owningDocument.CreateTableHeader();
            this.AppendChild(e);
            return e;
        }
        public HtmlElement AppendTableHeader(string innerText)
        {
            HtmlElement e = this.owningDocument.CreateTableHeader(innerText);
            this.AppendChild(e);

            return e;
        }
        public HtmlElement AppendTableRow()
        {
            HtmlElement E = this.owningDocument.CreateTableRow();
            this.AppendChild(E);

            return E;
        }
        public HtmlElement AppendTableRow(IEnumerable<string> Items)
        {
            HtmlElement E = this.owningDocument.CreateTableRow(Items);
            this.AppendChild(E);

            return E;
        }
        //public HtmlElement AppendTableRow(IEnumerable<string> items, StyleType rowStyle)
        //{

        //}
        //public HtmlElement AppendTableRow(IEnumerable<string> items, StyleType rowStyle, StyleType[] itemStyles)
        //{

        //}
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets or sets the value of the class attribute of the current HtmlElement.
        /// </summary>
        public string Class
        {
            get
            {
                return this.classAttribute.Value;
            }
            set
            {
                this.classAttribute.Value = value;
            }
        }
        /// <summary>
        /// Gets or sets the value of the id attribute of the current HtmlElement.
        /// </summary>
        public string Id
        {
            get
            {
                return this.idAttribute.Value;
            }
            set
            {
                this.idAttribute.Value = value;
            }
        }
        /// <summary>
        /// Gets or sets the value of the style attribute of the current HtmlElement.
        /// </summary>
        public string Style
        {
            get
            {
                return this.styleAttribute.Value;
            }
            set
            {
                this.styleAttribute.Value = value;
            }
        }
        /// <summary>
        /// Gets or sets the value of the title attribute of the current HtmlElement.
        /// </summary>
        public string Title
        {
            get
            {
                return this.titleAttribute.Value;
            }
            set
            {
                this.titleAttribute.Value = value;
            }
        }
        public HtmlDocument OwningDocument
        {
            get
            {
                return this.owningDocument;
            }
        }
        #endregion
    }
}
