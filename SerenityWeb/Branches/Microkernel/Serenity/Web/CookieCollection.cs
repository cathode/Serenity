﻿/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
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
        protected override string GetKeyForItem(Cookie item)
        {
            return item.Name;
        }
    }
}