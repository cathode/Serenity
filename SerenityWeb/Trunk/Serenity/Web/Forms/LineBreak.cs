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
    public sealed class LineBreak : Control
    {
        #region Constructors - Public
        public LineBreak()
        {
        }
        #endregion
        #region Properties - Protected
        protected override bool CanContainAttributes
        {
            get
            {
                return false;
            }
        }
        protected override bool CanContainControls
        {
            get
            {
                return false;
            }
        }
        protected override string DefaultName
        {
            get
            {
                return "br";
            }
        }
        #endregion
    }
}
