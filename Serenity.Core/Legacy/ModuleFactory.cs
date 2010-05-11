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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Serenity.Web;

namespace Serenity
{
    /// <summary>
    /// Represents an abstract base class for classes that create 
    /// <see cref="Module"/> instances.
    /// </summary>
    /// <remarks>
    /// A class that derives from <see cref="ModuleFactory"/> is expected to
    /// create a <see cref="Module"/> instance, populated with resources and
    /// other data that the module requires.
    /// </remarks>
    public abstract class ModuleFactory
    {
        #region Methods
        /// <summary>
        /// Overloaded. Builds a resource tree.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<Resource> BuildResourceTree()
        {
            return this.BuildResourceTree(this.ModuleAssembly.GetName().Name);
        }
        /// <summary>
        /// Overloaded. Builds a resource tree.
        /// </summary>
        /// <param name="resourceNamespace"></param>
        /// <returns></returns>
        protected virtual IEnumerable<Resource> BuildResourceTree(string resourceNamespace)
        {
            if (resourceNamespace == null)
            {
                resourceNamespace = this.ModuleAssembly.GetName().Name + ".";
            }
            else if (!resourceNamespace.EndsWith("."))
            {
                resourceNamespace += ".";
            }
            Assembly asm = this.ModuleAssembly;

            List<Resource> resources = new List<Resource>();

            foreach (string fullPath in from p in asm.GetManifestResourceNames()
                                        where p.StartsWith(resourceNamespace)
                                        select p)
            {
                var parts = fullPath.Substring(resourceNamespace.Length).Split('.');
                string name;

                if (parts.Length < 2)
                {
                    name = parts[0];
                }
                else
                {
                    name = parts[parts.Length - 2] + "." + parts[parts.Length - 1];
                }

                Resource res;
                if (parts.Length < 3)
                {
                    resources.Add(new ResourceResource(name, asm.GetManifestResourceStream(fullPath)));
                }
                else
                {
                    res = resources.Find(r => r.Name == parts[0]);

                    if (res == null)
                    {
                        res = new DirectoryResource(parts[0]);
                        resources.Add(res);
                    }
                    for (int i = 1; i < parts.Length - 2; i++)
                    {
                        Resource prev = res;
                        res = res.GetChild(parts[i]);
                        if (res == null)
                        {
                            res = new DirectoryResource(parts[i]);
                        }
                        prev.Add(res);
                    }
                    //TODO: Figure out if theres some way to determine the
                    //      created and modified date of an embedded resource.
                    res.Add(new ResourceResource(name, asm.GetManifestResourceStream(fullPath)));
                }

            }
            return resources;
        }
        /// <summary>
        /// When overridden in a derived class, creates an instance of the
        /// <see cref="Module"/> class.
        /// </summary>
        /// <returns></returns>
        public abstract Module CreateModule();
        #endregion
        #region Properties
        /// <summary>
        /// When overridden in a derived class, gets the <see cref="Assembly"/>
        /// that the built <see cref="Module"/> is created from.
        /// </summary>
        protected abstract Assembly ModuleAssembly
        {
            get;
        }
        #endregion
    }
}
