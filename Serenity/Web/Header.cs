/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace Serenity.Web
{
    /// <summary>
    /// Represents a name and a set of values associated with a request or response.
    /// </summary>
    public sealed class Header
    {
        #region Fields
        /// <summary>
        /// Backing field for the <see cref="Header.Name"/> property.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Backing field for the <see cref="Header.Validator"/> property.
        /// </summary>
        private Regex validator;

        /// <summary>
        /// Backing field for the <see cref="Header.Value"/> property.
        /// </summary>
        private string value = string.Empty;
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        public Header(string name)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            this.name = name;
            this.value = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <param name="validator">A <see cref="System.Text.Regex"/> instance that is used to validate header values.</param>
        public Header(string name, Regex validator)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            this.name = name;
            this.validator = validator;
            this.value = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <param name="value">An initial value for the new header.</param>
        public Header(string name, string value)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            this.name = name;
            this.value = value;
        }
        #endregion
        #region Properties
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
        /// Gets or sets a <see cref="Regex"/> that is used to determine
        /// if the header value is valid.
        /// </summary>
        public Regex Validator
        {
            get
            {
                return this.validator;
            }
            set
            {
                this.validator = value;
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
        #region Methods
        /// <summary>
        /// Overridden. Converts the current <see cref="Header"/> to it's string representation.
        /// </summary>
        /// <returns>A string representation of the current <see cref="Header"/>.</returns>
        public override string ToString()
        {
            return this.Name + ": " + this.Value;
        }

        /// <summary>
        /// Contains invariant contracts for this type.
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
        }
        #endregion
    }
}