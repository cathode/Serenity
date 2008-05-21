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
using System.Linq;
using System.Text;
using Serenity.Web.Resources;
using System.IO;

namespace Serenity.Web.Forms
{
    public abstract class WebFormResource : DynamicResource
    {
        #region Methods - Public
        public sealed override void OnRequest(CommonContext context)
        {
            base.OnRequest(context);

            using (MemoryStream ms = new MemoryStream())
            {
                WebForm form = this.Form;

                
            }
        }
        #endregion
        #region Properties - Protected
        protected abstract WebForm Form
        {
            get;
        }
        #endregion
    }
}
