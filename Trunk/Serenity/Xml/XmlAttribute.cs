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
    /// Represents an attribute of an XmlElement. This class cannot be inherited.
    /// </summary>
    public sealed class XmlAttribute : XmlNode
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the XmlAttribute class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public XmlAttribute(string name, string value) : base(name)
        {
            this.Value = value;
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets the entire markup for the current XmlAttribute.
        /// </summary>
        public override string OuterMarkup
        {
            get
            {
                if (this.Value.Length > 0)
                {
                    return this.Name + "=\"" + this.Value + "\"";
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion
    }
}