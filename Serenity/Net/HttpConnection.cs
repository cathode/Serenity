﻿/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Net.Sockets;
using System.Net;

namespace Serenity.Net
{
    /// <summary>
    /// Implements a client-server link that exchanges requests and responses using the HTTP protocol.
    /// </summary>
    public class HttpConnection : Connection
    {
        #region Fields
        #endregion
        #region Constructors
        public HttpConnection(Socket socket)
            : base(socket)
        {

        }
        #endregion
        #region Methods
        public override void ProcessBuffer(byte[] buffer)
        {
            unsafe
            {
                fixed (byte* b = &buffer[0])
                {
                    byte* n = b;
                    // Decrement loop is (generally) faster than increment loop due to CPU optimizations.
                    for (int i = buffer.Length - 1; i >= 0; --i)
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
