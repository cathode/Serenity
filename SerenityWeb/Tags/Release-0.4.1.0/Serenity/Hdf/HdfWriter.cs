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

namespace Serenity.Hdf
{
    /// <summary>
    /// Provides a way to write HdfDatasets to a stream.
    /// </summary>
    public sealed class HdfWriter : Writer<HdfDataset>
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the HdfWriter class, using the default HdfWriterSettings.
        /// </summary>
        public HdfWriter()
            : this(new HdfWriterSettings())
        {

        }
        /// <summary>
        /// Initializes a new instance of the HdfWriter class, using the supplied HdfWriterSettings.
        /// </summary>
        /// <param name="settings"></param>
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
        /// <summary>
        /// Writes the supplied HdfDataset to the supplied Stream,
        /// in the manner defined by the HdfWriterSettings that the
        /// current HdfWriter was created with.
        /// </summary>
        /// <param name="stream">The stream to be written to.</param>
        /// <param name="obj">The HdfDataset to write.</param>
        /// <returns>True if everything went okay, otherwise false.</returns>
        public override bool Write(Stream stream, HdfDataset obj)
        {
            if (stream.CanWrite)
            {
                foreach (HdfElement element in obj)
                {
                    this.WriteElement(stream, element);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Frees any unmanaged resources used by the current HdfWriter.
        /// </summary>
        public override void Dispose()
        {
            
        }
        #endregion
    }
}
