using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Serenity.Properties;
using Serenity.Web;

namespace Serenity
{
    /// <summary>
    /// Represents a web application loaded into the server.
    /// </summary>
    public sealed class Module
    {
        #region Fields
        private readonly string name;
        private readonly List<Resource> resources = new List<Resource>();
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class.
        /// </summary>
        /// <param name="name">The name of the new <see cref="Module"/>.</param>
        public Module(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            else if (name.Length == 0)
                throw new ArgumentException(string.Format(AppResources.ParamEmptyException, "name"), "name");
            
            this.name = name;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the name of the current <see cref="Module"/>.
        /// </summary>
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
        #region Methods
        public static IEnumerable<Module> LoadModules(string assemblyPath)
        {
            Assembly moduleAsm = Assembly.LoadFrom(assemblyPath);

            return from t in moduleAsm.GetTypes()
                   where t.IsSubclassOf(typeof(ModuleFactory)) && !t.IsAbstract
                   select ((ModuleFactory)Activator.CreateInstance(t)).CreateModule();
        }
        #endregion
    }
}
