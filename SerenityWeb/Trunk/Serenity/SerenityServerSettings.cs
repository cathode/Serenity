using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity
{
    public sealed class SerenityServerSettings
    {
        public bool LogToConsole = true;
        public Dictionary<string, string> ModuleEntries = new Dictionary<string, string>();
        public List<string> DomainEntries = new List<string>();
    }
}
