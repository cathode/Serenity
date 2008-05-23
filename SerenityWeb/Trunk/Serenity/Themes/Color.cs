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
    public class Color
    {
        #region Constructors - Internal
        internal Color()
            : this(0, 0, 0)
        {
        }
        internal Color(byte red, byte green, byte blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }
        #endregion
        #region Fields - Private
        private byte blue;
        private byte green;
        private bool isDefined;
        private byte red;
        #endregion
        #region Properties - Public
        public byte Blue
        {
            get
            {
                return this.blue;
            }
            set
            {
                this.isDefined = true;
                this.blue = value;
            }
        }
        public byte Green
        {
            get
            {
                return this.green;
            }
            set
            {
                this.isDefined = true;
                this.green = value;
            }
        }
        public byte Red
        {
            get
            {
                return this.red;
            }
            set
            {
                this.isDefined = true;
                this.red = value;
            }
        }
        /// <summary>
        /// Gets the hexadecimal representation of the value of the current Color.
        /// </summary>
        public string Value
        {
            get
            {
                return HexEncoder.Convert(this.red, this.green, this.blue);
            }
            set
            {
                this.isDefined = true;
                string[] Parts = value.Split(' ');
                if (Parts.Length == 3)
                {
                    try
                    {
                        this.red = Byte.Parse(Parts[0]);
                        this.green = Byte.Parse(Parts[1]);
                        this.blue = Byte.Parse(Parts[2]);
                    }
                    catch
                    {

                    }
                }
                else if (Parts.Length == 1)
                {
                    byte[] Values = HexEncoder.Convert(value);
                    if (Values.Length == 3)
                    {
                        this.red = Values[0];
                        this.green = Values[1];
                        this.blue = Values[2];
                    }
                }
            }
        }
        #endregion
    }
}