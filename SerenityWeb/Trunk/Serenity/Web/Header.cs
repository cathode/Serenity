/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;

namespace Serenity.Web
{
    /// <summary>
    /// Represents a name and a set of values associated with a request or response.
    /// </summary>
    public sealed class Header
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the Header class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public Header(string name, string value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else if (name.Length == 0)
            {
                throw new ArgumentException("Argument 'name' cannot be empty.", "name");
            }
            else if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else if (value == string.Empty)
            {
                throw new ArgumentException("Argument 'value' cannot be empty.", "value");
            }
            this.name = name;
            this.primaryValue = value;
        }
        #endregion
        #region Fields - Private
        private readonly string name;
        private bool complex;
        private List<string> secondaryValues = new List<string>();
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
                if (index > this.secondaryValues.Count || index < 0)
                {
                    throw new IndexOutOfRangeException("Index must be between 0 and "
                        + this.secondaryValues.Count.ToString() + ".");
                }
                return this.secondaryValues[index];
            }
        }
        #endregion
        #region Methods - Public
        /// <summary>
        /// Adds a secondary value to the current Header.
        /// </summary>
        /// <param name="value"></param>
        public void Add(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            this.secondaryValues.Add(value);
        }
        /// <summary>
        /// Adds a range of secondary values to the current Header.
        /// </summary>
        /// <param name="values">The string array containing the secondary values to add.</param>
        public void AddRange(string[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
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
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else if (value.Length == 0)
                {
                    throw new ArgumentException("Argument 'value' cannot be empty.", "value");
                }
                this.primaryValue = value;
            }
        }
        #endregion
    }
}