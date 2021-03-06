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

namespace Serenity.Web.Forms
{
    public class Image : Control
    {
        public Image()
        {
        }
        public Image(Uri target)
        {
            this.Target = target;
        }
        private Uri target;
        private string alternateText;
        private ControlAttribute srcAttribute;
        protected override string DefaultName
        {
            get
            {
                return "img";
            }
        }
        public Uri Target
        {
            get
            {
                return this.target;
            }
            set
            {
                this.target = value;
                if (this.srcAttribute == null)
                {
                    this.srcAttribute = new ControlAttribute("src");
                    this.srcAttribute.Include = true;
                    this.Attributes.Add(this.srcAttribute);
                }
                this.srcAttribute.Value = value.ToString();
                
            }
        }
        public string AlternateText
        {
            get
            {
                return this.alternateText;
            }
            set
            {
                this.alternateText = value;
            }
        }
    }
}
