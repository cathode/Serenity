/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Web
{
    /// <summary>
    /// Represents a name and a set of values associated with a request or response.
    /// </summary>
    public sealed class Header
    {
        #region Constructors - Public
        public Header(string name, string primaryValue)
        {
            this.name = name;
            this.primaryValue = primaryValue;
            this.secondaryValues = new List<string>();
        }
        #endregion
        #region Fields - Private
        private readonly string name;
        private bool complex = false;
        private List<string> secondaryValues;
        private string primaryValue;
        #endregion
        #region Indexers - Public
        /// <summary>
        /// Gets the header value at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index</param>
        /// <returns>The header value at the specified index.</returns>
        /// <remarks>
        /// Any values less than 0 return the primary value for the header.
        /// </remarks>
        public string this[int index]
        {
            get
            {
                return this.secondaryValues[index];
            }
        }
        #endregion
        #region Methods - Internal
        /// <summary>
        /// Adds the primary key and all secondary keys of the specified header to the secondary keys of the current header.
        /// </summary>
        /// <param name="header"></param>
        internal void Add(Header header)
        {
            this.Add(header.PrimaryValue);
            this.AddRange(header.SecondaryValues);
        }
        #endregion
        #region Methods - Public
        /// <summary>
        /// Adds a secondary value to the current Header.
        /// </summary>
        /// <param name="value"></param>
        public void Add(string value)
        {
            this.secondaryValues.Add(value);
        }
        /// <summary>
        /// Adds a range of secondary values to the current Header.
        /// </summary>
        /// <param name="values">The string array containing the secondary values to add.</param>
        public void AddRange(string[] values)
        {
            if ((values == null) || (values.Length == 0))
            {
                return;
            }
            this.complex = true;
            this.secondaryValues.AddRange(values);
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets a boolean value that indicates whether the current Header
        /// has multiple values (secondary values).
        /// </summary>
        public bool Complex
        {
            get
            {
                return this.complex;
            }
        }
        /// <summary>
        /// Gets a string array containing all the values of the current Header.
        /// </summary>
        public string[] SecondaryValues
        {
            get
            {
                return this.secondaryValues.ToArray();
            }
        }
        /// <summary>
        /// Gets the name of the current Header.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        /// <summary>
        /// Gets or sets the primary value of the current Header.
        /// </summary>
        public string PrimaryValue
        {
            get
            {
                return this.primaryValue;
            }
            set
            {
                this.primaryValue = value;
            }
        }
        #endregion
    }
}