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
    /// Represents an individual style of a Theme.
    /// </summary>
    public sealed class Style
    {
        #region Constructors - Internal
        internal Style(string Class)
        {
            this.styleClass = Class;
            this.textColor = new Color(0, 0, 0);
            this.backgroundColor = new Color(255, 255, 255);
            this.custom = "";
            this.border = new Border();
            this.margin = new Margin();
            this.padding = new Padding();
        }
        #endregion
        #region Fields - Private
        private Style baseStyle;
        private Color backgroundColor;
        private Border border;
        private string custom;
        private bool isDefined;
        private Margin margin;
        private Padding padding;
        private string styleClass;
        private Color textColor;
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets the Color used as the background color for the current Style.
        /// </summary>
        public Color BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }
        }
        public Style BaseStyle
        {
            get
            {
                return this.baseStyle;
            }
            set
            {
                if (value != this)
                {
                    this.baseStyle = value;
                }
            }
        }
        /// <summary>
        /// Gets the Border properties used for the current Style.
        /// </summary>
        public Border Border
        {
            get
            {
                return this.border;
            }
        }
        /// <summary>
        /// Gets the name of the class that will be used for the current Style.
        /// </summary>
        public string Class
        {
            get
            {
                return this.styleClass;
            }
        }
        /// <summary>
        /// Gets or sets any custom CSS properties for the current Style.
        /// </summary>
        public string Custom
        {
            get
            {
                return this.custom;
            }
            set
            {
                this.isDefined = true;
                this.custom = value;
            }
        }
        /// <summary>
        /// Gets the Margin properties used for the current Style.
        /// </summary>
        public Margin Margin
        {
            get
            {
                return this.margin;
            }
        }
        /// <summary>
        /// Gets the Padding properties used for the current Style.
        /// </summary>
        public Padding Padding
        {
            get
            {
                return this.padding;
            }
        }
        /// <summary>
        /// Gets the Color used as the foreground (text) color for the current Style.
        /// </summary>
        public Color TextColor
        {
            get
            {
                return this.textColor;
            }
        }

        #endregion
    }
}