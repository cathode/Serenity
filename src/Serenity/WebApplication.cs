/* Serenity - The next evolution of web server technology.
 * Copyright © 2006-2010 Will Shelley. All Rights Reserved. */
using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity
{
    /// <summary>
    /// Represents a web application.
    /// </summary>
    public abstract class WebApplication
    {
        #region Properties
        /// <summary>
        /// Gets the name of the current <see cref="WebApplication"/>.
        /// </summary>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// Gets the unique identifier of the current <see cref="WebApplication"/>.
        /// </summary>
        public abstract Guid UniqueID
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Version"/> of the current <see cref="WebApplication"/>.
        /// </summary>
        public abstract Version Version
        {
            get;
        }
        #endregion
    }
}
