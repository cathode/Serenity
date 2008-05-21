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
using Serenity.Web.Resources;
using Serenity.Web;
using Serenity.Web.Drivers;
using Serenity.Web.Forms;
using Serenity.Web.Forms.Controls;

namespace Serenity.Pages
{
    public sealed class DefaultPage : WebFormResource
    {
        #region Constructors - Public
        public DefaultPage()
        {
            this.Name = "Default";
            this.ContentType = MimeType.TextHtml;
        }
        #endregion
        #region Methods - Protected
        protected override WebForm CreateForm()
        {
            return new DefaultPage.DefaultPageForm();
        }
        #endregion
        #region Types - Private
        private sealed class DefaultPageForm : WebForm
        {
            internal DefaultPageForm()
            {
                this.Body.Controls.AddRange(
                    new Division(
                        new TextControl("Hello World!")));
            }
        }
        #endregion
    }
}
