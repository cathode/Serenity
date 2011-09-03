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

namespace Serenity.WebApps.UserManagement
{
    /// <summary>
    /// Represents built-in user management webapp.
    /// </summary>
    public class UserManagement : WebApplication
    {
        #region Constructors
        internal UserManagement()
        {
            this.UniqueID = new Guid("{3ED56E57-314D-497F-9E37-210174FEF186}");
            this.Version = new Version(1, 0, 0, 0);
            this.Name = "UserManagement";
        }
        #endregion
        public override void InitializeResources()
        {
            this.BindResource(new LoginPage(), "/User/Login", true);
        }

        public override void ProcessRequest(Web.Request request, Web.Response response)
        {
            throw new NotImplementedException();
        }
    }
}
