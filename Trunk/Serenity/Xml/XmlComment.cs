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
