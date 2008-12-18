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
using System.Net.Sockets;
using System.Net;

namespace Serenity.Net
{
    /// <summary>
    /// Adds additional functionality to support servers that utilize
    /// asynchronous I/O operations.
    /// </summary>
    public abstract class AsyncServer : Server
    {
        #region Fields
        private Socket listener;
        #endregion
        #region Methods
        /// <summary>
        /// Provides a callback method for asynchronous accept operations.
        /// </summary>
        /// <param name="result"></param>
        protected virtual void AcceptCallback(IAsyncResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }
            var state = (AsyncServerState)result.AsyncState;

            state.Client = this.Listener.EndAccept(result);
            state.Client.BeginReceive(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, new AsyncCallback(this.ReceiveCallback), state);

            this.Listener.BeginAccept(new AsyncCallback(this.AcceptCallback),
                new AsyncServerState()
                {
                    Listener = state.Listener
                });
        }
        /// <summary>
        /// Overridden. Invokes the <see cref="Server.Initializing"/> event and
        /// performs initialization tasks such as creating and binding the
        /// listening socket and loading modules.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInitializing(EventArgs e)
        {
            base.OnInitializing(e);

            this.listener = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            this.listener.Bind(this.Profile.LocalEndPoint);
        }
        /// <summary>
        /// Overridden. Invokes the <see cref="Server.Starting"/> event and
        /// performs tasks like setting up the listening socket to begin
        /// listening for incoming connections.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStarting(EventArgs e)
        {
            base.OnStarting(e);

            this.Listener.Listen(this.Profile.ConnectionBacklog);

            this.Listener.BeginAccept(new AsyncCallback(this.AcceptCallback),
                new AsyncServerState()
                {
                    Listener = this.Listener,
                });
        }
        /// <summary>
        /// Overridden. Invokes the <see cref="Server.Stopping"/> event and
        /// performs tasks like shutting down the listening socket.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStopping(EventArgs e)
        {
            base.OnStopping(e);

            this.Listener.Shutdown(SocketShutdown.Both);
        }
        /// <summary>
        /// Provides a callback method for asynchronous socket receive calls.
        /// </summary>
        /// <param name="result"></param>
        protected virtual void ReceiveCallback(IAsyncResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the <see cref="Socket"/> that is used to listen for incoming
        /// connections from clients.
        /// </summary>
        protected Socket Listener
        {
            get
            {
                return this.listener;
            }
        }
        #endregion
    }
}
