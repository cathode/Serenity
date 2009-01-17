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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NSynth.Scripting;

namespace NSynth.WinForms
{
    public partial class VideoDisplay : Control
    {
        #region Constructors
        public VideoDisplay()
        {

        }
        #endregion
        #region Events
        public event EventHandler<ScriptChangedEventArgs> ScriptChanged;
        #endregion
        #region Fields
        private Script script;
        #endregion
        #region Methods
        /// <summary>
        /// Raises the <see cref="ScriptChanged"/> event.
        /// </summary>
        /// <param name="e">The event data associated with the event.</param>
        protected virtual void OnScriptChanged(ScriptChangedEventArgs e)
        {
            if (this.ScriptChanged != null)
            {
                this.ScriptChanged(this, e);
            }
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the <see cref="Script"/> that is being viewed.
        /// </summary>
        public Script Script
        {
            get
            {
                return this.script;
            }
            set
            {
                this.script = value;
            }
        }
        #endregion
    }
}