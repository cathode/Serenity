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
    public sealed class Box
    {
        #region Constructors - Public
        public Box()
        {
        }
        public Box(Measurement all)  
        {
            this.All = all;
        }
        public Box(Measurement left, Measurement top, Measurement right, Measurement bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }
        #endregion
        #region Fields - Private
        private Measurement left;
        private Measurement top;
        private Measurement right;
        private Measurement bottom;
        #endregion
        #region Properties - Public
        /// <summary>
        /// Sets a single measurement to be used for all four sides of the current <see cref="Box"/>.
        /// </summary>
        public Measurement All
        {
            set
            {
                this.bottom = value;
                this.left = value;
                this.right = value;
                this.top = value;
            }
        }
        public Measurement Bottom
        {
            get
            {
                return this.bottom;
            }
            set
            {
                this.bottom = value;
            }
        }
        public Measurement Left
        {
            get
            {
                return this.left;
            }
            set
            {
                this.left = value;
            }
        }
        public Measurement Right
        {
            get
            {
                return this.right;
            }
            set
            {
                this.right = value;
            }
        }
        public Measurement Top
        {
            get
            {
                return this.top;
            }
            set
            {
                this.top = value;
            }
        }
        #endregion
    }
}
