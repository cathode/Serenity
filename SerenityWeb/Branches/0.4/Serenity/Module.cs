/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
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
        [ThreadStatic]
        private static Module current;
        private static Dictionary<string, Module> modules;
        private readonly string name;
        private Dictionary<string, ContentPage> pages;
        private string title;
        private ContentPage defaultPage;
        #endregion
        #region Methods - Private
        private static Module LoadModule(string name)
        {
            return Module.LoadModuleFile(SPath.Combine("Modules", name + ".dll"), name);
        }
        private static Module LoadModuleFile(string path, string name)
        {
            string title = "Untitled";
            ContentPage defaultPage = null;

            Assembly moduleAsm = Assembly.LoadFile(Path.GetFullPath(path));

            object[] moduleAttributes = moduleAsm.GetCustomAttributes(true);
            foreach (object attrib in moduleAttributes)
            {
                if (attrib is ModuleTitleAttribute)
                {
                    title = ((ModuleTitleAttribute)attrib).Name;
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
                return new Module(name);
            }
            else
            {
                if (defaultPage == null)
                {
                    defaultPage = pages[0];
                }

                Module module = new Module(name);
                module.title = title;
                module.defaultPage = defaultPage;
                module.AddPages(pages);

                return module;
            }
        }
        #endregion
        #region Methods - Public
        public void AddPage(ContentPage page)
        {
            if (!this.pages.ContainsKey(page.Name))
            {
                this.pages.Add(page.Name, page);
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
            if (Module.modules.ContainsKey(name))
            {
                return Module.modules[name];
            }
            else
            {
                return Module.LoadModule(name);
            }
        }
        public static void LoadAllModules()
        {
            string[] paths = Directory.GetFiles(SPath.ModulesFolder);

            foreach (string path in paths)
            {
                if (Path.GetExtension(path).Equals(".dll"))
                {
                    Module m = Module.GetModule(Path.GetFileNameWithoutExtension(path));

                    if ((m != null) && (!Module.modules.ContainsKey(m.name)))
                    {
                        Module.modules.Add(m.name, m);
                    }

                }
            }
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
        public static int ModuleCount
        {
            get
            {
                return Module.modules.Count;
            }
        }
        public static Module Current
        {
            get
            {
                return Module.current;
            }
        }
        #endregion
    }
}
