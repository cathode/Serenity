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
using Serenity.Web;

namespace Serenity.WebApps.ServerInfo
{
    public class ListApps : Resource
    {
        public ListApps()
        {
            this.Name = "ListApps";
        }
        public override void OnRequest(Request request, Response response)
        {
            base.OnRequest(request, response);


        }
    }
}
