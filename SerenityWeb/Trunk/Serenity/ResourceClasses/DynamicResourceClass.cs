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

namespace Serenity.ResourceClasses
{
    internal class DynamicResourceClass : ResourceClass
    {
        public DynamicResourceClass()
            : base("dynamic")
        {
        }
        public override void HandleContext(CommonContext context)
        {
            ErrorHandler.Handle(context, StatusCode.Http501NotImplemented);
            return;
        }
    }
}
