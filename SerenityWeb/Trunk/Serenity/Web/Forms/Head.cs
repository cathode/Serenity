/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Web.Forms
{
    /// <summary>
    /// Represents the head section of a document.
    /// </summary>
    public class Head : Control
    {
        /// <summary>
        /// Overridden. Gets the default name of the head section which is "head".
        /// </summary>
        protected override string DefaultName
        {
            get
            {
                return "head";
            }
        }
    }
}
