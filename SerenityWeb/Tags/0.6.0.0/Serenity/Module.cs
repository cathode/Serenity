/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright � 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

using Serenity.Attributes;
using Serenity.Resources;

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
		public static Module LoadModule(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else if (name == string.Empty)
            {
                throw new ArgumentException(__Strings.ArgumentCannotBeEmpty);
            }
            return Module.LoadModuleFile(name, SPath.Combine("Modules", name + ".dll"));
        }
        public static Module LoadModuleFile(string name, string assemblyPath)
        {
			string title = name;
            DynamicResource defaultPage = null;

            Assembly moduleAsm = Assembly.LoadFile(Path.GetFullPath(assemblyPath));

            string resourceNamespace = moduleAsm.GetName().Name + ".Resources.";

            object[] moduleAttributes = moduleAsm.GetCustomAttributes(true);
            foreach (object attrib in moduleAttributes)
            {
                if (attrib is ModuleTitleAttribute)
                {
                    title = ((ModuleTitleAttribute)attrib).Title;
                    break;
                }
            }
            foreach (object attrib in moduleAttributes)
            {
                if (attrib is ModuleDefaultPageAttribute)
                {
                    ModuleDefaultPageAttribute defaultPageAttribute = (ModuleDefaultPageAttribute)attrib;
                    defaultPage = (DynamicResource)moduleAsm.CreateInstance(defaultPageAttribute.Name);
                    break;
                }
            }
            foreach (object attrib in moduleAttributes)
            {
                if (attrib is ModuleResourceNamespaceAttribute)
                {
                    resourceNamespace = ((ModuleResourceNamespaceAttribute)attrib).ResourceNamespace;
                    break;
                }
            }
            List<DynamicResource> pages = new List<DynamicResource>();
            foreach (Type type in moduleAsm.GetTypes())
            {
                if (type.IsSubclassOf(typeof(DynamicResource)) == true)
                {
                    DynamicResource page = (DynamicResource)moduleAsm.CreateInstance(type.FullName);

                    pages.Add(page);
                }
            }
            if (pages.Count == 0)
            {
				Module module = new Module(name);
				module.assembly = moduleAsm;
				return module;
            }
            else
            {
                if (defaultPage == null)
                {
                    defaultPage = pages[0];
                }

                Module module = new Module(name);
				module.assembly = moduleAsm;
                module.title = title;
                module.resourceNamespace = resourceNamespace;
                module.AddPages(pages);

                return module;
            }
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
        #endregion
    }
}