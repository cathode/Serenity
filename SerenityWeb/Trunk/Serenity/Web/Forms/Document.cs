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

using Serenity.Web.Forms;

namespace Serenity.Web.Forms
{
    public abstract class Document : Control
    {
        #region Constructors - Protected
        protected Document()
        {
            this.body = new Body();
            this.head = new Head();

            this.Controls.AddRange(this.head,
                this.body);
        }
        #endregion
        #region Events - Public
        public event EventHandler Loaded;
        public event EventHandler Unloaded;
        #endregion
        #region Fields - Private
        private Body body;
        private Head head;
        private Doctype doctype = Doctype.XHTML11;
        #endregion
        #region Properties - Protected
        protected override bool CanContainAttributes
        {
            get
            {
                return false;
            }
        }
        protected override string DefaultName
        {
            get
            {
                return "html";
            }
        }
        #endregion
        #region Properties - Public
        public Body Body
        {
            get
            {
                return this.body;
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="Doctype"/> of the current <see cref="WebForm"/>.
        /// </summary>
        /// <remarks>
        /// This property default to <see cref="Doctype.XHTML11"/> which is the XHTML 1.1 Document Type.
        /// </remarks>
        public Doctype Doctype
        {
            get
            {
                return this.doctype;
            }
            set
            {
                this.doctype = value;
            }
        }
        public Head Head
        {
            get
            {
                return this.head;
            }
        }
        #endregion
    }
}
