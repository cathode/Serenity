/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

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
    /// Represents the properties of one side of a border properties collection.
    /// </summary>
    public sealed class BorderSide : IStyleNode
    {
        #region Constructors - Internal
        internal BorderSide()
        {
            this.color = new Color(0, 0, 0);
            this.width = new Width();
            this.borderType = BorderType.None;
        }
        #endregion
        #region Fields - Private
        private BorderType borderType;
        private bool isDefined;
        private Color color;
        private Width width;
        #endregion
        #region Methods - Public
        /// <summary>
        /// Restores the current BorderSide to it's default state.
        /// </summary>
        public void Undefine()
        {
            this.borderType = BorderType.None;
            this.color.Undefine();
            this.isDefined = false;
            this.width.Undefine();
        }
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
                this.isDefined = true;
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
        /// Gets a boolean value that indicates if the current BorderSide has any properties defined.
        /// </summary>
        public bool IsDefined
        {
            get
            {
                if (this.isDefined == true)
                {
                    return true;
                }
                else
                {
                    if ((this.isDefined == true)
                        || (this.color.IsDefined == true)
                        || (this.width.IsDefined == true))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }        
        /// <summary>
        /// Gets the Width of the line of the current BorderSide.
        /// </summary>
        public Width Width
        {
            get
            {
                return this.width;
            }
        }
        #endregion
    }
}
