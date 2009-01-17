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

namespace NSynth.Scripting.Transforms
{
    /// <summary>
    /// Represents a transformation action that adjusts the frame size without
    /// resizing. This transform can be used to crop or add.
    /// </summary>
    public class CanvasTransform : TransformAction
    {
        public override Frame GetFrame(int index)
        {
            throw new NotImplementedException();
        }
    }
}
