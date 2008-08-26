using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Serenity.Web
{
    /// <summary>
    /// Represents a collection of <see cref="Cookie"/>s.
    /// </summary>
    public sealed class CookieCollection : KeyedCollection<string, Cookie>
    {
        protected override string GetKeyForItem(Cookie item)
        {
            return item.Name;
        }
    }
}
