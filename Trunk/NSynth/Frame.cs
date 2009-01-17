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
    /// Represents a frame within a video file.
    /// </summary>
    public class Frame
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Frame"/> class.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Frame(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.pixels = new ColorRGBA32[width, height];
        }
        #endregion
        #region Fields
        private int width;
        private int height;
        private ColorRGBA32[,] pixels;
        #endregion
        #region Methods

        #endregion
        #region Properties
        public int Height
        {
            get
            {
                return this.height;
            }
        }
        public int Width
        {
            get
            {
                return this.width;
            }
        }
        #endregion
    }
}
