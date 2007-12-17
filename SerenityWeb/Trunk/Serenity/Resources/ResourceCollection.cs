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

namespace Serenity.Resources
{
    /// <summary>
    /// Represents a collection of resources.
    /// </summary>
    public sealed class ResourceCollection : KeyedCollection<string, Resource>
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the ResourceCollection class.
        /// </summary>
        public ResourceCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
          
        }
        #endregion
        #region Methods - Protected
        /// <summary>
        /// Overridden. Returns the SystemName property of the supplied Resource.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override string GetKeyForItem(Resource item)
        {
            return item.Name;
        }
        #endregion
    }
}