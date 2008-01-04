/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using Serenity;
using Serenity.Resources;
using Serenity.Web;
using Serenity.Web.Drivers;

namespace Serenity.Pages
{
    public sealed class DefaultPage : DynamicResource
    {
        #region Constructors - Public
        public DefaultPage()
        {
            this.Name = "Default";
            this.ContentType = MimeType.TextHtml;
        }
        #endregion
        #region Methods - Public
        public override void OnRequest(CommonContext context)
        {
            context.Response.WriteLine("This is the default page of the Serenity module.\r\nPlease visit <a href='http://serenityproject.net/'>http://serenityproject.net/</a> for more information.");
        }
        #endregion
    }
}
