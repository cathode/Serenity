/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Serenity.Net
{
    /// <summary>
    /// Implements a client-server link that exchanges requests and responses using the HTTP protocol.
    /// </summary>
    public class HttpConnection : Connection
    {
        #region Fields
        /// <summary>
        /// Holds the little-endian (Intel) value of a standard HTTP End-of-Line (EOL) character sequence.
        /// </summary>
        private const ushort LittleEndianSingleEol = (byte)'\n' << 8 | (byte)'\r';

        /// <summary>
        /// Holds the little-endian (Intel) value of a standard HTTP double-End-of-Line (EOL) character sequence,
        /// which terminates the header block and the content block.
        /// </summary>
        private const uint LittleEndianDoubleEol = (byte)'\n' << 24 | (byte)'\r' << 16 | (byte)'\n' << 8 | (byte)'\r';

        /// <summary>
        /// Holds the big-endian (Motorola) value of a standard HTTP End-of-Line (EOL) character sequence.
        /// </summary>
        private const ushort BigEndianSingleEol = (byte)'\r' << 8 | (byte)'\n';

        /// <summary>
        /// Holds the big-endian (Motorola) value of a standard HTTP double-End-of-Line (EOL) character sequence,
        /// which terminates the header block and the content block.
        /// </summary>
        private const uint BigEndianDoubleEol = (byte)'\r' << 24 | (byte)'\n' << 16 | (byte)'\r' << 8 | (byte)'\n';

        /// <summary>
        /// Holds the system-endian value of a standard HTTP End-of-Line (EOL) character sequence.
        /// </summary>
        private static readonly ushort SystemSingleEol = BitConverter.IsLittleEndian ? LittleEndianSingleEol : BigEndianSingleEol;
        
        /// <summary>
        /// Holds the system-endian value of a standard HTTP double-End-of-Line (EOL) character sequence,
        /// which terminates the header block and the content block.
        /// </summary>
        private static readonly uint SystemDoubleEol = BitConverter.IsLittleEndian ? LittleEndianDoubleEol : BigEndianDoubleEol;
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpConnection"/> class.
        /// </summary>
        /// <param name="socket">The <see cref="Socket"/> used for transmitting data with the remote endpoint.</param>
        public HttpConnection(Socket socket)
            : base(socket)
        {
            Contract.Requires(socket != null);
        }
        #endregion
        #region Methods
        /// <summary>
        /// Overridden.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        protected override void ProcessBufferContents(byte[] buffer, int startIndex, int count)
        {
            Contract.Requires(buffer != null);

            int x = -2, y = 0;
            string debug = Encoding.ASCII.GetString(buffer, startIndex, count);
            List<string> lines = new List<string>();
            List<int> headerBreaks = new List<int>();
            List<int> sectionBreaks = new List<int>();

            unsafe
            {
                fixed (byte* b = &buffer[0])
                {
                    byte* n = (byte*)b;

                    int m = count;
                    for (int i = 0; i < m; ++i)
                    {
                        var w8 = *n;
                        if (w8 == ':')
                        {
                            headerBreaks.Add(i - x);
                        }
                        else if (w8 == '\r')
                        {
                            y = i;
                            var w16 = *((ushort*)n);
                            if (w16 == HttpConnection.SystemSingleEol)
                            {
                                x += 2;
                                var w32 = *((uint*)n);
                                if (w32 == HttpConnection.SystemDoubleEol)
                                {
                                    x += 2;
                                    sectionBreaks.Add(x);
                                }
                                else
                                    lines.Add(new string((sbyte*)b, x, y - x));
                            }
                            x = y;
                        }
                        ++n;
                    }
                }
            }
        }
        #endregion
    }
}
