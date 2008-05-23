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

namespace Serenity.Web.Forms
{
    /// <summary>
    /// Represents a web control that only contains text.
    /// </summary>
    public sealed class TextControl : Control
    {
        #region Constructors - Public
        public TextControl()
        {
        }
        public TextControl(string content)
        {
            this.content = content;
        }
        #endregion
        #region Fields - Private
        private string content;
        #endregion
        #region Methods - Protected
        protected override void RenderBegin(RenderingContext context)
        {
            if (string.IsNullOrEmpty(this.content))
            {
                return;
            }

            byte[] buf = context.OutputEncoding.GetBytes(this.content);
            context.OutputStream.Write(buf, 0, buf.Length);
        }
        protected override void RenderEnd(RenderingContext context)
        {
        }
        #endregion
        #region Properties - Protected
        protected override bool CanContainControls
        {
            get
            {
                return false;
            }
        }
        #endregion
        #region Properties - Public
        public string Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
            }
        }
        #endregion
    }
}
