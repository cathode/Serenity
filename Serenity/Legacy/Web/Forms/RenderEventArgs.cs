﻿/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *----------------------------------------------------------------------------*
 * Authors:                                                                   *
 * - Will 'AnarkiNet' Shelley (AnarkiNet@gmail.com): Original Author          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Web.Forms
{
    /// <summary>
    /// Event data for the <see cref="Control.PreRender"/> event.
    /// </summary>
    public sealed class RenderEventArgs : EventArgs
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderEventArgs"/>
        /// class.
        /// </summary>
        /// <param name="context"></param>
        public RenderEventArgs(RenderingContext context)
        {
            this.context = context;
        }
        #endregion
        #region Fields - Private
        private readonly RenderingContext context;
        #endregion
        #region Properties - Public
        public RenderingContext Context
        {
            get
            {
                return this.context;
            }
        }
        #endregion
    }
}
