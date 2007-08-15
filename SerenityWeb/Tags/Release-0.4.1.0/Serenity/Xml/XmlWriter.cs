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
    /// Provides a way to convert XmlNodes to strings.
    /// </summary>
    public static class XmlWriter
    {
        #region Methods - Public
        /// <summary>
        /// Gets the complete markup of the specified node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetMarkup(XmlNode node)
        {
            if (node != null)
            {
                return node.OuterMarkup;
            }
            else
            {
                return "";
            }
        }

        #endregion
    }
}