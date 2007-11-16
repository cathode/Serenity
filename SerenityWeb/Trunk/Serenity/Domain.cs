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

using Serenity.Collections;
using Serenity.Resources;

namespace Serenity
{
    /// <summary>
    /// Represents a domain.
    /// </summary>
    public sealed class Domain
    {
        #region Constructors - Public
        public Domain(string hostName)
        {
            this.hostName = hostName;
        }
        #endregion
        #region Fields - Private
        private readonly string hostName;
        private ResourceTree resources = new ResourceTree();
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
        public ResourceTree Resources
        {
            get
            {
                return this.resources;
            }
        }
        #endregion
    }
}
