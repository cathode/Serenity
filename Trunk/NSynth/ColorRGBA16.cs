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
    /// Represents a color in the <see cref="ColorFormat.RGBA16"/> format.
    /// This type is immutable. NOTE: This color space is not supported yet.
    /// </summary>
    public struct ColorRGBA16 : IColor
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorRGBA16"/> struct.
        /// </summary>
        /// <param name="red">The value of the red channel.</param>
        /// <param name="green">The value of the blue channel.</param>
        /// <param name="blue">The value of the green channel.</param>
        /// <param name="alpha">The value of the alpha channel.</param>
        public ColorRGBA16(byte red, byte green, byte blue, byte alpha)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Fields
        private readonly ushort value;
        #endregion
        #region Methods
        public ColorRGB24 ToRGB24()
        {
            throw new NotImplementedException();
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
        public byte Alpha
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public byte Blue
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public byte Green
        {
            get
            {
                throw new ArgumentNullException();
            }
        }
        public byte Red
        {
            get
            {
                throw new ArgumentNullException();
            }
        }
    }
}
