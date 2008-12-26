/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *----------------------------------------------------------------------------*
 * Authors:                                                                   *
 * - Will 'AnarkiNet' Shelley (AnarkiNet@gmail.com): Original Author          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serenity.Web.Resources;
using System.Reflection;

namespace Serenity
{
    /// <summary>
    /// Provides a factory for building the Serenity module.
    /// </summary>
    internal sealed class __SerenityModuleFactory : ModuleFactory
    {
        private const string ResourceNamespace = "Serenity.Resources.";
        #region Methods
        /// <summary>
        /// Builds the module for Serenity.
        /// </summary>
        /// <returns></returns>
        public override Module CreateModule()
        {
            Module m = new Module("serenity");

            Assembly asm = Assembly.GetExecutingAssembly();

            m.Resources.AddRange(this.BuildResourceTree(ResourceNamespace));
            return m;
        }

        
        #endregion
        protected override Assembly ModuleAssembly
        {
            get
            {
                return Assembly.GetExecutingAssembly();
            }
        }
    }
}
