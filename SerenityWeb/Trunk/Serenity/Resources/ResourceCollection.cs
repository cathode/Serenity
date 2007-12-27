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
    public sealed class ResourceCollection : IList<Resource>
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
        #region Explicit Interface Implementations
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion
        #region Fields - Private
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
            return this.uc.GetEnumerator();
        }
        public int IndexOf(Resource item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Insert(int index, Resource item)
        {
            this.uc.Insert(index, item);
        }
        public bool Remove(Resource item)
        {
            return this.uc.Remove(item);
        }
        public void RemoveAt(int index)
        {
            this.uc.RemoveAt(index);
        }
        #endregion
        #region Properties - Public
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
    }
}