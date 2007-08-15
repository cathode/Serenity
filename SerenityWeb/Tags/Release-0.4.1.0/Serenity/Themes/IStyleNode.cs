/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Themes
{
    /// <summary>
    /// Defines method signatures shared by different parts of the Style DOM.
    /// </summary>
    public interface IStyleNode
    {
        /// <summary>
        /// When implemented in an inheriting class, restores the style node to it's default state.
        /// </summary>
        void Undefine();
        /// <summary>
        /// When implemented in an inheriting class, gets a boolean value that indicates
        /// if the current style node has had any part of it be defined yet.
        /// </summary>
        bool IsDefined
        {
            get;
        }
    }
}
