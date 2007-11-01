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
using System.Text;

namespace Serenity.Web.Drivers
{
    /// <summary>
    /// Provides a way for WebDrivers to expose information about themselves.
    /// </summary>
    public sealed class DriverInfo
    {
        #region Constructors - Internal
        internal DriverInfo()
        {
            this.provider = "unknown";
            this.version = new Version();
            this.protocol = "unknown";
            this.uriSchema = "unknown";
        }
        #endregion
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the DriverInfo class.
        /// </summary>
        /// <param name="className"></param>
        /// <param name="typeName"></param>
        /// <param name="uriSchema"></param>
        /// <param name="version"></param>
        public DriverInfo(string provider, string protocol, string uriSchema, Version version)
        {
            this.provider = provider;
            this.protocol = protocol;
            this.uriSchema = uriSchema;
            this.version = version;
        }
        #endregion
        #region Fields - Private
        private string provider;
        private string protocol;
        private string uriSchema;
        private Version version;
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets the name of the provider of the driver.
        /// </summary>
        public string Provider
        {
            get
            {
                return this.provider;
            }
            internal set
            {
                this.provider = value;
            }
        }
        /// <summary>
        /// Gets a string describing the type of the current DriverInfo.
        /// </summary>
        public string Protocol
        {
            get
            {
                return this.protocol;
            }
            internal set
            {
                this.protocol = value;
            }
        }
        /// <summary>
        /// Gets the URI schema used by the current DriverInfo.
        /// </summary>
        public string UriSchema
        {
            get
            {
                return this.uriSchema;
            }
            internal set
            {
                this.uriSchema = value;
            }
        }
        /// <summary>
        /// Gets the version of the current DriverInfo.
        /// </summary>
        public Version Version
        {
            get
            {
                return this.version;
            }
            internal set
            {
                this.version = value;
            }
        }
        #endregion
    }
}
