using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Data
{
    /// <summary>
    /// Used to determine which database is used for any data operations.
    /// </summary>
    [Flags]
    public enum DataScope
    {
        /// <summary>
        /// Specifies global scope. Data in the global scope is accessible from all modules and across all domains.
        /// </summary>
        Global = 0,
        /// <summary>
        /// Specifies domain-level scope. Data in the domain scope is accessible from all modules,
        /// but is kept separate from other domains.
        /// </summary>
        Domain = 1,
        /// <summary>
        /// Specifies module-level scope. Data in the module scope is accessible from a single module,
        /// but is available to that module on any domain.
        /// </summary>
        Module = 2,
        /// <summary>
        /// Specifies module and domain scope. Data in the module and domain scope is only available to
        /// a single module, on a single domain.
        /// </summary>
        DomainAndModule = DataScope.Domain | DataScope.Module,
    }
}
