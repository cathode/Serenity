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
    public delegate void WebFormsEvent(EventArgs e);

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class WebFormsEventAttribute : Attribute
    {
        #region Constructors - Public
        public WebFormsEventAttribute(string eventName, WebFormsEvent callback)
        {
            this.eventName = eventName;
            this.callback = callback;
        }
        #endregion
        #region Fields - Private
        private readonly string eventName;
        private readonly WebFormsEvent callback;
        #endregion
        #region Properties - Public
        public string EventName
        {
            get
            {
                return this.eventName;
            }
        }
        public WebFormsEvent Callback
        {
            get
            {
                return this.callback;
            }
        }
        #endregion
    }
}
