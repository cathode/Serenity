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

namespace Serenity.Attributes
{
    /// <summary>
    ///	Provides a way to specify the default page of a given Module.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class ModuleDefaultPageAttribute : Attribute
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the ModuleDefaultPageAttribute class.
        /// </summary>
        /// <param name="name"></param>
        public ModuleDefaultPageAttribute(string name)
        {
            this.Name = name;
        }
        #endregion
        #region Fields - Public
        /// <summary>
        /// Holds the name of the default page for a Module. This field is read-only.
        /// </summary>
        public readonly string Name;
        #endregion
    }
}
