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

namespace Serenity
{
    public sealed class EventRecordedEventArgs : EventArgs
    {
        #region Constructors
        public EventRecordedEventArgs(EventDetails details)
        {
            this.details = details;
        }
        #endregion
        #region Fields
        private readonly EventDetails details;
        #endregion
        #region Properties
        public EventDetails Details
        {
            get
            {
                return this.details;
            }
        }
        #endregion
    }
}
