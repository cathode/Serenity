using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Serenity.Web.Resources;
using System.IO;

namespace Serenity
{
    public abstract class ModuleFactory
    {
        #region Methods
        protected virtual IEnumerable<Resource> BuildResourceTree()
        {
            return this.BuildResourceTree(this.ModuleAssembly.GetName().Name);
        }
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
                    res.Add(new ResourceResource(name, asm.GetManifestResourceStream(fullPath)));
                }

            }
            return resources;
        }
        public abstract Module CreateModule();
        #endregion
        #region Properties
        protected abstract Assembly ModuleAssembly
        {
            get;
        }
        #endregion
    }
}
