/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;

namespace Serenity.Web.Forms
{
    /// <summary>
    /// Represents a collection of controls.
    /// </summary>
    public sealed class ControlCollection : Collection<Control>
    {
        /// <summary>
        /// Adds a range of controls to the current <see cref="ControlCollection"/>.
        /// </summary>
        /// <param name="controls">The controls to add.</param>
        public void AddRange(IEnumerable<Control> controls)
        {
            foreach (Control c in controls)
            {
                this.Add(c);
            }
        }
        /// <summary>
        /// Adds a range of controls to the current <see cref="ControlCollection"/>.
        /// </summary>
        /// <param name="controls">The controls to add.</param>
        public void AddRange(params Control[] controls)
        {
            foreach (Control c in controls)
            {
                this.Add(c);
            }
        }
    }
}
