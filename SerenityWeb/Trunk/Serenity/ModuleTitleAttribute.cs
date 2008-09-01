/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright � 2006-2008 Serenity Project - http://SerenityProject.net/       *
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
    /// Provides a way to specify the title of a module.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class ModuleTitleAttribute : Attribute
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the ModuleTitleAttribute class.
        /// </summary>
        /// <param name="title"></param>
        public ModuleTitleAttribute(string title)
        {
            this.title = title;
        }
        #endregion
        #region Fields - Private
        private readonly string title;
        #endregion
        #region Properties - Public
        /// <summary>
        /// Holds the title of the module.
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }
        }
        #endregion
    }
}