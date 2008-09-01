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
    /// Represents a heading in a document.
    /// </summary>
    public class Heading : Control
    {
        public Heading()
            : this(HeadingLevel.H1)
        {
        }
        public Heading(HeadingLevel level)
        {
            this.Level = level;
        }
        private HeadingLevel level;
        protected override string DefaultName
        {
            get
            {
                return "FIXME";
            }
        }
        public HeadingLevel Level
        {
            get
            {
                return this.level;
            }
            set
            {
                this.level = value;
                this.Name = this.level.ToString().ToLower();
            }
        }
    }
}
