/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Web.Forms
{
    public sealed class ControlAttributeCollection : Collection<ControlAttribute>
    {
        public void EnableAttribute(string name)
        {
            if (!this.Contains(name))
            {
                return;
            }

            var attribute = (from a in this
                             where a.Name == name
                             select a).First();
            attribute.Include = true;
                           

        }
        public void DisableAttribute(string name)
        {
        }
        public bool Contains(string attributeName)
        {
            return (from a in this
                    where a.Name == attributeName
                    select a).Count() > 0;
        }
    }
}
