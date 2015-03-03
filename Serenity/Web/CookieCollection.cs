/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2015 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Serenity.Web
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
