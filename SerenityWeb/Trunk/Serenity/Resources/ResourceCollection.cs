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
using System.Collections.ObjectModel;
using System.Text;

namespace Serenity.Resources
{
    /// <summary>
    /// Represents a collection of resources.
    /// </summary>
    public sealed class ResourceCollection : ICollection<Resource>, IEnumerable<Resource>
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the ResourceCollection class.
        /// </summary>
        public ResourceCollection()
        {
            this.uc = new UnderlyingResourceCollection();
        }
        #endregion
        #region Fields - Private
        private bool autoMaintainDirectoryResources = true;
        private UnderlyingResourceCollection uc;
        #endregion
        #region Indexers - Public
        public Resource this[int index]
        {
            get
            {
                return this.uc[index];
            }
            set
            {
                this.uc[index] = value;
            }
        }
        public Resource this[ResourcePath path]
        {
            get
            {
                return this.uc[path];
            }
        }
        #endregion
        #region Methods - Public
        public void Add(Resource item)
        {
            if (this.AutoMaintainDirectoryResources)
            {
                ResourcePath path = item.Path.GetParentDirectory();
                if (path != null)
                {
                    if (!this.uc.Contains(path))
                    {
                        DirectoryResource res = new DirectoryResource(path);
                        this.Add(res);
                    }
                }
            }
            this.uc.Add(item);
        }
        public void Clear()
        {
            this.uc.Clear();
        }
        public bool Contains(Resource item)
        {
            return this.uc.Contains(item);
        }
        public bool Contains(ResourcePath path)
        {
            return this.uc.Contains(path);
        }
        public void CopyTo(Resource[] array, int arrayIndex)
        {
            this.uc.CopyTo(array, arrayIndex);
        }
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
                    if (res.Path.ToString().StartsWith(parentUri.ToString())
                        && res.Path.Depth == parentUri.Depth + 1)
                    {
                        yield return res;
                    }
                }
                else
                {
                    if (res.Path.ToString().StartsWith(parentUri.ToString()))
                    {
                        yield return res;
                    }
                }
            }
        }
        public IEnumerator<Resource> GetEnumerator()
        {
            foreach (Resource res in this.uc)
            {
                yield return res;
            }
        }
        public bool Remove(Resource item)
        {
            return this.uc.Remove(item);
        }
        #endregion
        #region Properties - Public
        public bool AutoMaintainDirectoryResources
        {
            get
            {
                return this.autoMaintainDirectoryResources;
            }
            set
            {
                this.autoMaintainDirectoryResources = value;
            }
        }
        public int Count
        {
            get
            {
                return this.uc.Count;
            }
        }
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        #endregion
        #region Types - Private
        private class UnderlyingResourceCollection : KeyedCollection<ResourcePath, Resource>
        {
            protected override ResourcePath GetKeyForItem(Resource item)
            {
                return item.Path;
            }
        }
        #endregion
        #region Explicit Interface Implementations
        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
        #endregion
    }
}