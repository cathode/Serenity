/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using Serenity.Web;

namespace Serenity
{
    public class DirectoryResource : Resource
    {
        #region Constructors - Public
        public DirectoryResource(string location)
        {
            if (location.EndsWith("/"))
            {
                this.location = location;
            }
            else
            {
                this.location = location + "/";
            }
        }
        #endregion
        #region Fields - Private
        private string location = "/";
        #endregion
        #region Methods - Public
        public override void OnRequest(CommonContext context)
        {

        }
        #endregion
    }
}
