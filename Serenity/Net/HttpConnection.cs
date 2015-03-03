/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2015 William 'cathode' Shelley. All Rights Reserved.      *
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
using Serenity.Web;

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

            int x = -2;
            //string debug = Encoding.ASCII.GetString(buffer, startIndex, count);
            List<string> lines = new List<string>();
            List<int> headerBreaks = new List<int>();
            List<int> sectionBreaks = new List<int>();

            unsafe
            {
                fixed (byte* b = &buffer[0])
                {
                    byte* n = b;
                    sbyte* s = (sbyte*)b;
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
                            var w16 = *((ushort*)n);
                            if (w16 == HttpConnection.SystemSingleEol)
                            {
                                var w32 = *((uint*)n);
                                if (w32 == HttpConnection.SystemDoubleEol)
                                {
                                    sectionBreaks.Add(i);
                                }
                                else
                                    lines.Add(new string(s, x + 2, i - x - 2));
                            }
                            x = i;
                        }
                        ++n;
                    }

                }
                // Check if we received the whole request. The last line should be empty.
                if (lines.Count > 0 && lines[lines.Count - 1] == "")
                {
                    var line0Tokens = lines[0].Split(' ');

                    var context = new ResourceExecutionContext
                    {
                        Request = new Request
                        {
                            RawMethod = line0Tokens[0],
                            RawUrl = line0Tokens[1],
                        },
                        Response = new Response(),
                        Connection = this,
                    };

                    for (int i = 1; i < (lines.Count - 1); ++i)
                        context.Request.Headers.Add(new Header(lines[i].Substring(0, headerBreaks[i - 1] -2), lines[i].Substring(headerBreaks[i - 1])));

                    var host = context.Request.Headers["Host"].Value;
                    ushort port = 80;

                    if (host.IndexOf(':') > 0)
                    {
                        var portstring = host.Substring(host.IndexOf(':') + 1);
                        port = ushort.Parse(portstring);
                        host = host.Substring(0, host.IndexOf(':'));
                    }

                    var urlb = new UriBuilder("http", host);
                    urlb.Port = port;
                    urlb.Path = line0Tokens[1];
                    context.Request.Url = urlb.Uri;

                    this.QueueNewPendingContext(context);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
        #endregion
    }
}
