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

namespace Serenity.Pages.Admin
{
    public abstract class AdminFormBase : Document
    {
        #region Constructors - Protected
        protected AdminFormBase()
        {
            this.contentArea = new Division();
            this.footer = new Division();
            this.sideBar = new Division();
            this.topBar = new Division();

            this.Body.Controls.AddRange(
                this.topBar,
                this.sideBar,
                this.contentArea,
                this.footer);
        }
        #endregion
        #region Fields - Private
        private Division sideBar;
        private Division topBar;
        private Division contentArea;
        private Division footer;
        #endregion
        #region Properties - Public
        public Division ContentArea
        {
            get
            {
                return this.contentArea;
            }
        }
        public Division Footer
        {
            get
            {
                return this.footer;
            }
        }
        public Division SideBar
        {
            get
            {
                return this.sideBar;
            }
        }
        public Division TopBar
        {
            get
            {
                return this.topBar;
            }
        }
        #endregion
    }
}
