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
using System.IO;
using System.Reflection;
using System.Linq;
using Serenity.Properties;
using Serenity.Web.Resources;

namespace Serenity
{
    public sealed class Module
    {
        #region Constructors
        public Module(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else if (name.Length == 0)
            {
                throw new ArgumentException(string.Format(AppResources.ArgumentEmptyException, "name"), "name");
            }
            this.name = name;
        }
        #endregion
        #region Fields - Private
        private readonly string name;
        private readonly List<Resource> resources = new List<Resource>();
        #endregion
        #region Methods - Public
        public static IEnumerable<Module> LoadModules(string assemblyPath)
        {
            Assembly moduleAsm = Assembly.LoadFrom(assemblyPath);

            return from t in moduleAsm.GetTypes()
                   where t.GetInterface(typeof(IModuleFactory).Name) != null
                   select ((IModuleFactory)Activator.CreateInstance(t)).CreateModule();
        }
        #endregion
        #region Properties - Public
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        public List<Resource> Resources
        {
            get
            {
                return this.resources;
            }
        }
        #endregion
    }
}
