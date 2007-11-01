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
    /// Manages multiple drivers and provides unified access to incoming
    /// CommonContexts.
    /// </summary>
    public sealed class DriverPool
    {
        #region Constructors - Internal
        internal DriverPool(SerenityServer server)
        {
            this.server = server;
        }
        #endregion
        #region Fields - Private
        private List<WebDriver> drivers = new List<WebDriver>();
        private SerenityServer server;
        #endregion
        #region Methods - Public
        public IEnumerable<WebDriver> GetDriversByProvider(string provider)
        {
            foreach (WebDriver driver in this.drivers)
            {
                if (driver.Info.Provider.Equals(provider,this.Server.StringComparison))
                {
                    yield return driver;
                }
            }
        }
        public IEnumerable<WebDriver> GetDriversBySchema(string schema)
        {
            foreach (WebDriver driver in this.drivers)
            {
                if (driver.Info.UriSchema.Equals(schema, this.Server.StringComparison))
                {
                    yield return driver;
                }
            }
        }
        #endregion
        #region Properties - Public
        public SerenityServer Server
        {
            get
            {
                return this.server;
            }
        }
        #endregion
    }
}