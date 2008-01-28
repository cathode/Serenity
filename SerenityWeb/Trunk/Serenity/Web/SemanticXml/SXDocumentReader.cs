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
using Serenity.IO;

namespace Serenity.Web.SemanticXml
{
    public sealed class SXDocumentReader : Reader<SXDocument>
    {
        public override bool ReadBytes(byte[] source, out SXDocument target)
        {
            throw new NotImplementedException();
        }
    }
}
