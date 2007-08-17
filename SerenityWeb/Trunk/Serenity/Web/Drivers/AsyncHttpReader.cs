/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright � 2006-2007 Serenity Project - http://SerenityProject.net/       *
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

namespace Serenity.Web.Drivers
{
    public sealed class AsyncHttpReader : Reader<CommonContext>
    {
        public override CommonContext Read(System.IO.Stream stream, out bool result)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
