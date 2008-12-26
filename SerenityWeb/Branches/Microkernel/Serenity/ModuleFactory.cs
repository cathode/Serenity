using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Serenity.Web.Resources;

namespace Serenity
{
    public abstract class ModuleFactory
    {
        protected IEnumerable<Resource> BuildResourceTree(string resourceNamespace)
        {
            if (!resourceNamespace.EndsWith("."))
            {
                resourceNamespace += ".";
            }
            Assembly asm = this.ModuleAssembly;

            foreach (string fullPath in asm.GetManifestResourceNames())
            {
                if (fullPath.StartsWith(resourceNamespace))
                {
                    string path = fullPath.Substring(resourceNamespace.Length);



                }
            }
            return null;
        }
        public abstract Module CreateModule();
        protected abstract Assembly ModuleAssembly
        {
            get;
        }
    }
}
