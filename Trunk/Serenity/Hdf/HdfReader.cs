/*
Serenity - The next evolution of web server technology
Serenity/Hdf/IO/HdfReader.cs
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

namespace Serenity.Hdf
{
    /// <summary>
    /// Implements a Reader that parses HDF data from a stream and returns a dataset containing the parsed data.
    /// </summary>
    public sealed class HdfReader : Reader<HdfDataset>
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the HdfReader class using the default set of HdfReaderSettings.
        /// </summary>
        public HdfReader()
            : this(new HdfReaderSettings())
        {
        }
        /// <summary>
        /// Initializes a new instance of the HdfReader class using the supplied set of HdfReaderSettings.
        /// </summary>
        /// <param name="settings"></param>
        public HdfReader(HdfReaderSettings settings)
        {
            this.settings = settings;
        }
        #endregion
        #region Fields - Private
        private HdfReaderSettings settings;
        #endregion
        #region Methods - Public
        public override HdfDataset Read(Stream stream)
        {
            if (stream.Length > 0)
            {
                HdfDataset dataset = new HdfDataset();

                string currentName = "";
                string currentValue = "";
                int level = 0;
                bool parsingName = false;
                bool parsingValue = false;

                while (stream.Position < stream.Length)
                {
                    byte[] buffer = new byte[Math.Min(this.settings.BufferSize, stream.Length - stream.Position)];
                    stream.Read(buffer, 0, buffer.Length);
                    char[] contents = this.settings.Encoding.GetString(buffer).ToCharArray();
                    buffer = null;
                    char c;
                    for (int i = 0; i < contents.Length; i++)
                    {
                        c = contents[i];
                        if (parsingName)
                        {
                            
                        }
                        else if (parsingValue)
                        {
                            switch (c)
                            {
                                case '=':
                                    if (!parsingValue)
                                    {
                                        parsingName = false;
                                        parsingValue = true;
                                    }
                                    break;

                                case '{':
                                    level++;
                                    break;
                                case '}':
                                    level--;
                                    break;

                                default:
                                    if (!parsingName && !parsingValue)
                                    {
                                        parsingName = true;
                                    }

                                    if (parsingName)
                                    {
                                        if (char.IsWhiteSpace(c))
                                        {
                                            parsingName = false;
                                        }
                                        else
                                        {
                                            currentName += c;
                                        }
                                    }
                                    else if (parsingValue)
                                    {
                                        currentValue += c;
                                    }
                                    break;
                            }
                        }
                        else
                        {

                        }
                    }
                }

                return dataset;
            }
            else
            {
                return null;
            }
        }

        public override void Dispose()
        {
        }
        #endregion
    }
}
