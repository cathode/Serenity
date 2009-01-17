/* NSynth - A Non-Linear Media Graph System
 * Copyright © 2009 NSynth Development Team
 *
 * This software is released the terms and conditions of the MIT License,
 * a copy of which can be found in the License.txt file.
 *
 * Contributors:
 * Will 'AnarkiNet' Shelley (AnarkiNet@gmail.com)
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSynth
{
    /// <summary>
    /// Represents a color in the <see cref="ColorFormat.RGB24"/> format.
    /// </summary>
    public struct ColorRGB24 : IColor
    {
        #region Constructors
        /// <summary>
        /// Initialises a new instance of the <see cref="ColorRGB24"/> struct.
        /// </summary>
        /// <param name="red">The value of the red channel.</param>
        /// <param name="green">The value of the green channel.</param>
        /// <param name="blue">The value of the blue channel.</param>
        public ColorRGB24(byte red, byte green, byte blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }
        #endregion
        #region Fields
        private readonly byte blue;
        private readonly byte green;
        private readonly byte red;
        #endregion
        #region Methods
        public override bool Equals(object obj)
        {
            if (obj.GetType().TypeHandle.Equals(typeof(ColorRGB24).TypeHandle))
            {
                return this.Equals((ColorRGB24)obj);
            }
            else if (obj is IColor)
            {
                return this.Equals(((IColor)obj).ToRGB24());
            }
            return false;
        }
        public bool Equals(ColorRGB24 other)
        {
            if (this.Blue == other.Blue && this.Green == other.Green && this.Red == other.Red)
            {
                return true;
            }
            return false;
        }
        #endregion
        #region Properties
        public byte Blue
        {
            get
            {
                return this.blue;
            }
        }
        public byte Green
        {
            get
            {
                return this.green;
            }
        }
        public byte Red
        {
            get
            {
                return this.red;
            }
        }
        #endregion

        #region IColor Members

        public ColorRGB24 ToRGB24()
        {
            return this;
        }

        public ColorRGBA16 ToRGBA16()
        {
            throw new NotImplementedException();
        }

        public ColorRGBA32 ToRGBA32()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
