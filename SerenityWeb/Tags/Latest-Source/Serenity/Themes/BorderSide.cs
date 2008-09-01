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
    /// Represents the properties of one side of a border properties collection.
    /// </summary>
    public sealed class BorderSide
    {
        #region Constructors - Internal
        public BorderSide()
        {
            this.color = new Color(0, 0, 0);
            this.width = new Measurement();
            this.borderType = BorderType.None;
        }
        #endregion
        #region Fields - Private
        private BorderType borderType;
        private Color color;
        private Measurement width;
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets or sets the type of the current BorderSide.
        /// </summary>
        public BorderType BorderType
        {
            get
            {
                return this.borderType;
            }
            set
            {
                this.borderType = value;
            }
        }
        /// <summary>
        /// Gets the Color used for the line of the current BorderSide.
        /// </summary>
        public Color Color
        {
            get
            {
                return this.color;
            }
        }        
        /// <summary>
        /// Gets the Width of the line of the current BorderSide.
        /// </summary>
        public Measurement Width
        {
            get
            {
                return this.width;
            }
        }
        #endregion
    }
}
