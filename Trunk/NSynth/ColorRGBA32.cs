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
    public struct ColorRGBA32 : IColor
    {
        #region IColor Members

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
            return this;
        }

        #endregion
    }
}
