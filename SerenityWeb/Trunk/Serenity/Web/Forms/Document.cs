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
    public class Document : Control
    {
        #region Constructors - Public
        /// <summary>
        /// Intializes a new instance of the <see cref="Document"/> class.
        /// </summary>
        public Document()
        {
            this.body = new Body();
            this.head = new Head();

            this.Controls.AddRange(this.head,
                this.body);
        }
        #endregion
        #region Events - Public
        public event EventHandler ClientLoaded;
        public event EventHandler ClientUnloaded;
        #endregion
        #region Fields - Private
        private Body body;
        private Head head;
        private Doctype doctype = Doctype.XHTML11;
        #endregion
        #region Methods - Protected
        protected virtual void OnClientLoaded(EventArgs e)
        {
            if (this.ClientLoaded != null)
            {
                this.ClientLoaded(this, e);
            }
        }
        protected virtual void OnClientUnloaded(EventArgs e)
        {
            if (this.ClientUnloaded != null)
            {
                this.ClientUnloaded(this, e);
            }
        }
        #endregion
        #region Methods - Public
        protected override void RenderBegin(RenderingContext context)
        {
            base.RenderBegin(context);
        }
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
        /// <summary>
        /// Gets the element that represents the body section of the current <see cref="Document"/>.
        /// </summary>
        public Body Body
        {
            get
            {
                return this.body;
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="Doctype"/> of the current <see cref="Document"/>.
        /// </summary>
        /// <remarks>
        /// This property defaults to <see cref="p:Doctype.XHTML11"/> which is the XHTML 1.1 Document Type.
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
        /// <summary>
        /// Gets the element that represents the head section of the current <see cref="Document"/>.
        /// </summary>
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
