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
using System.Net.Sockets;
using System.Net;
using System.Diagnostics.Contracts;

namespace Serenity.Net
{
    public abstract class Connection : IDisposable
    {
        #region Fields
        private readonly Socket socket;
        #endregion
        #region Constructors
        protected Connection(Socket socket)
        {
            Contract.Requires(socket != null);

            this.socket = socket;
        }
        #endregion
        #region Methods
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        public abstract void ProcessBuffer(byte[] buffer);
        #endregion

       
    }
}
