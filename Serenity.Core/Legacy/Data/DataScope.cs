using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Data
{
    /// <summary>
    /// Used to determine which database is used for any data operations.
    /// </summary>
    public enum DataScope
    {
        /// <summary>
        /// Specifies global scope. Data in the global scope is accessible from all modules and across all domains.
        /// </summary>
        Global = 0,
        /// <summary>
        /// Specifies module-level scope. Data in the module scope is accessible from a single module,
        /// but is available to that module on any domain. This scope uses the module's schemas.
        /// </summary>
        Module = 1,
        /// <summary>
        /// Specifies domain-level scope. Data in the domain scope is accessible from a single domain,
        /// but is available to any module on that domain. This scope uses the domain schemas.
        /// </summary>
        Domain = 2,
        /// <summary>
        /// Specifies module-and-domain-level scope. Data in this scope is accessible from a single module,
        /// and only on a single domain. This scope uses the module's schemas.
        /// </summary>
        /// <remarks>
        /// This scope should be used when you wish to store data for a module on a per-domain basis.
        /// </remarks>
        ModuleAndDomain = 3,
    }
}
