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

namespace Serenity.Web.Forms.Controls
{
    public class Anchor : Control
    {
        #region Constructors - Public
        public Anchor()
        {
        }
        public Anchor(params Control[] controls)
            : base(controls)
        {
        }
        #endregion
        #region Fields - Private
        private Uri target;
        #endregion
        #region Properties - Protected
        protected override string DefaultName
        {
            get
            {
                return "a";
            }
        }
        #endregion
        #region Properties - Public
        public Uri Target
        {
            get
            {
                return this.target;
            }
        }
        #endregion
    }
}
