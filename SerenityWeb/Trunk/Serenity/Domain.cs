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

using Serenity.Resources;

namespace Serenity
{
    /// <summary>
    /// Represents a domain.
    /// </summary>
    public sealed class Domain
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the Domain class.
        /// </summary>
        /// <param name="hostName">The host name of the new Domain instance.</param>
        public Domain(string hostName)
        {
            if (hostName == null)
            {
                throw new ArgumentNullException("hostName");
            }
            
            this.hostName = hostName;
        }
        #endregion
        #region Fields - Private
        private readonly string hostName;
        #endregion
        #region Methods - Public
        public static string GetParentHost(string hostName)
        {
            string[] oldNames = hostName.Split('.');
            string[] newNames = new string[oldNames.Length - 1];
            Array.Copy(oldNames, newNames, newNames.Length);
            return string.Join(".", newNames);
        }
        #endregion
        #region Properties - Public
        public string HostName
        {
            get
            {
                return this.hostName;
            }
        }
        #endregion
    }
}
