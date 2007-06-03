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

namespace Serenity.Xml
{
    /// <summary>
    /// Represents a preprocessor directive in an XML document.
    /// </summary>
    public class XmlPreprocessorDirective : XmlNode
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the XmlPreprocessorDirective class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        public XmlPreprocessorDirective(string name, string content) : base(name)
        {
            this.Value = content;
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets the complete markup for the current XmlPreprocessorDirective.
        /// </summary>
        public override string OuterMarkup
        {
            get
            {
                return "<?" + this.Name + " " + this.Value + " ?>";
            }
        }
        #endregion
    }
}