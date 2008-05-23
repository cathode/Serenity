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
    /// Represents a "box", with top, bottom, left, and right values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Box<T>
    {
        #region Constructors - Internal
        internal Box()
        {
        }
        #endregion
        #region Fields - Protected
        protected T top;
        protected T bottom;
        protected T left;
        protected T right;
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets the bottom value.
        /// </summary>
        public T Bottom
        {
            get
            {
                return this.bottom;
            }
        }
        /// <summary>
        /// Gets the left value.
        /// </summary>
        public T Left
        {
            get
            {
                return this.left;
            }
        }
        /// <summary>
        /// Gets the right value.
        /// </summary>
        public T Right
        {
            get
            {
                return this.right;
            }
        }
        /// <summary>
        /// Gets the top value.
        /// </summary>
        public T Top
        {
            get
            {
                return this.top;
            }
        }
        #endregion
    }
}
