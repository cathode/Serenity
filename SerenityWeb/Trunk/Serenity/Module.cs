/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
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
        static Module()
        {
            Module.modules = new Dictionary<string, Module>();
        }
        private Module(string name)
        {
            this.name = name;
            this.pages = new Dictionary<string, ContentPage>();
        }
        #endregion
        #region Fields - Private
		private Assembly assembly;
        private static Dictionary<string, Module> modules;
        private readonly string name;
        private Dictionary<string, ContentPage> pages;
        private string title;
        private ContentPage defaultPage;
        private string resourceNamespace;
        #endregion
		#region Methods - Public
		public static Module LoadModule(string name)
        {
            return Module.LoadModuleFile(name, SPath.Combine("Modules", name + ".dll"));
        }
        public static Module LoadModuleFile(string name, string assemblyPath)
        {
            string title = "Untitled";
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
                    defaultPage = (ContentPage)moduleAsm.CreateInstance(defaultPageAttribute.TypeName);
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
                module.defaultPage = defaultPage;
                module.resourceNamespace = resourceNamespace;
                module.AddPages(pages);

                return module;
            }
        }
		public static bool AddModule(Module module)
		{
			if (module != null && !Module.modules.ContainsKey(module.Name))
			{
				Module.modules.Add(module.Name, module);
				return true;
			}
			else
			{
				return false;
			}
		}
        public void AddPage(ContentPage page)
        {
            if (!this.pages.ContainsKey(page.Name))
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
        public static Module GetModule(string name)
        {
			if (!string.IsNullOrEmpty(name) && Module.modules.ContainsKey(name))
			{
				return Module.modules[name];
			}
			else
			{
				return null;
			}
        }
		public ContentPage GetPage(string name)
		{
			if (this.pages.ContainsKey(name))
			{
				return this.pages[name];
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
		public ContentPage DefaultPage
		{
			get
			{
				return this.defaultPage;
			}
		}
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        public static int LoadedCount
        {
            get
            {
                return Module.modules.Count;
            }
        }
        public string ResourceNamespace
        {
            get
            {
                return this.resourceNamespace;
            }
        }
        #endregion
    }
}
