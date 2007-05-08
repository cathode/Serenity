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

namespace Serenity.Xml
{
    /// <summary>
    /// Represents an Extensible Markup Language document.
    /// </summary>
    public class XmlDocument
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the XmlDocument class.
        /// </summary>
        public XmlDocument()
        {
            this.xmlDeclaration = new XmlPreprocessorDirective("xml", "version=\"1.0\" encoding=\"utf-8\"");
        }
        #endregion
        #region Fields - Private
        private XmlElement documentElement;
        private XmlPreprocessorDirective xmlDeclaration;
        #endregion
        #region Methods - Public
        /// <summary>
        /// Creates and returns a new XmlElement with the specified Name.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public XmlElement CreateElement(string Name)
        {
            return new XmlElement(Name);
        }
        public XmlElement CreateElement(string name, string innerText)
        {
            XmlElement e = new XmlElement(name);
            e.InnerText = innerText;
            return e;
        }
        /// <summary>
        /// Reads the specified XML file into the current XmlDocument.
        /// </summary>
        /// <param name="FileName">The path to the file to read.</param>
        public void LoadFile(string FileName)
        {
            TryResult<XmlNodeCollection<XmlNode>> Result = XmlReader.TryReadFile(FileName);
            if (Result.IsSuccessful == true)
            {
                XmlNode[] Nodes = Result.Value.ToArray();
                if (Nodes.Length >= 2)
                {
                    if (Nodes[0] is XmlPreprocessorDirective)
                    {
                        this.xmlDeclaration = (XmlPreprocessorDirective)(Nodes[0]);
                    }
                    this.documentElement = (XmlElement)(Nodes[1]);
                }
            }
        }
        /// <summary>
        /// Reads the specified markup content and adds the XmlNodes it contains to the current XmlDocument.
        /// </summary>
        /// <param name="markup"></param>
        public void LoadMarkup(string markup)
        {
            this.documentElement.AppendMarkup(markup);
        }
        public virtual string SaveMarkup()
        {
            StringBuilder Output = new StringBuilder(this.xmlDeclaration.OuterMarkup);
            Output.Append(this.documentElement.OuterMarkup);

            return Output.ToString();
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets or sets the document element for the current XmlDocument.
        /// </summary>
        public XmlElement DocumentElement
        {
            get
            {
                return this.documentElement;
            }
            set
            {
                this.documentElement = value;
            }
        }
        #endregion
    }
}