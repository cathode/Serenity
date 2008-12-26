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
    public sealed class EventLog
    {
        #region Events
        public event EventHandler<EventRecordedEventArgs> EventRecorded;
        #endregion
        #region Fields
        private bool enabled = true;
        #endregion
        #region Methods
        public void RecordEvent(string message)
        {
            this.RecordEvent(new EventDetails(message));
        }
        public void RecordEvent(string message, EventKind kind)
        {
            this.RecordEvent(new EventDetails(message)
            {
                Kind = kind
            });
        }
        public void RecordEvent(string message, EventKind kind, string stackTrace)
        {
            this.RecordEvent(new EventDetails(message)
            {
                Kind = kind,
                StackTrace = stackTrace
            });
        }
        public void RecordEvent(EventDetails details)
        {
            if (!this.Enabled)
            {
                return;
            }

            if (this.EventRecorded != null)
            {
                this.EventRecorded(this, new EventRecordedEventArgs(details));
            }
        }
        #endregion
        #region Properties
        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                this.enabled = value;
            }
        }
        #endregion
    }
}
