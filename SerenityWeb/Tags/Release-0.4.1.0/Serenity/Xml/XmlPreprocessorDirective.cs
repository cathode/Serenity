/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
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