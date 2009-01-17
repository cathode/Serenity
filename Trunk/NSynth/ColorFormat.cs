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
    /// Represents the available color formats supported by NSynth.
    /// </summary>
    public enum ColorFormat
    {
        /// <summary>
        /// Represents the RGB color space with 24-bits per pixel.
        /// </summary>
        /// <remarks>
        /// Each color channel uses 8-bits per pixel, with no alpha channel.
        /// This is the default color format.
        /// </remarks>
        RGB24 = 0,
        /// <summary>
        /// Represents the RGBA color space with 32-bits per pixel.
        /// </summary>
        /// <remarks>
        /// Each color channel uses 8-bit per pixel, with 8-bit alpha channel.
        /// </remarks>
        RGBA32,
        /// <summary>
        /// Represents the RGB color space with 16-bits per pixel.
        /// </summary>
        /// <remarks>
        /// Each color channel uses 5-bits per pixel, plus a 1-bit alpha.
        /// </remarks>
        RGBA16,
        YUY2,
    }
}
