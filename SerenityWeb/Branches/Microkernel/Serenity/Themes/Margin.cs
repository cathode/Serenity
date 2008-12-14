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

namespace Serenity.Themes
{
    public sealed class Margin : Box<Measurement>
    {
        #region Constructors - Internal
        internal Margin()
        {
            this.bottom = new Measurement();
            this.left = new Measurement();
            this.right = new Measurement();
            this.top = new Measurement();
        }
        #endregion
    }
}
