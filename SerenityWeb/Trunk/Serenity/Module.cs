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
using System.IO;
using System.Reflection;
using System.Text;

using Serenity.Attributes;

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
        private Dictionary<string, Page> pages = new Dictionary<string, Page>();
        private string title;
        private string resourceNamespace;
        #endregion
		#region Methods - Public
		public static Module LoadModule(string name)
        {
            return Module.LoadModuleFile(name, SPath.Combine("Modules", name + ".dll"));
        }
        public static Module LoadModuleFile(string name, string assemblyPath)
        {
			string title = name;
            ContentPage defaultPage = null;

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
                    defaultPage = (ContentPage)moduleAsm.CreateInstance(defaultPageAttribute.Name);
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
            List<ContentPage> pages = new List<ContentPage>();
            foreach (Type type in moduleAsm.GetTypes())
            {
                if (type.IsSubclassOf(typeof(ContentPage)) == true)
                {
                    ContentPage page = (ContentPage)moduleAsm.CreateInstance(type.FullName);

                    pages.Add(page);
                }
                else if (type.IsSubclassOf(typeof(ResourceClass)) == true)
                {
                    ResourceClass.RegisterResourceClass(moduleAsm.CreateInstance(type.FullName) as ResourceClass);
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
        public void AddPage(ContentPage page)
        {
            if (!this.pages.ContainsKey(page.SystemName))
            {
                this.pages.Add(page.SystemName, page);
            }
        }
        public void AddPages(IEnumerable<ContentPage> pages)
        {
            foreach (ContentPage page in pages)
            {
                this.AddPage(page);
            }
        }
		public Page GetPage(string name)
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
		public Page DefaultPage
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
        public IEnumerable<Page> Pages
        {
            get
            {
                foreach (Page page in this.pages.Values)
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
