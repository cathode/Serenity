/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Web.Controls
{
    public sealed class IndexControl : Control
    {
        private string location = "/";

        #region Methods - Public
        public override byte[] Render(CommonContext context)
        {
            return Indexer.Standard.Generate(this.location);
        }
        #endregion

        public string Location
        {
            get
            {
                return this.location;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.location = value;
                }
            }
        }
    }
}
