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
using Serenity.Xml;

namespace Serenity
{
    /// <summary>
    /// Represents a Serenity module.
    /// </summary>
    public sealed class SerenityModule : Multiton<string, SerenityModule>
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the SerenityModule class.
        /// </summary>
        /// <param name="name">The name of the new SerenityModule.</param>
        public SerenityModule(string name)
            : base(name.ToLower())
        {
            this.pages = new List<ContentPage>();
        }
        #endregion
        #region Fields - Private
        private string title;
        private ContentPage defaultPage;
        private List<ContentPage> pages;
        #endregion
        #region Methods - Public
        public void AddPage(ContentPage page)
        {
            if (this.GetPage(page.Name) == null)
            {
                this.pages.Add(page);
            }
        }
        /// <summary>
        /// Adds a collection of pages to the current Module.
        /// </summary>
        /// <param name="pages"></param>
        public void AddPages(IEnumerable<ContentPage> pages)
        {
            foreach (ContentPage page in pages)
            {
                this.AddPage(page);
            }
        }

        public ContentPage GetPage(string name)
        {
            if (string.IsNullOrEmpty(name) == false)
            {
                foreach (ContentPage P in this.pages)
                {
                    if (P.Name == name)
                    {
                        return P;
                    }
                }
                return null;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Gets an array of module names that exist for the current configuration.
        /// </summary>
        /// <returns>A string array containing all module names.</returns>
        public static string[] GetModuleList()
        {
            string[] paths = Directory.GetDirectories(SPath.ModulesFolder);

            for (int i = 0; i < paths.Length; i++)
            {
                paths[i] = Path.GetFileName(paths[i]);
            }

            return paths;
        }
        public static void LoadAllModules()
        {
            string[] names = SerenityModule.GetModuleList();
            foreach (string name in names)
            {
                SerenityModule.LoadModule(name);
            }
        }
        public static SerenityModule LoadModule(string name)
        {
            SerenityModule M = SerenityModule.GetInstance(name.ToLower());
            if (M != null)
            {
                return M;
            }
            else
            {
                string modulePath = SPath.GetModuleFile(name);
                if (File.Exists(modulePath) == true)
                {
                    return SerenityModule.LoadModuleFile(modulePath, name);
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// Loads a module.
        /// </summary>
        /// <param name="path">The file path of the module's assembly.</param>
        /// <param name="name">The name to load the module under.</param>
        /// <returns></returns>
        public static SerenityModule LoadModuleFile(string path, string name)
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
                    defaultPage = (ContentPage)defaultPageAttribute.PageType.Assembly.CreateInstance(defaultPageAttribute.PageType.FullName);
                    break;
                }
            }
            LinkedList<ContentPage> pages = new LinkedList<ContentPage>();
            foreach (Type type in moduleAsm.GetTypes())
            {
                if (type.IsSubclassOf(typeof(ContentPage)) == true)
                {
                    ContentPage page = (ContentPage)moduleAsm.CreateInstance(type.FullName);

                    pages.AddLast(page);
                }
                else if (type.IsSubclassOf(typeof(ResourceClass)) == true)
                {
                    moduleAsm.CreateInstance(type.FullName);
                }
            }
            if (pages.Count == 0)
            {
                return new SerenityModule(name);
            }
            else
            {
                if (defaultPage == null)
                {
                    defaultPage = pages.First.Value;
                }

                SerenityModule module = new SerenityModule(name);
                module.title = title;
                module.defaultPage = defaultPage;
                module.pages.AddRange(pages);

                return module;
            }
        }
        public static TryResult<SerenityModule> TryLoadModule(string name)
        {
            try
            {
                SerenityModule Module = SerenityModule.LoadModule(name);
                if (Module == null)
                {
                    return TryResult<SerenityModule>.FailResult(new Exception("Unspecified Error"));
                }
                else
                {
                    return TryResult<SerenityModule>.SuccessResult(Module);
                }
            }
            catch (Exception E)
            {
                return TryResult<SerenityModule>.FailResult(E);
            }
        }
        public static bool TryLoadAllModules()
        {
            try
            {
                SerenityModule.LoadAllModules();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static TryResult<SerenityModule> TryLoadModuleFile(string path, string name)
        {
            try
            {
                SerenityModule module = SerenityModule.LoadModuleFile(path, name);
                if (module == null)
                {
                    return TryResult<SerenityModule>.FailResult(new Exception("Unspecified Error"));
                }
                else
                {
                    return TryResult<SerenityModule>.SuccessResult(module);
                }
            }
            catch (Exception ex)
            {
                return TryResult<SerenityModule>.FailResult(ex);
            }
        }
        #endregion
        #region Properties - Public
        public ContentPage DefaultPage
        {
            get
            {
                return this.defaultPage;
            }
            set
            {
                this.defaultPage = value;
            }
        }
        public ContentPage[] Pages
        {
            get
            {
                return this.pages.ToArray();
            }
        }
        public string Title
        {
            get
            {
                return this.title;
            }
        }
        #endregion
    }
}