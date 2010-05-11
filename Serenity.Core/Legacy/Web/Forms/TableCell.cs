﻿/******************************************************************************
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
    public class TableCell : Control
    {
        public TableCell()
        {

        }
        public TableCell(params Control[] contents)
        {
            this.Controls.AddRange(contents);
        }
        public TableCell(string content)
            : this(new TextControl(content))
        {
        }
        protected override string DefaultName
        {
            get
            {
                return "td";
            }
        }
    }
}