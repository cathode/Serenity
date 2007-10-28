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

using Serenity;
using Serenity.Web;
using Serenity.Web.Drivers;

namespace Serenity.Pages
{
    public sealed class DefaultPage : ContentPage
    {
        #region Methods - Public
        public override void OnRequest(CommonContext context)
        {
            context.Response.WriteLine("This Page has not yet been implemented!");
        }
        #endregion
        #region Properties - Public
        public override string Name
        {
            get
            {
                return "Default";
            }
        }
        #endregion
    }
}
