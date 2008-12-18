/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *----------------------------------------------------------------------------*
 * Authors:                                                                   *
 * - Will 'AnarkiNet' Shelley (AnarkiNet@gmail.com): Original Author          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Serenity.Net
{
    /// <summary>
    /// Provides <see cref="Server"/> implementation that communicates requests
    /// and responses using the HTTP protocol version 1.1.
    /// </summary>
    public class HttpServer : AsyncServer
    {
        #region Methods
        protected override void ReceiveCallback(IAsyncResult result)
        {
            base.ReceiveCallback(result);


        }
        #endregion
        #region Properties
        #endregion
    }
}
