/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *----------------------------------------------------------------------------*
 * Authors:                                                                   *
 * - Will 'AnarkiNet' Shelley (AnarkiNet@gmail.com): Original Author          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serenity.Net;

namespace Serenity.Web
{
    public sealed class RootResource : DirectoryResource
    {
        #region Constructors
        internal RootResource(Server owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }
            this.Owner = owner;
        }
        #endregion
        #region Methods
        public override Resource GetChild(Uri uri)
        {
            return this.GetChild(uri.Host) ?? base.GetChild(uri);
        }
        #endregion
    }
}
