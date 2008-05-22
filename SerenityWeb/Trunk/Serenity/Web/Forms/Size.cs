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
    public sealed class Size
    {
        public Size()
        {

        }
        public Size(Measurement width, Measurement height)
        {
            this.width = width;
            this.height = height;
        }
        #region Fields - Private
        private Measurement width;
        private Measurement height;
        #endregion
        #region Properties - Public
        public Measurement Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }
        public Measurement Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
            }
        }
        public static Size Empty
        {
            get
            {
                return new Size();
            }
        }
        #endregion
    }
}
