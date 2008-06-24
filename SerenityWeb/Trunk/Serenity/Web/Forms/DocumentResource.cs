﻿/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serenity.Web.Resources;
using System.IO;

namespace Serenity.Web.Forms
{
    public abstract class DocumentResource : DynamicResource
    {
        #region Constructors - Public
        public DocumentResource()
        {
        }
        #endregion
        #region Methods - Public
        public sealed override void OnRequest()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document form = this.CreateForm();

                RenderingContext rc = new RenderingContext(ms);
                form.Render(rc);

                Response.Write(ms.ToArray());
            }
        }
        #endregion
        #region Properties - Protected
        protected abstract Document CreateForm();
        #endregion
    }
}