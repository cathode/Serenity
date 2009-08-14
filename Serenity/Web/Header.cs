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
using System.Text.RegularExpressions;

namespace Serenity.Web
{
    /// <summary>
    /// Represents a name and a set of values associated with a request or response.
    /// </summary>
    public sealed class Header
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the name parameter is null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the name parameter is invalid as a header name.</exception>
        public Header(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            else if (name.Length == 0)
                throw new ArgumentException(string.Format("Argument '{0}' cannot be empty", "name"), "name");

            this.name = name;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <param name="validator">A <see cref="System.Text.Regex"/> instance that is used to validate header values.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the name parameter is null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the name parameter is invalid as a header name.</exception>
        public Header(string name, Regex validator)
            : this(name)
        {
            this.validator = validator;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <param name="value">An initial value for the new header.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the name parameter is null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the name parameter is invalid as a header name.</exception>
        public Header(string name, string value)
            : this(name)
        {
            this.Value = value;
        }
        #endregion
        #region Fields - Private
        private Regex validator;
        private readonly string name;
        private string value = string.Empty;
        #endregion
        #region Methods - Public
        /// <summary>
        /// Overridden. Converts the current <see cref="Header"/> to it's string representation.
        /// </summary>
        /// <returns>A string representation of the current <see cref="Header"/>.</returns>
        public override string ToString()
        {
            return this.Name + ": " + this.Value;
        }
        #endregion
        #region Properties - Public
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
        /// Gets or sets the value of the current Header.
        /// </summary>
        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value ?? string.Empty;
            }
        }
        #endregion
    }
}