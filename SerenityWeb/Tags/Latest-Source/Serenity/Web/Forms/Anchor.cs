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
    public class Anchor : Control
    {
        #region Constructors - Public
        public Anchor()
            : this(null, null)
        {
        }
        public Anchor(Uri target)
            : this(target, null)
        {
        }
        public Anchor(Uri target, string text)
        {
            this.Target = target;

            if (text != null)
            {
                this.Controls.Add(new TextControl(text));
            }

            this.Attributes.Add(this.targetAttribute);
        }
        #endregion
        #region Fields - Private
        private Uri target;
        private ControlAttribute targetAttribute = new ControlAttribute("href");
        #endregion
        #region Properties - Protected
        protected override string DefaultName
        {
            get
            {
                return "a";
            }
        }
        #endregion
        #region Properties - Public
        public Uri Target
        {
            get
            {
                return this.target;
            }
            set
            {
                this.target = value;
                if (value == null)
                {
                    this.targetAttribute.Include = false;
                }
                else
                {
                    this.targetAttribute.Include = true;
                    this.targetAttribute.Value = value.ToString();
                }
            }
        }
        #endregion
    }
}
