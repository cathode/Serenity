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

namespace Serenity.Internal
{
    internal sealed class BuiltinWebApp : WebApplication
    {

        public override void InitializeResources()
        {
            throw new NotImplementedException();
        }

        public override void ProcessRequest(Web.Request request, Web.Response response)
        {
            throw new NotImplementedException();
        }
    }
}
