/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *----------------------------------------------------------------------------*
 * Authors:                                                                   *
 * - Will 'AnarkiNet' Shelley (AnarkiNet@gmail.com): Original Author          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serenity.Web;
namespace Serenity.Net
{
    public class HttpServerAsyncState : ServerAsyncState
    {
        #region Fields
        private RequestProcessingStage stage;
        #endregion
        #region Methods
        public override void Reset()
        {
            this.Stage = RequestProcessingStage.None;
            base.Reset();
        }
        #endregion
        #region Properties
        public RequestProcessingStage Stage
        {
            get
            {
                return this.stage;
            }
            set
            {
                this.stage = value;
            }
        }
        #endregion
    }
}
