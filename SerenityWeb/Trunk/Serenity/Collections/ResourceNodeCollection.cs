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

namespace Serenity.Collections
{
    public class ResourceNodeCollection : KeyedCollection<string, ResourceNode>
    {
        public ResourceNodeCollection(bool pathIndexing)
            : base(SerenityStringComparer.Instance)
        {
            this.pathIndexing = pathIndexing;
        }
        #region Fields - Private
        private readonly bool pathIndexing;
        #endregion
        #region Methods - Public
        protected override string GetKeyForItem(ResourceNode item)
        {
            return (this.pathIndexing) ? item.Path : item.Name;
        }
        #endregion
    }
}
