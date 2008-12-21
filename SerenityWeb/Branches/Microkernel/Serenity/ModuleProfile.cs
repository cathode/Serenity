using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity
{
    public sealed class ModuleProfile
    {
        private string assemblyFile;

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
    }
}
