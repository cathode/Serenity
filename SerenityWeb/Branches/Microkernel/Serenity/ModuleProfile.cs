using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity
{
    public sealed class ModuleProfile
    {
        private string assemblyFile;
        private Module module;
        public string AssemblyFile
        {
            get
            {
                return this.assemblyFile;
            }
            set
            {
                this.assemblyFile = value;
            }
        }
        public Module Module
        {
            get
            {
                return this.module;
            }
            set
            {
                this.module = value;
            }
        }
    }
}
