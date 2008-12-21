using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Web.Resources
{
    /// <summary>
    /// Extends the <see cref="Resource"/> class with additional functionality
    /// to support the containment of child resources.
    /// </summary>
    public abstract class TreeResource : Resource
    {
        private readonly List<Resource> children;

        /// <summary>
        /// Gets the child resources of the current <see cref="TreeResource"/>.
        /// </summary>
        public List<Resource> Children
        {
            get
            {
                return this.children;
            }
        }
    }
}
