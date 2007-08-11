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
    public class Color : IStyleNode
    {
        #region Constructors - Internal
        internal Color()
            : this(0, 0, 0)
        {
        }
        internal Color(Byte R, Byte G, Byte B)
        {
            this.red = R;
            this.green = G;
            this.blue = B;
        }
        #endregion
        #region Fields - Private
        private Byte blue;
        private Byte green;
        private bool isDefined;
        private Byte red;
        #endregion
        #region Methods - Public
        public void Undefine()
        {
            this.blue = 0;
            this.green = 0;
            this.isDefined = false;
            this.red = 0;
        }
        #endregion
        #region Properties - Public
        public Byte Blue
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
        public Byte Green
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
        public bool IsDefined
        {
            get
            {
                return this.isDefined;
            }
        }
        public Byte Red
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
                return HexEncoder.Convert(new byte[] { this.red, this.green, this.blue });
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