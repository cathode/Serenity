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

using Serenity.Xml.Html;

namespace Serenity.Themes
{
    /// <summary>
    /// Represents an individual style of a Theme.
    /// </summary>
    public sealed class Style : IStyleNode
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
        #region Methods - Public
        public void Undefine()
        {
            this.backgroundColor.Undefine();
            this.baseStyle = null;
            this.border.Undefine();
            this.custom = "";
            this.isDefined = false;
            this.margin.Undefine();
            this.padding.Undefine();
            this.textColor.Undefine();
            
        }
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
        /// Gets a boolean value that indicates if the current Style has any properties that have been defined.
        /// </summary>
        public bool IsDefined
        {
            get
            {
                if (this.isDefined == false)
                {
                    if ((this.backgroundColor.IsDefined)
                        || (this.border.IsDefined)
                        || (this.margin.IsDefined)
                        || (this.padding.IsDefined)
                        || (this.textColor.IsDefined))
                    {
                        this.isDefined = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
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
        public Color QualifiedBackgroundColor
        {
            get
            {
                if ((this.baseStyle != null) && (this.BackgroundColor.IsDefined == false))
                {
                    return this.baseStyle.QualifiedBackgroundColor;
                }
                else
                {
                    return this.BackgroundColor;
                }
            }
        }
        public Border QualifiedBorder
        {
            get
            {
                if ((this.baseStyle != null) && (this.Border.IsDefined == false))
                {
                    return this.baseStyle.QualifiedBorder;
                }
                else
                {
                    return this.Border;
                }
            }
        }
        public bool QualifiedIsDefined
        {
            get
            {
                if (this.IsDefined == false)
                {
                    if (this.baseStyle != null)
                    {
                        return this.BaseStyle.QualifiedIsDefined;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
        }
        public Margin QualifiedMargin
        {
            get
            {
                if ((this.baseStyle != null) && (this.Margin.IsDefined == false))
                {
                    return this.baseStyle.QualifiedMargin;
                }
                else
                {
                    return this.Margin;
                }
            }
        }
        public Padding QualifiedPadding
        {
            get
            {
                if ((this.baseStyle != null) && (this.Padding.IsDefined == false))
                {
                    return this.baseStyle.QualifiedPadding;
                }
                else
                {
                    return this.Padding;
                }
            }
        }
        public Color QualifiedTextColor
        {
            get
            {
                if ((this.baseStyle != null) && (this.TextColor.IsDefined == false))
                {
                    return this.baseStyle.QualifiedTextColor;
                }
                else
                {
                    return this.TextColor;
                }
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