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

namespace NSynth.Scripting
{
    public abstract class ScriptAction
    {
        /// <summary>
        /// When overridden in a derived class, gets the frame data for the
        /// specified frame index.
        /// </summary>
        /// <param name="index">The index of the frame to get.</param>
        /// <returns>The frame with the specified index.</returns>
        public abstract Frame GetFrame(int index);

        /// <summary>
        /// Gets the frame data for the specified frame range.
        /// </summary>
        /// <param name="range">The range of frames to get data for.</param>
        /// <returns>The frame data of the specified frame range.</returns>
        public IEnumerable<Frame> GetFrames(FrameRange range)
        {
            foreach (int index in range)
            {
                yield return this.GetFrame(index);
            }
        }
    }
}
