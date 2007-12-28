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
using System.Collections.ObjectModel;
using System.Text;

namespace Serenity
{
    /// <summary>
    /// Represents a collection of Modules.
    /// </summary>
    public sealed class ModuleCollection : KeyedCollection<string, Module>
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the ModuleCollection class.
        /// </summary>
        public ModuleCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }
        #endregion
        #region Methods - Protected
        /// <summary>
        /// Overridden. Used by KeyedCollection.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override string GetKeyForItem(Module item)
        {
            return item.Name;
        }
        #endregion
    }
}
