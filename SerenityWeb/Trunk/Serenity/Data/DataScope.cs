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
        /// but is available to that module on any domain.
        /// </summary>
        Module = 1,
        /// <summary>
        /// Specifies domain-level scope. Data in the domain scope is like data in the module scope but is
        /// also separated on a per-domain basis.
        /// </summary>
        Domain = 2,
    }
}
