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

using Serenity.Web.Resources;
using Serenity.Logging;

namespace Serenity
{
    public sealed class Module
    {
        #region Constructors - Private
        private Module(string name)
        {
            this.name = name;
        }
        #endregion
        #region Fields - Private
        private Assembly assembly;
        private readonly string name;
        private Dictionary<string, DynamicResource> pages = new Dictionary<string, DynamicResource>();
        private string title;
        private string resourceNamespace;
        #endregion
        #region Methods - Public
        public static Module LoadModule(string assemblyPath)
        {
            if (assemblyPath == null)
            {
                throw new ArgumentNullException("path");
            }
            else if (assemblyPath.Length == 0)
            {
                throw new ArgumentException(__Strings.ArgumentCannotBeEmpty);
            }
            if (!File.Exists(assemblyPath))
            {
                Log.RecordEvent("Failed to find module assembly file at " + assemblyPath, Serenity.Logging.Severity.Error);
                throw new FileNotFoundException("The module was not found at the supplied assemblyPath", assemblyPath);
            }

            Assembly moduleAsm = Assembly.LoadFile(Path.GetFullPath(assemblyPath));

            string resourceNamespace = moduleAsm.GetName().Name + ".Resources.";
            string title = null;
            object[] moduleAttributes = moduleAsm.GetCustomAttributes(true);
            foreach (object attrib in moduleAttributes)
            {
                var a = attrib as ModuleTitleAttribute;
                if (a != null)
                {
                    title = a.Title;
                    break;
                }
            }
            foreach (object attrib in moduleAttributes)
            {
                var a = attrib as ModuleResourceNamespaceAttribute;

                if (a != null)
                {
                    resourceNamespace = a.ResourceNamespace;
                    break;
                }
            }
            List<DynamicResource> pages = new List<DynamicResource>();
            foreach (Type type in moduleAsm.GetTypes())
            {
                if (type.IsSubclassOf(typeof(DynamicResource)) && !type.IsAbstract && type.GetCustomAttributes(false).OfType<SuppressLoadCreationAttribute>().Count() == 0)
                {
                    DynamicResource page = (DynamicResource)moduleAsm.CreateInstance(type.FullName);

                    pages.Add(page);
                }
            }
            Module module = new Module(assemblyPath);
            module.assembly = moduleAsm;
            module.title = title;
            module.resourceNamespace = resourceNamespace;
            module.AddPages(pages);

            return module;
        }
        public void AddPage(DynamicResource page)
        {
            if (!this.pages.ContainsKey(page.Name))
            {
                this.pages.Add(page.Name, page);
            }
        }
        public void AddPages(IEnumerable<DynamicResource> pages)
        {
            foreach (DynamicResource page in pages)
            {
                this.AddPage(page);
            }
        }
        public DynamicResource GetPage(string name)
        {
            if (this.pages.ContainsKey(name.ToLower()))
            {
                return this.pages[name.ToLower()];
            }
            else
            {
                return null;
            }
        }
        #endregion
        #region Properties - Public
        public Assembly Assembly
        {
            get
            {
                return this.assembly;
            }
        }
        public DynamicResource DefaultPage
        {
            get
            {
                return this.pages["default"];
            }
        }
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        public IEnumerable<DynamicResource> Pages
        {
            get
            {
                foreach (DynamicResource page in this.pages.Values)
                {
                    yield return page;
                }
            }
        }
        public string ResourceNamespace
        {
            get
            {
                return this.resourceNamespace;
            }
        }
        public string SystemName
        {
            get
            {
                return this.Name.ToLower();
            }
        }
        public static Module Current
        {
            get
            {
                return null;
            }
            internal set
            {

            }
        }
        #endregion
    }
}
