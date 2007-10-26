/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Collections
{
    /// <summary>
    /// Represents a tree of resources.
    /// </summary>
    public sealed class ResourceTree
    {
        #region Methods - Public
        /// <summary>
        /// Initializes a new instance of the ResourceTree class.
        /// </summary>
        public ResourceTree()
        {
            this.trunk = new ResourceTreeBranch(this, "/");
        }
        #endregion
        #region Fields - Private
        private ResourceTreeBranch trunk; 
        #endregion
        #region Methods - Public
        /// <summary>
        /// Adds a resource to the current ResourceTree.
        /// </summary>
        /// <param name="relativeUri"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public bool AddResource(string relativeUri, Resource resource)
        {
            return false;
        }
        #endregion
        #region Properties - Public
        public ResourceTreeBranch Trunk
        {
            get
            {
                return this.trunk;
            }
        }
        #endregion
    }

}
