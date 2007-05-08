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
    /// Represents a section of text contained within an XML tree.
    /// </summary>
    public class XmlTextNode : XmlNode
    {
        #region Constructors - Internal
        /// <summary>
        /// Initializes a new instance of the XmlText class.
        /// </summary>
        /// <param name="innerText"></param>
        internal XmlTextNode(string innerText) : base("XmlText")
        {
            this.Value = innerText;
        }
        #endregion
        #region Fields - Private
        private string entitizedValue;
        #endregion
        #region Methods - Public
        public static string GetEntity(char value)
        {
            string ent;
            switch (value)
            {
                case '<':
                    ent = "lt";
                    break;
                case '>':
                    ent = "gt";
                    break;
                case '"':
                    ent = "quot";
                    break;
                case '\'':
                    ent = "apos";
                    break;
                case '&':
                    ent = "amp";
                    break;

                default:
                    return value.ToString();
            }
            return "&" + ent + ";";
        }
        public static string CreateEntitizedString(string innerText)
        {
            StringBuilder result = new StringBuilder();
            char[] chars = innerText.ToCharArray();
            foreach (char c in chars)
            {
                result.Append(XmlTextNode.GetEntity(c));
            }
            return result.ToString();
        }
        public static string StripEntitizedString(string innerMarkup)
        {
            return innerMarkup.Replace("&amp;", "&").Replace("&apos;", "'").Replace("&quot;", "\"").Replace("&gt;", ">").Replace("&lt;", "<");
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets the entitized value of the text content represented by the current XmlTextNode.
        /// </summary>
        public override string OuterMarkup
        {
            get
            {
                return this.entitizedValue;
            }
        }
        /// <summary>
        /// Gets the raw (real) value of the text content represented by the current XmlTextNode.
        /// </summary>
        public override string Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                this.entitizedValue = XmlTextNode.CreateEntitizedString(value);
                base.Value = value;
            }
        }
        #endregion
    }
}