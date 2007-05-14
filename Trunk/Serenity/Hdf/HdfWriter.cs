/*
Serenity - The next evolution of web server technology
Serenity/Hdf/IO/HdfWriter.cs
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
            this.settings = settings;
        }
        #endregion
        #region Fields - Private
        private readonly HdfWriterSettings settings;
        #endregion
        #region Methods - Private
        private void WriteElement(Stream stream, HdfElement element)
        {
            switch (this.settings.Format)
            {
                default:
                case HdfFormat.Simple:
                    if (element.HasValue)
                    {
                        this.WriteString(stream, element.Path + " = " + element.Value + "\r\n");
                    }
                    if (element.HasChildren)
                    {
                        foreach (HdfElement item in element)
                        {
                            this.WriteElement(stream, item);
                        }
                    }
                    break;

                case HdfFormat.Nested:
                    int depth = element.Depth;
                    if (element.HasValue)
                    {
                        this.WriteString(stream, element.Name.PadLeft(element.Name.Length + depth, '\t') + " = " + element.Value + "\r\n");
                    }
                    else if (element.HasChildren)
                    {
                        this.WriteString(stream, element.Name.PadLeft(element.Name.Length + depth, '\t') + "\r\n");
                    }
                    if (element.HasChildren)
                    {
                        string s = "{\r\n";
                        this.WriteString(stream, s.PadLeft(s.Length + depth, '\t'));

                        foreach (HdfElement item in element)
                        {
                            this.WriteElement(stream, item);
                        }
                        s = "}\r\n";
                        this.WriteString(stream, s.PadLeft(s.Length + depth, '\t'));
                    }
                    break;
            }
        }
        private void WriteString(Stream stream, string value)
        {
            byte[] buffer = this.settings.Encoding.GetBytes(value);
            stream.Write(buffer, 0, buffer.Length);
        }
        #endregion
        #region Methods - Public
        public override void Write(Stream stream, HdfDataset obj)
        {
            if (stream.CanWrite)
            {
                foreach (HdfElement element in obj)
                {
                    this.WriteElement(stream, element);
                }
            }
            else
            {
                throw new NotSupportedException("The supplied stream does not support writing, which is a required capability");
            }
        }

        public override void Dispose()
        {
            
        }
        #endregion
    }
}
