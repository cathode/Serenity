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
    /// Represents the width of a visual element.
    /// </summary>
    public sealed class Width : IStyleNode
    {
        #region Constructors - Internal
        internal Width()
        {
            this.unit = Unit.Pixels;
            this.auto = true;
        }
        #endregion
        #region Fields - Private
        private bool auto;
        private bool isDefined = false;
        private Unit unit;
        private int value;
        #endregion
        #region Methods - Public
        /// <summary>
        /// Undefines the current Width object.
        /// </summary>
        public void Undefine()
        {
            this.auto = true;
            this.unit = Unit.Pixels;
            this.isDefined = false;
            this.value = 0;
        }
        #endregion
        #region Properties - Public
        public bool Auto
        {
            get
            {
                return this.auto;
            }
            set
            {
                this.isDefined = true;
                this.auto = value;
            }
        }
        public bool IsDefined
        {
            get
            {
                return this.isDefined;
            }
        }

        public Unit Unit
        {
            get
            {
                return this.unit;
            }
            set
            {
                this.isDefined = true;
                this.unit = value;
            }
        }
        public int Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.isDefined = true;
                this.auto = false;
                this.value = value;
            }
        }
        #endregion
    }
}