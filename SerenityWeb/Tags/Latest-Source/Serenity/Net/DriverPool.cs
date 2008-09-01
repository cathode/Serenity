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
using System.Text;
using System.Linq;

namespace Serenity.Net
{
    /// <summary>
    /// Manages multiple drivers and provides unified access to incoming
    /// CommonContexts.
    /// </summary>
    public sealed class DriverPool
    {
        #region Constructors - Internal
        internal DriverPool()
        {
        }
        #endregion
        #region Fields - Private
        private List<ProtocolDriver> drivers = new List<ProtocolDriver>();
        #endregion
        #region Methods - Public
        public void Add(ProtocolDriver driver)
        {
            this.drivers.Add(driver);
        }
        public IEnumerable<ProtocolDriver> GetDriversByProvider(string provider)
        {
            return from d in this.drivers
                   where d.ProviderName == provider
                   select d;
        }
        public IEnumerable<ProtocolDriver> GetDriversBySchema(string schema)
        {
            return from d in this.drivers
                   where d.SchemaName == schema
                   select d;
        }
        #endregion
    }
}