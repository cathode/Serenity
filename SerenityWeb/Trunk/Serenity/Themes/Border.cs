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
    /// <summary>
    /// Represents the border properties of a Style.
    /// </summary>
    public sealed class Border : Box<BorderSide>
    {
        #region Constructors - Internal
        internal Border()
        {
            this.bottom = new BorderSide();
            this.left = new BorderSide();
            this.right = new BorderSide();
            this.top = new BorderSide();
        }
        #endregion
    }
}
