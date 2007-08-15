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
using System.IO;
using System.Text;

using Serenity.IO;

namespace Serenity.Presentation.Templates
{
    public sealed class CsTemplateReader : Reader<CsTemplate>
    {
        #region Constructors - Public
        public CsTemplateReader()
        {
        }
        #endregion
        #region Methods - Public
		public override CsTemplate Read(Stream stream, out bool result)
		{
			throw new Exception("The method or operation is not implemented.");
		}
        #endregion
	}
}
