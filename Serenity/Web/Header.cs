/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics.Contracts;

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
        public Header(string name)
        {
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
            Contract.Requires(!string.IsNullOrEmpty(name));

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

        [ContractInvariantMethod]
        private void __InvariantMethod()
        {
            //Contract.Invariant(!string.IsNullOrEmpty(this.name));
            
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