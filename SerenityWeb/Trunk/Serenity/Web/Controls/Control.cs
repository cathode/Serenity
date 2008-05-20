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
using System.IO;
using System.Xml;

namespace Serenity.Web.Controls
{
    public abstract class Control
    {
        #region Constructors - Protected
        protected Control()
        {

        }
        protected Control(params Control[] children)
        {

        }
        #endregion
        #region Fields - Private
        private string name = Control.DefaultControlName;
        private ControlCollection children;
        #endregion
        #region Fields - Private
        public const string DefaultControlName = "control";
        #endregion
        #region Properties - Public
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
        #endregion
    }
}
