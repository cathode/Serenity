/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://serenityproject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Themes
{
    /// <summary>
    /// Represents a "box" of a generic type, with top, bottom, left, and right sides.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Box<T> : IStyleNode where T : IStyleNode
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
        #region Methods - Public
        public void Undefine()
        {
            this.bottom.Undefine();
            this.left.Undefine();
            this.right.Undefine();
            this.top.Undefine();
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets a boolean value that indicates if any of the sides of the current Box are defined.
        /// </summary>
        public bool IsDefined
        {
            get
            {
                if ((this.Bottom.IsDefined == true)
                    || (this.Left.IsDefined == true)
                    || (this.Right.IsDefined == true)
                    || (this.Top.IsDefined == true))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
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
