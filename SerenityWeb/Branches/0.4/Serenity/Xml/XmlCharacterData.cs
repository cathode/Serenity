/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright � 2006-2007 Serenity Project - http://SerenityProject.net/       *
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
    public class XmlCharacterData : XmlNode
    {
        public XmlCharacterData(string innerText) : base("")
        {
            this.Value = innerText;
        }
        public override string OuterMarkup
        {
            get
            {
                return "<![CDATA[" + this.Value + "]]>";
            }
        }
    }
}
