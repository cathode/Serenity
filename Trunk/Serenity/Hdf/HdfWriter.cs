/*
Serenity - The next evolution of web server technology
Serenity/Hdf/IO/HdfWriter.cs
Copyright © 2006-2007 Serenity Project (http://serenityproject.net/)

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

namespace Serenity.Hdf
{
    public sealed class HdfWriter : Writer<HdfDataset>
    {
        #region Constructors - Public
        public HdfWriter()
            : this(new HdfWriterSettings())
        {

        }
        public HdfWriter(HdfWriterSettings settings)
        {

        }
        #endregion
        #region Methods - Public
        public override void Write(Stream stream, HdfDataset obj)
        {
            
        }

        public override void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion
    }
}
