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
        /// <param name="name"></param>
        /// <param name="validator"></param>
        public Header(string name, Regex validator)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            else if (name.Length == 0)
                throw new ArgumentException(string.Format(Serenity.Resources.ExceptionMessages.ArgumentCannotBeEmpty, "name"), "name");

            this.name = name;
            this.validator = validator;
        }
        /// <summary>
        /// Initializes a new instance of the Header class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public Header(string name, string value)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            else if (name.Length == 0)
                throw new ArgumentException(string.Format(Serenity.Resources.ExceptionMessages.ArgumentCannotBeEmpty, "name"), "name");
            
            this.name = name;
            this.value = value;
        }
        #endregion
        #region Fields - Private
        private Regex validator;
        private readonly string name;
        private string value;
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
                return this.value ?? string.Empty;
            }
            set
            {
                this.value = value;
            }
        }
        #endregion
    }
}