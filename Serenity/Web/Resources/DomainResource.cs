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

namespace Serenity.Web.Resources
{
    public sealed class DomainResource : DirectoryResource
    {
        private string hostName;
        public override void OnRequest(Request request, Response response)
        {

        }
        public string HostName
        {
            get
            {
                return this.hostName;
            }
            set
            {
                this.hostName = value;
            }
        }
    }
}
