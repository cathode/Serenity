/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.WebApps.ServerManagement
{
    public sealed class ServerManagementWebApp : WebApplication
    {
        #region Constructors
        internal ServerManagementWebApp()
        {
            this.Name = "ServerManagement";
            this.UniqueID = new Guid("{9D67106C-6081-41EC-A6DA-93B7E3A2AD21}");
            this.Version = new Version(1, 0, 0, 0);
        }
        #endregion
        #region Methods
        public override void InitializeResources()
        {
        }

        public override void ProcessRequest(Web.Request request, Web.Response response)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
