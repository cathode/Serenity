/******************************************************************************
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
using System.Net;
using System.Net.Sockets;

namespace Serenity.Net
{
    /// <summary>
    /// Provides a socket listen server that accepts incoming HTTP connections.
    /// </summary>
    public class HttpListenerAsync
    {
        #region Fields
        public const int DefaultPort = 80;
        private int port;
        #endregion
        #region Constructors
        public HttpListenerAsync()
        {
            this.port = HttpListenerAsync.DefaultPort;
        }
        public HttpListenerAsync(int port)
        {
        }
        #endregion
        #region Properties
        public int Port
        {
            get
            {
                return this.port;
            }
            set
            {
                Contract.Requires(value >= ushort.MinValue);
                Contract.Requires(value <= ushort.MaxValue);

                this.port = value;
            }
        }
        #endregion
        #region Methods
        protected virtual void AcceptAsync(SocketAsyncEventArgs e)
        {

        }

        #endregion
    }
}
