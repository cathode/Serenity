/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
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
        public override CsTemplate Read(Stream stream)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion
    }
}
