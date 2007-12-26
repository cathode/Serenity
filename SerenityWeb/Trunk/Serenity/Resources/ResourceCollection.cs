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
    public sealed class ResourceCollection : KeyedCollection<ResourcePath, Resource>
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the ResourceCollection class.
        /// </summary>
        public ResourceCollection()
        {
        }
        #endregion
        #region Methods - Protected
        /// <summary>
        /// Overridden. Returns the SystemName property of the supplied Resource.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override ResourcePath GetKeyForItem(Resource item)
        {
            return item.Uri;
        }
        #endregion
        #region Methods - Public
        public IEnumerable<Resource> GetChildren(ResourcePath parentUri)
        {
            return this.GetChildren(parentUri, true);
        }
        public IEnumerable<Resource> GetChildren(ResourcePath parentUri, bool immediateOnly)
        {
            foreach (Resource res in this)
            {
                if (immediateOnly)
                {
                    if (res.Uri.ToString().StartsWith(parentUri.ToString())
                        && res.Uri.Depth == parentUri.Depth + 1)
                    {
                        yield return res;
                    }
                }
                else
                {
                    if (res.Uri.ToString().StartsWith(parentUri.ToString()))
                    {
                        yield return res;
                    }
                }
            }
        }
        #endregion
        
    }
}