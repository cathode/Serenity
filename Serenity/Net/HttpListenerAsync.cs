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
    public class HttpListenerAsync : IListener
    {
        #region Fields
        public const int DefaultPort = 80;
        private int port;
        private Socket listener;
        private ListenerState state;
        private readonly object padlock = new object();
        #endregion
        #region Constructors
        public HttpListenerAsync()
        {
            this.port = HttpListenerAsync.DefaultPort;
        }
        public HttpListenerAsync(int port)
        {
            Contract.Requires(port >= ushort.MinValue);
            Contract.Requires(port <= ushort.MaxValue);

            this.port = port;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the network port number that the current <see cref="HttpListenerAsync"/>
        /// will listen for connections on.
        /// </summary>
        public int Port
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= ushort.MinValue);
                Contract.Ensures(Contract.Result<int>() <= ushort.MaxValue);

                return this.port;
            }
            set
            {
                Contract.Requires(value >= ushort.MinValue);
                Contract.Requires(value <= ushort.MaxValue);

                this.port = value;
            }
        }
        public IPAddress Address
        {
            get
            {
                return null;
            }
            set
            {
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Causes the server to start listening for connections. This method is thread-safe.
        /// </summary>
        /// <returns>true if the server is in a <see cref="ListenerState.Started"/> or better state when this method returns; otherwise, false.</returns>
        public bool Start()
        {
            try
            {
                lock (this.padlock)
                {
                    if (this.state == ListenerState.Ready)
                    {
                        // Indicate that we're trying to start.
                        this.state = ListenerState.Starting;

                        try
                        {
                            // Try to make a dual-mode (IPv4 + IPv6) listen socket. This will fail pre-Vista.
                            this.listener = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                            this.listener.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
                            this.listener.Bind(new IPEndPoint(this.Address ?? IPAddress.IPv6Any, this.Port));
                        }
                        catch (SocketException ex)
                        {
                            // Fallback to a plain, boring IPv4 listener.
                            this.listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            this.listener.Bind(new IPEndPoint(this.Address ?? IPAddress.Any, this.Port));
                        }

                        this.listener.Listen(16);
                        this.AcceptAsync(null);

                        // Listener is started, so we indicate success.
                        this.state = ListenerState.Started;
                        return true;
                    }
                    else
                        return (this.state == ListenerState.Started);
                }
            }
            catch (SocketException ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Causes the server to stop listening for connections.
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            try
            {
                this.listener.Dispose();
                this.listener = null;
                this.state = ListenerState.Stopped;
                return true;
            }
            catch (SocketException ex)
            {
                this.state = ListenerState.Faulted;
                return false;
            }
        }

        /// <summary>
        /// Causes the server to stop listening for connections, then start
        /// listening for connections again using the options currently configured.
        /// </summary>
        /// <returns>True if the server successfully restarted; otherwise false.</returns>
        public bool Restart()
        {
            if (this.Stop())
            {
                this.state = ListenerState.Ready;
                return this.Start();
            }
            else
                return false;
        }
        public bool AcceptCallback(SocketAsyncEventArgs e)
        {
            return false;
        }

        public static unsafe int FindEOL(byte[] buffer)
        {
            byte[] buf = new byte[8] { 0, 1, 3, 7, 15, 31, 63, 127 };
            fixed (byte* b = &buf[0])
            {
                byte* n = b;
                // Decrement loop is (generally) faster than increment loop due to CPU optimizations.
                for (int i = buf.Length - 1; i >= 0; --i)
                {
                    Console.WriteLine(((uint)*((uint*)n++)).ToString("X8"));
                }
            }
           
            return -1;
        }
        protected virtual void AcceptAsync(SocketAsyncEventArgs e)
        {

        }

        #endregion



    }
}
