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
    public sealed class __SerenityModuleFactory : IModuleFactory
    {
        private const string ResourceNamespace = "Serenity.Resources.";
        #region Methods
        /// <summary>
        /// Builds the module for Serenity.
        /// </summary>
        /// <returns></returns>
        public Module CreateModule()
        {
            Module m = new Module("serenity");

            Assembly asm = Assembly.GetExecutingAssembly();

            m.Resources.Add(new DirectoryResource("resource",
                (from p in asm.GetManifestResourceNames()
                where p.StartsWith(ResourceNamespace)
                let s = asm.GetManifestResourceStream(p)
                let a = new byte[s.Length]
                let i = s.Read(a, 0, a.Length)
                select new ResourceResource(p.Remove(0, ResourceNamespace.Length), a)).ToArray()));
            return m;
        }
        #endregion
    }
}
