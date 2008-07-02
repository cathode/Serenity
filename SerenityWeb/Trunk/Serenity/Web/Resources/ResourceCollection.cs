/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
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

namespace Serenity.Web.Resources
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
        private Dictionary<ResourcePath, Resource[]> cachedDirectories;
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
                this.InvalidateCaches();

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
        #region Methods - Private
        private void InvalidateCaches()
        {
            this.cachedDirectories = null;
        }
        private void RebuildCaches()
        {
            if (this.cachedDirectories != null)
            {
                return;
            }
            else
            {
                this.cachedDirectories = new Dictionary<ResourcePath, Resource[]>();

                Dictionary<ResourcePath, List<Resource>> tempCache = new Dictionary<ResourcePath, List<Resource>>();
                foreach (Resource res in this)
                {
                    ResourcePath resPath = res.Path;
                    ResourcePath resParent = resPath.GetParentDirectory();

                    if (resParent == null)
                    {
                        if (!tempCache.ContainsKey(resPath))
                        {
                            tempCache.Add(resPath, new List<Resource>());
                        }
                    }
                    else
                    {
                        if (!tempCache.ContainsKey(resParent))
                        {
                            tempCache.Add(resParent, new List<Resource>());
                        }
                        tempCache[resParent].Add(res);
                    }
                }
                foreach (KeyValuePair<ResourcePath, List<Resource>> pair in tempCache)
                {
                    this.cachedDirectories.Add(pair.Key, pair.Value.ToArray());
                }
            }
        }
        #endregion
        #region Methods - Public
        public void Add(Resource item)
        {
            this.InvalidateCaches();

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
            //TODO: Figure out why duplicate items are being added.
            if (!this.uc.Contains(item.Path))
            {
                this.uc.Add(item);
            }
        }
        public void Clear()
        {
            this.InvalidateCaches();
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
            this.RebuildCaches();

            foreach (Resource res in this.cachedDirectories[parentUri])
            {
                yield return res;
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
            this.InvalidateCaches();

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