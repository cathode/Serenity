/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity
{
    /// <summary>
    /// Represents the status of a continuous or maintained operation/action.
    /// </summary>
    public enum OperationStatus
    {
        /// <summary>
        /// Indicates that the operation is newly created and has not carried
        /// out any action so far.
        /// </summary>
        None = 0,
        /// <summary>
        /// Indicates that the operation has been initialized with a set of
        /// parameters which define it's behaviour, but has not carried out any
        /// action so far.
        /// </summary>
        Initialized = 1,
        /// <summary>
        /// Indicates that the operation is in a stopped state. It is not
        /// performing any action.
        /// </summary>
        Stopped = 2,
        /// <summary>
        /// Indicates that the operation is in a started state. It is 
        /// performing it's actions normally.
        /// </summary>
        Started = 3,
    }
}
