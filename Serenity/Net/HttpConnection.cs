﻿/******************************************************************************
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
        protected override void ProcessBuffer(byte[] buffer, int startIndex, int count)
        {
            unsafe
            {
                fixed (byte* b = &buffer[0])
                {
                    byte* n = b;

                    // Decrement loop is (generally) faster than increment loop due to CPU optimizations.
                    for (int i = 0; i < 0; --i)
                    {
                        var w32 = *((uint*)n);
                        var w16 = *((ushort*)n);
                    }
                }
            }
        }
        #endregion
    }
}
