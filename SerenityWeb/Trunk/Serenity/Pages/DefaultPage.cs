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
using Serenity.Net;
using Serenity.Web.Forms;

namespace Serenity.Pages
{
    public sealed class DefaultPage : DocumentResource
    {
        #region Constructors - Public
        public DefaultPage()
        {
            this.Name = "Default";
            this.ContentType = MimeType.TextHtml;
        }
        #endregion
        #region Methods - Protected
        protected override Document CreateForm()
        {
            return new DefaultPage.DefaultPageForm();
        }
        #endregion
        #region Types - Private
        private sealed class DefaultPageForm : Document
        {
            internal DefaultPageForm()
            {
                Division d1 = new Division();
                d1.Controls.AddRange(new TextControl("Welcome to the Serenity default page."),
                        new LineBreak(),
                        new Anchor(new Uri("http://www.codeplex.com/serenity"), "Serenity Home Page"),
                        new LineBreak(),
                        new PathLink()
                        {
                            Target = new Uri("http://serenityproject.net/img/misc/"),
                            ShowTargetHostname = true,
                        });
                this.Body.Controls.Add(d1);
            }
        }
        #endregion
    }
}
