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
using System.Threading;

namespace Serenity.Net
{
    /// <summary>
    /// Provides a socket listen server that accepts incoming HTTP connections.
    /// </summary>
    public class HttpConnectionListener : ConnectionListener<HttpConnection>
    {
        #region Fields
        /// <summary>
        /// Holds an internally-used sychronization object instance.
        /// </summary>
        private readonly object padlock = new object();
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see href="HttpConnectionListener" /> class.
        /// </summary>
        public HttpConnectionListener()
        {
        }

        public HttpConnectionListener(int port)
        {
            Contract.Requires(port >= ushort.MinValue);
            Contract.Requires(port <= ushort.MaxValue);
        }
        #endregion
        #region Properties
        /// <summary>
        /// Holds the default port number used for the HTTP protocol (80).
        /// </summary>
        public override int DefaultPort
        {
            get
            {
                return 80;
            }
        }
        #endregion
        #region Methods
        protected override HttpConnection CreateConnection(Socket socket)
        {
            return new HttpConnection(socket);
        }
        #endregion
    }
}
