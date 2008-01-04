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
using System.Text;

namespace Serenity.Web.Drivers
{
    /// <summary>
    /// Represents a collection of options used to initialize a WebDriver with.
    /// </summary>
    public sealed class WebDriverSettings
    {
        #region Fields - Private
        private IEnumerable<ushort> ports;
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets or sets a list of port numbers that should be used,
        /// in the event that the primary listen port is already in use.
        /// </summary>
        public IEnumerable<ushort> Ports
        {
            get
            {
                return this.ports;
            }
            set
            {
                this.ports = value;
            }
        }

        #endregion
    }
}