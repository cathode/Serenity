/* Serenity - The next evolution of web server technology.
 * Copyright © 2006-2010 Will Shelley. All Rights Reserved. */
using System.Collections.ObjectModel;

namespace Serenity.Core
{
    /// <summary>
    /// Represents a collection of <see cref="Cookie"/> objects.
    /// </summary>
    public sealed class CookieCollection : KeyedCollection<string, Cookie>
    {
        /// <summary>
        /// Overridden. Gets the key for the specified element.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override string GetKeyForItem(Cookie item)
        {
            return item.Name;
        }
    }
}
