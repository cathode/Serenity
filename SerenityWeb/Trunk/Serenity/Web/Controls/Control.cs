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
using System.IO;
using System.Text;

namespace Serenity.Web.Controls
{
    public abstract class Control
    {
        #region Methods - Public
        public abstract byte[] Render(CommonContext context);
        public void RenderToStream(CommonContext context, Stream stream)
        {
            byte[] content = this.Render(context);

            stream.Write(content, 0, content.Length);
        }
        #endregion
    }
}
