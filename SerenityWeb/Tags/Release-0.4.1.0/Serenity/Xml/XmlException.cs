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
    /// Represents an exception that occurs inside the XML DOM.
    /// </summary>
    public class XmlException : Exception
    {
        #region Public
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the XmlException class.
        /// </summary>
        /// <param name="Message"></param>
        public XmlException(string Message) : base(Message)
        {

        }
        #endregion
        #endregion
    }
}
