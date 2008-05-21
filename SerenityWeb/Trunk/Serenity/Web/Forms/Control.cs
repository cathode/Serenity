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
            this.controls = new ControlCollection();
        }
        #endregion
        #region Fields - Private
        private string name = Control.DefaultControlName;
        private readonly ControlCollection controls;
        private string id;
        private string classification;
        private string style;
        private ControlAttributeCollection attributes;
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
        /// <param name="output"></param>
        protected virtual void RenderBegin(RenderingContext context)
        {
            using (StreamWriter writer = new StreamWriter(context.OutputStream, context.OutputEncoding))
            {
                writer.Write("<" + this.Name);

                var v = from a in this.attributes
                        where a.Include == true
                        let s = " " + a.Name + "=\"" + a.Value + "\""
                        select s;
                
                foreach (string s in v)
                {
                    writer.Write(s);
                }
                if (this.Controls.Count == 0)
                {
                    writer.Write(" />");
                }
                else
                {
                    writer.Write(">");
                }
            }
        }
        /// <summary>
        /// When overridden in a derived class, performs control rendering.
        /// </summary>
        /// <remarks>
        /// This method is invoked after all the children of the current <see cref="Control"/> have completed their rendering.
        /// It should be used to close any open XML or XHTML tags, for example.
        /// </remarks>
        /// <param name="output"></param>
        protected virtual void RenderEnd(RenderingContext context)
        {
            using (StreamWriter writer = new StreamWriter(context.OutputStream, context.OutputEncoding))
            {
                writer.Write("</" + this.Name + ">");
            }
        }
        #endregion
        #region Properties - Protected
        protected virtual bool CanContainChildren
        {
            get
            {
                return true;
            }
        }
        #endregion
        #region Properties - Public
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
        #endregion
    }
}
