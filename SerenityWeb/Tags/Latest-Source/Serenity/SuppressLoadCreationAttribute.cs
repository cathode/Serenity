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

namespace Serenity
{
    /// <summary>
    /// Used to direct the module loader to ignore the class this attribute is applied to
    /// when building a resource tree from the module.
    /// </summary>
    /// <remarks>
    /// This attribute is useful for special types of resources. For example, resources created
    /// during runtime or after the module is loaded.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SuppressLoadCreationAttribute : Attribute
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the <see cref="SuppressLoadCreationAttribute"/> class.
        /// </summary>
        /// <param name="suppress">True if loading should be suppressed, otherwise false.</param>
        public SuppressLoadCreationAttribute(bool suppress)
        {
            this.suppress = suppress;
        }
        #endregion
        #region Fields - Private
        private readonly bool suppress;
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets a value that indicates if loading should be suppressed for the class this attribute is applied to.
        /// </summary>
        public bool Suppress
        {
            get
            {
                return this.suppress;
            }
        }
        #endregion
    }
}
