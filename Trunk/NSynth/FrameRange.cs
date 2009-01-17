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
using System.Collections;
using System.Collections.Generic;

namespace NSynth
{
    /// <summary>
    /// Represents a range of frames.
    /// </summary>
    public struct FrameRange : IEnumerable<uint>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameRange"/> struct.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        public FrameRange(uint start, uint count)
        {
            this.start = start;
            this.count = count;
        }
        #region Fields
        private uint start;
        private uint count;
        #endregion
        #region Methods
        /// <summary>
        /// Enumerates over the frame indices in the current
        /// <see cref="FrameRange"/>.
        /// </summary>
        /// <returns>A range of <see cref="UInt32"/>s representing the
        /// frames in the current <see cref="FrameRange"/>.</returns>
        public IEnumerator<uint> GetEnumerator()
        {
            for (uint i = 0; i < count; i++)
            {
                yield return i + start;
            }
        }
        /// <summary>
        /// Enumerates over the frame indices in the current
        /// <see cref="FrameRange"/>.
        /// </summary>
        /// <returns>A range of <see cref="UInt32"/>s representing the
        /// frames in the current <see cref="FrameRange"/>.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the 0-aligned frame number that marks the first frame
        /// in the current <see cref="FrameRange"/>.
        /// </summary>
        public uint Start
        {
            get
            {
                return this.start;
            }
            set
            {
                this.start = value;
            }
        }
        /// <summary>
        /// Gets or sets the number of frames in the current
        /// <see cref="FrameRange"/>.
        /// </summary>
        public uint Count
        {
            get
            {
                return this.count;
            }
            set
            {
                this.count = value;
            }
        }
        #endregion
    }
}
