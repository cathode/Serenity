/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://serenityproject.net/)

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
    /// Represents a HyperText Markup Language document.
    /// </summary>
    public class HtmlDocument : XmlDocument
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the HtmlDocument class.
        /// </summary>
        public HtmlDocument()
        {
            this.DocumentElement = new HtmlElement("html", this);
            this.bodyElement = new HtmlElement("body", this);
            this.headElement = new HtmlElement("head", this);
            this.titleElement = new HtmlElement("title", this);
            this.headElement.AppendChild(this.titleElement);
            this.DocumentElement.AppendChild(this.headElement);
            this.DocumentElement.AppendChild(this.bodyElement);
        }
        #endregion
        #region Fields - Private
        private HtmlElement bodyElement;
        private string defaultClass;
        private string defaultStyle;
        private string defaultTitle;
        private bool hasDefaultClass = false;
        private bool hasDefaultStyle = false;
        private bool hasDefaultTitle = false;
        private HtmlElement headElement;
        private HtmlElement titleElement;
        #endregion
        #region Methods - Public
        /// <summary>
        /// Adds a reference to a stylesheet located at StylesheetUrl to the current HtmlDocument.
        /// </summary>
        /// <param name="url">An absolute or relative path to the stylesheet.</param>
        public void AddStylesheet(string url)
        {
            HtmlElement e = new HtmlElement("link", this);
            e.AppendAttribute("rel", "stylesheet");
            e.AppendAttribute("type", "text/css");
            e.AppendAttribute("href", url);
            this.headElement.AppendChild(e);
        }
        /// <summary>
        /// Creates a new anchor element.
        /// </summary>
        /// <param name="url">The URL which the new anchor will point at.</param>
        /// <param name="innerText">The text to use for the anchor.</param>
        /// <returns>The created element.</returns>
        public HtmlElement CreateAnchor(string url, string innerText)
        {
            HtmlElement e = new HtmlElement("a", this);
            e.AppendAttribute("href", url);
            e.InnerText = innerText;
            
            return e;
        }
        /// <summary>
        /// Creates a new anchor element.
        /// </summary>
        /// <param name="url">The URL which the new anchor will point at.</param>
        /// <param name="innerText">The text to use for the anchor.</param>
        /// <param name="style">A Style to apply to the new element.</param>
        /// <returns>The created element.</returns>
        public HtmlElement CreateAnchor(string url, string innerText, Style style)
        {
            HtmlElement e = new HtmlElement("a", this);
            e.AppendAttribute("href", url);
            e.InnerText = innerText;
            e.Class = style.Class;

            return e;
        }
        /// <summary>
        /// Creates a new linebreak.
        /// </summary>
        /// <returns>The created element.</returns>
        public HtmlElement CreateBreak()
        {
            HtmlElement e = new HtmlElement("br", this);
            e.Class = "";
            return e;
        }
        /// <summary>
        /// Creates a new button element.
        /// </summary>
        /// <param name="innerText">The text used for the button.</param>
        /// <returns>The created element.</returns>
        public HtmlElement CreateButton(string innerText)
        {
            HtmlElement e = new HtmlElement("button", this);
            e.InnerText = innerText;
            return e;
        }
        /// <summary>
        /// Creates a new division (div) element.
        /// </summary>
        /// <returns>The created element.</returns>
        public HtmlElement CreateDivision()
        {
            return new HtmlElement("div", this);
        }
        /// <summary>
        /// Creates a new division (div) element.
        /// </summary>
        /// <param name="innerText">A string which will become the InnerText for the new div element.</param>
        /// <returns>The created element.</returns>
        public HtmlElement CreateDivision(string innerText)
        {
            HtmlElement e = new HtmlElement("div", this);
            e.InnerText = innerText;
            return e;
        }
        /// <summary>
        /// Creates a new division (div) element.
        /// </summary>
        /// <param name="innerText">A string which will become the InnerText for the new div element.</param>
        /// <param name="style">A Style to apply to the new element.</param>
        /// <returns>The created element.</returns>
        public HtmlElement CreateDivision(string innerText, Style style)
        {
            HtmlElement e = new HtmlElement("div", this);
            e.InnerText = innerText;
            e.Class = style.Class;
            return e;
        }
        /// <summary>
        /// Creates a new HTML form.
        /// </summary>
        /// <param name="action">The action of the new HTML form,
        /// e.g. the URL which will recieve the contents of the form, when sent by the browser.</param>
        /// <returns>The created element.</returns>
        public HtmlElement CreateForm(string action)
        {
            return this.CreateForm(action, "GET", "multipart/form-data");
        }
        /// <summary>
        /// Creates a new HTML form.
        /// </summary>
        /// <param name="action">The action of the new HTML form,
        /// e.g. the URL which will recieve the contents of the form, when sent by the browser.</param>
        /// <param name="method">The HTTP method which will be used to send the form's data.
        /// Valid values are "GET" and "POST".</param>
        /// <returns>The created element.</returns>
        public HtmlElement CreateForm(string action, string method)
        {
            return this.CreateForm(action, method, "multipart/form-data");
        }
        /// <summary>
        /// Creates a new HTML form.
        /// </summary>
        /// <param name="action">The action of the new HTML form,
        /// e.g. the URL which will recieve the contents of the form, when sent by the browser.</param>
        /// <param name="style">A Style to apply to the new element.</param>
        /// <returns>The created element.</returns>
        public HtmlElement CreateForm(string action, Style style)
        {
            HtmlElement e = this.CreateForm(action);
            e.AddClass(style.Class);

            return e;
        }
        /// <summary>
        /// Creates a new HTML form.
        /// </summary>
        /// <param name="action">The action of the new HTML form,
        /// e.g. the URL which will recieve the contents of the form, when sent by the browser.</param>
        /// <param name="method">The HTTP method which will be used to send the form's data.
        /// Valid values are "GET" and "POST".</param>
        /// <param name="encodingType">The mimetype which indicates the method used to encode the form's data.</param>
        /// <returns>The created element.</returns>
        public HtmlElement CreateForm(string action, string method, string encodingType)
        {
            HtmlElement e = new HtmlElement("form", this);
            e.AppendAttribute("action", action);
            e.AppendAttribute("method", method);
            e.AppendAttribute("enctype", encodingType);

            return e;
        }
        /// <summary>
        /// Creates a new HTML form.
        /// </summary>
        /// <param name="action">The action of the new HTML form,
        /// e.g. the URL which will recieve the contents of the form, when sent by the browser.</param>
        /// <param name="method">The HTTP method which will be used to send the form's data.
        /// Valid values are "GET" and "POST".</param>
        /// <param name="style">A Style to apply to the new element.</param>
        /// <returns></returns>
        public HtmlElement CreateForm(string action, string method, Style style)
        {
            HtmlElement e = this.CreateForm(action, method);
            e.AddClass(style.Class);

            return e;
        }
        public HtmlElement CreateForm(string action, string method, string encodingType, Style style)
        {
            HtmlElement e = this.CreateForm(action, method, encodingType);
            e.AddClass(style.Class);

            return e;
        }
        /// <summary>
        /// Creates a reference to an image using the "img" tag.
        /// </summary>
        /// <param name="url">The location of the image.</param>
        /// <returns>The created element.</returns>
        public HtmlElement CreateImage(string url)
        {
            HtmlElement e = new HtmlElement("img", this);
            e.AppendAttribute("src", url);

            return e;
        }
        /// <summary>
        /// Creates a reference to an image using the "img" tag.
        /// </summary>
        /// <param name="url">The location of the image.</param>
        /// <param name="alternateText">Text to display in place of the image if it cannot be loaded.</param>
        /// <returns>The created element.</returns>
        public HtmlElement CreateImage(string url, string alternateText)
        {
            HtmlElement e = new HtmlElement("img", this);
            e.AppendAttribute("src", url);
            e.AppendAttribute("alt", alternateText);

            return e;
        }
        /// <summary>
        /// Creates a reference to an image using the "img" tag.
        /// </summary>
        /// <param name="url">The location of the image.</param>
        /// <param name="style">A style to apply to the new element.</param>
        /// <returns>The created element.</returns>
        public HtmlElement CreateImage(string url, Style style)
        {
            HtmlElement e = this.CreateImage(url);
            e.AddClass(style.Class);

            return e;
        }
        public HtmlElement CreateImage(string url, string alternateText, Style style)
        {
            HtmlElement e = this.CreateImage(url, alternateText);
            e.AddClass(style.Class);

            return e;
        }
        /// <summary>
        /// Creates a new paragraph element.
        /// </summary>
        /// <returns></returns>
        public HtmlElement CreateParagraph()
        {
            return new HtmlElement("p", this);
        }
        public HtmlElement CreateParagraph(string innerText)
        {
            HtmlElement e = new HtmlElement("p", this);
            e.InnerText = innerText;

            return e;
        }
        public HtmlElement CreateParagraph(string innerText, Style style)
        {
            HtmlElement e = new HtmlElement("p", this);
            e.InnerText = innerText;
            e.Class = style.Class;

            return e;
        }
        public HtmlElement CreateTable()
        {
            return new HtmlElement("table", this);
        }
        public HtmlElement CreateTable(IEnumerable<string> headerNames)
        {
            HtmlElement table = new HtmlElement("table", this);
            HtmlElement headingRow = table.AppendTableRow();
            foreach (string name in headerNames)
            {
                HtmlElement headingElement = new HtmlElement("th", this);
                headingElement.InnerText = name;
                headingRow.AppendChild(headingElement);
            }
            return table;
        }
        public HtmlElement CreateTable(IEnumerable<string> headerNames, Style style)
        {
            HtmlElement e = this.CreateTable(headerNames);
            e.Class = style.Class;

            return e;
        }
        public HtmlElement CreateTableCell()
        {
            return new HtmlElement("td", this);
        }
        public HtmlElement CreateTableCell(string innerText)
        {
            HtmlElement e = new HtmlElement("td", this);
            e.InnerText = innerText;

            return e;
        }
        public HtmlElement CreateTableCell(string innerText, Style style)
        {
            HtmlElement e = new HtmlElement("td", this);
            e.InnerText = innerText;

            return e;
        }
        public HtmlElement CreateTableHeader()
        {
            return new HtmlElement("th", this);
        }
        public HtmlElement CreateTableHeader(string innerText)
        {
            HtmlElement e = new HtmlElement("th", this);
            e.InnerText = innerText;

            return e;
        }
        public HtmlElement CreateTableHeader(string innerText, Style style)
        {
            HtmlElement e = new HtmlElement("th", this);
            e.InnerText = innerText;
            e.Class = style.Class;

            return e;
        }
        public HtmlElement CreateTableRow()
        {
            return new HtmlElement("tr", this);
        }
        public HtmlElement CreateTableRow(IEnumerable<string> rowCells)
        {
            HtmlElement e = new HtmlElement("tr", this);
            foreach (string Item in rowCells)
            {
                e.AppendTableCell(Item);
            }
            return e;
        }
        public override string SaveMarkup()
        {
            return "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">\r\n"
                           + this.DocumentElement.OuterMarkup;
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets the body element of the current HtmlDocument.
        /// </summary>
        public HtmlElement BodyElement
        {
            get
            {
                return this.bodyElement;
            }
        }
        public string DefaultClass
        {
            get
            {
                return this.defaultClass;
            }
            set
            {
                if (string.IsNullOrEmpty(value) == true)
                {
                    this.hasDefaultClass = false;
                }
                else
                {
                    this.hasDefaultClass = true;
                }
                this.defaultClass = value;
            }
        }
        public string DefaultStyle
        {
            get
            {
                return this.defaultStyle;
            }
            set
            {
                if (string.IsNullOrEmpty(value) == true)
                {
                    this.hasDefaultStyle = false;
                }
                else
                {
                    this.hasDefaultStyle = true;
                }
                this.defaultStyle = value;
            }
        }
        public string DefaultTitle
        {
            get
            {
                return this.defaultTitle;
            }
            set
            {
                if (string.IsNullOrEmpty(value) == true)
                {
                    this.hasDefaultTitle = false;
                }
                else
                {
                    this.hasDefaultStyle = true;
                }
                this.defaultTitle = value;
            }
        }
        /// <summary>
        /// Gets a boolean value which indicates if the current
        /// HtmlDocument has a default class assigned.
        /// </summary>
        public bool HasDefaultClass
        {
            get
            {
                return this.hasDefaultClass;
            }
        }
        /// <summary>
        /// Gets a boolean value which indicates if the current
        /// HtmlDocument has a default style assigned.
        /// </summary>
        public bool HasDefaultStyle
        {
            get
            {
                return this.hasDefaultStyle;
            }
        }
        /// <summary>
        /// Gets a boolean value which indicates if the current
        /// HtmlDocument has a default title assigned.
        /// </summary>
        public bool HasDefaultTitle
        {
            get
            {
                return this.hasDefaultTitle;
            }
        }
        /// <summary>
        /// Gets the head element of the current HtmlDocument.
        /// </summary>
        public HtmlElement HeadElement
        {
            get
            {
                return this.headElement;
            }
        }
        /// <summary>
        /// Gets or sets the title of the current HtmlDocument.
        /// </summary>
        public string Title
        {
            get
            {
                return this.titleElement.InnerText;
            }
            set
            {
                this.titleElement.InnerText = value;
            }
        }

        #endregion
    }
}
