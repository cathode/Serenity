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
using Serenity.Web;
using System.IO;

namespace Serenity.Web.Forms
{
    public abstract class DocumentResource : DynamicResource
    {
        #region Constructors
        protected DocumentResource()
        {
            this.ContentType = MimeType.TextHtml;
        }
        #endregion
        #region Methods - Protected
        protected abstract Document CreateForm();
        #endregion
        #region Methods - Public
        public sealed override void OnRequest(Request request, Response response)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document form = this.CreateForm();

                RenderingContext rc = new RenderingContext(ms);
                rc.Request = request;
                rc.Response = response;
                form.Render(rc);

                response.Write(ms.ToArray());
            }
        }
        #endregion
       
    }
}
