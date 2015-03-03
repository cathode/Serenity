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
using Serenity.Web;

namespace Serenity.Net
{
    public class DefaultRequestValidator : RequestValidator
    {
        public override bool ValidateRequest(Request request, Response response)
        {
            // TODO: Implement basic request validation.
            return true;
        }
    }
}
