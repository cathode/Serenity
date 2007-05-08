/*
Serenity - The next evolution of web server technology
Serenity/Hdf/HdfElement.cs
Copyright © 2006-2007 Serenity Project (http://serenityproject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Serenity.Hdf
{
    /// <summary>
    /// Represents an element in an HDF tree.
    /// </summary>
    public class HdfElement : IEnumerable<HdfElement>
    {
        #region Constructors - Internal
        internal HdfElement()
        {
            this.dataset = null;
            this.name = null;
            this.parent = null;
            this.value = null;
        }
        internal HdfElement(string name, HdfDataset dataset) : this(name, dataset, null)
        {
        }
        internal HdfElement(string name, HdfDataset dataset, HdfElement parent)
        {
            this.Name = HdfPath.GetName(name);
            this.Dataset = dataset;
            this.Parent = parent;
        }
        #endregion
        #region Fields - Private
        private HdfCollection children = new HdfCollection();
        private HdfDataset dataset;
        private string name;
        private HdfElement parent;
        private string value;
        #endregion
        #region Indexers - Public
        /// <summary>
        /// Gets the HdfElement with the supplied path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public HdfElement this[string path]
        {
            get
            {
                if (!string.IsNullOrEmpty(path))
                {
                    HdfElement element = this;

                    foreach (string item in HdfPath.EnumeratePath(path))
                    {
                        if (element != null)
                        {
                            if (element.children.Contains(item))
                            {
                                element = element.children[item];
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    return (HdfElement)element;
                }
                else
                {
                    throw new ArgumentException("Supplied path string cannot be null or empty");
                }
            }
        }
        #endregion
        #region Methods - Protected
        protected virtual void Link(HdfElement element)
        {
            this.dataset = element.dataset;
            this.parent = element;
        }
        #endregion
        #region Methods - Public
        public virtual void Add(HdfElement element)
        {
            if (!(element is HdfDataset))
            {
                if ((!this.children.Contains(element)) && (!this.children.Contains(element.Name)))
                {
                    this.children.Add(element);
                    element.Link(this);
                }
            }
        }
        /// <summary>
        /// Gets an enumerator that enumerates over child elements of the current HdfElement.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<HdfElement> GetEnumerator()
        {
            return this.children.GetEnumerator();
        }
        #endregion
        #region Properties - Public
        public virtual HdfDataset Dataset
        {
            get
            {
                return this.dataset;
            }
            internal set
            {
                this.dataset = value;
            }
        }
        /// <summary>
        /// Gets a boolean value which indicates if the current HdfElement has any children.
        /// </summary>
        public virtual bool HasChildren
        {
            get
            {
                return (this.children.Count == 0) ? false : true;
            }
        }
        /// <summary>
        /// Gets a boolean value which indicates if the current HdfElement has a parent HdfElement.
        /// </summary>
        public virtual bool HasParent
        {
            get
            {
                return (this.Parent == null) ? false : true;
            }
        }
        /// <summary>
        /// Gets a boolean value which indicates if the current HdfElement has a value assigned.
        /// </summary>
        public virtual bool HasValue
        {
            get
            {
                return (string.IsNullOrEmpty(this.value)) ? false : true;
            }
        }
        public virtual string Name
        {
            get
            {
                return this.name;
            }
            internal set
            {
                this.name = value;
            }
        }
        public virtual HdfElement Parent
        {
            get
            {
                return this.parent;
            }
            internal set
            {
                this.parent = value;
            }
        }
        /// <summary>
        /// Gets the full path of the current HdfElement.
        /// </summary>
        public virtual string Path
        {
            get
            {
                string path = "";
                HdfElement parent = this.Parent;
                while (parent != null)
                {
                    path = HdfPath.Combine(parent.Name, path);
                    parent = parent.Parent;
                }
                return path;
            }
        }
        /// <summary>
        /// Gets or sets the value associated with the current HdfElement.
        /// </summary>
        public virtual string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
        #endregion
        #region IEnumerable Members
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.children.GetEnumerator();
        }
        #endregion
    }
}
