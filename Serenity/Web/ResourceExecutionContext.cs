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
using Serenity.Net;

namespace Serenity.Web
{
    public class ResourceExecutionContext
    {
        #region Properties
        public WebServer Server
        {
            get;
            set;
        }
        public Connection Connection
        {
            get;
            set;
        }
        public Request Request
        {
            get;
            set;
        }
        public Response Response
        {
            get;
            set;
        }
        #endregion
    }
}
