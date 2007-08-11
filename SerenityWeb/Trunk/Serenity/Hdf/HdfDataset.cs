/*
Serenity - The next evolution of web server technology
Serenity/Hdf/HdfDataset.cs
Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace Serenity.Hdf
{
    /// <summary>
    /// Represents the root of an HDF tree.
    /// </summary>
    public sealed class HdfDataset : HdfElement
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the HdfDataset class.
        /// </summary>
        public HdfDataset()
        {

        }
        #endregion
        #region Fields - Private
        private bool isCaseSensitive = false;
        #endregion
        #region Methods - Public
        public HdfElement CreateElement(string name)
        {
            return new HdfElement(name, this);
        }
        public HdfElement CreateElement(string name, string value)
        {
            HdfElement element = new HdfElement(name, this);
            element.Value = value;

            return element;
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Overridden. Returns the current dataset.
        /// </summary>
        public override HdfDataset Dataset
        {
            get
            {
                return this;
            }
            internal set
            {
            }
        }
        /// <summary>
        /// Overridden. Returns false; datasets never have a parent.
        /// </summary>
        public override bool HasParent
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// Overridden. Returns false; datasets never have a value string.
        /// </summary>
        public override bool HasValue
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// Gets or sets a value which determines if names and path comparisons should be case sensitive or not.
        /// </summary>
        public bool IsCaseSensitive
        {
            get
            {
                return this.isCaseSensitive;
            }
            set
            {
                this.isCaseSensitive = value;
            }
        }
        /// <summary>
        /// Overridden. Returns null; datasets never have a parent.
        /// </summary>
        public override HdfElement Parent
        {
            get
            {
                return null;
            }
            internal set
            {
            }
        }
        /// <summary>
        /// Overridden. Returns the value of HdfPath.EmptyPath because; datasets have no path.
        /// </summary>
        public override string Path
        {
            get
            {
                return HdfPath.EmptyPath;
            }
        }
        /// <summary>
        /// Overridden. Returns an empty string; datasets have no value.
        /// </summary>
        public override string Value
        {
            get
            {
                return "";
            }
            set
            {
            }
        }
        
        #endregion
    }
}