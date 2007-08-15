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
    /// Represents a comment in an XML document. This class cannot be inherited.
    /// </summary>
    public sealed class XmlComment : XmlNode
    {
        #region Constructors - Internal
        internal XmlComment(string innerText) : base("")
        {
            this.Value = innerText;
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets the XML markup representing the current XmlComment.
        /// </summary>
        public override string OuterMarkup
        {
            get
            {
                return "<!--" + this.Value + "-->";
            }
        }
        #endregion
    }
}
