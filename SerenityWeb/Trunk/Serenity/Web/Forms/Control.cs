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
using System.IO;
using System.Xml;

namespace Serenity.Web.Forms
{
    /// <summary>
    /// Represents a control in the WebForms API.
    /// </summary>
    public abstract class Control
    {
        #region Constructors - Protected
        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        protected Control()
        {
            this.Name = this.DefaultName;
            if (this.CanContainAttributes)
            {
                this.attributes = new ControlAttributeCollection();
            }
            if (this.CanContainControls)
            {
                this.controls = new ControlCollection();
            }
        }
        #endregion
        #region Fields - Private
        private string name;
        private ControlCollection controls = new ControlCollection();
        private string id;
        private string classification;
        private string style;
        private ControlAttributeCollection attributes = new ControlAttributeCollection();
        #endregion
        #region Fields - Public
        /// <summary>
        /// Holds the default control name.
        /// </summary>
        public const string DefaultControlName = "control";
        #endregion
        #region Methods - Protected
        /// <summary>
        /// When overridden in a derived class, performs control rendering.
        /// </summary>
        /// <remarks>
        /// This method is invo0ked before any children of the current <see cref="Control"/> have started their rendering.
        /// Use it to begin XML or XHTML tags, for example.
        /// </remarks>
        /// <param name="context"></param>
        protected virtual void RenderBegin(RenderingContext context)
        {
            StreamWriter writer = new StreamWriter(context.OutputStream, context.OutputEncoding);

            writer.Write("<" + this.Name);

            if (this.CanContainAttributes && this.Attributes.Count > 0)
            {
                writer.Write(" " + string.Join(" ", (from a in this.Attributes
                                                     where a.Include == true
                                                     select a.Name + "=\"" + a.Value + "\"").ToArray()));
            }

            if (!this.CanContainControls || this.Controls.Count == 0)
            {
                writer.Write(" />");
            }
            else
            {
                writer.Write(">");
            }

            writer.Flush();
        }
        /// <summary>
        /// When overridden in a derived class, performs control rendering.
        /// </summary>
        /// <remarks>
        /// This method is invoked after all the children of the current <see cref="Control"/> have completed their rendering.
        /// It should be used to close any open XML or XHTML tags, for example.
        /// </remarks>
        /// <param name="context"></param>
        protected virtual void RenderEnd(RenderingContext context)
        {
            if (!this.CanContainControls)
            {
                return;
            }
            else if (this.Controls.Count > 0)
            {
                StreamWriter writer = new StreamWriter(context.OutputStream, context.OutputEncoding);

                writer.Write("</" + this.Name + ">");
                writer.Flush();
            }
        }
        #endregion
        #region Methods - Public
        /// <summary>
        /// Renders the current <see cref="Control"/> against the specified <see cref="RenderingContext"/>.
        /// </summary>
        /// <param name="context"></param>
        public void Render(RenderingContext context)
        {
            this.RenderBegin(context);
            if (this.CanContainControls)
            {
                foreach (Control c in this.Controls)
                {
                    c.Render(context);
                }
            }
            this.RenderEnd(context);
        }
        #endregion
        #region Properties - Protected
        /// <summary>
        /// Gets a value that indicates if the current <see cref="Control"/> supports containing other controls.
        /// </summary>
        protected virtual bool CanContainControls
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// Gets a value that indicates if the current <see cref="Control"/> supports containing attributes.
        /// </summary>
        protected virtual bool CanContainAttributes
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// Gets the default name of the current <see cref="Control"/> if none is specified elsewhere.
        /// </summary>
        protected virtual string DefaultName
        {
            get
            {
                return Control.DefaultControlName;
            }
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets a collection of attributes that belong to the current <see cref="Control"/>.
        /// </summary>
        public ControlAttributeCollection Attributes
        {
            get
            {
                return this.attributes;
            }
        }
        /// <summary>
        /// Gets a string that classifies the current <see cref="Control"/>.
        /// </summary>
        public string Classification
        {
            get
            {
                return this.classification;
            }
            set
            {
                this.classification = value;
            }
        }
        /// <summary>
        /// Gets the id of the current <see cref="Control"/>.
        /// </summary>
        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }
        /// <summary>
        /// Gets the name of the current <see cref="Control"/>.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
        /// <summary>
        /// Gets the controls that are children of the current <see cref="Control"/>.
        /// </summary>
        public ControlCollection Controls
        {
            get
            {
                return this.controls;
            }
        }
        /// <summary>
        /// Gets a string that contains style information for the current <see cref="Control"/>.
        /// </summary>
        public string Style
        {
            get
            {
                return this.style;
            }
            set
            {
                this.style = value;
            }
        }
        #endregion
    }
}
