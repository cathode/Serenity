/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using Serenity.Web;

namespace Serenity.Pages
{
    public sealed class SystemMaster : MasterPage
    {
        public override void PostRequest(CommonContext context)
        {
            
        }
        public override void PreRequest(CommonContext context)
        {
            //WS: Insert auth/credential checking code here
            //    (to prevent unauthorized access to administrative stuff)
        }
    }
}
