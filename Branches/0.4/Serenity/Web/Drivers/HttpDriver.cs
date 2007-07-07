/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Serenity.Web.Drivers
{
    /// <summary>
    /// Provides a WebDriver implementation that provides support for the HTTP protocol.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class HttpDriver : WebDriver
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the HttpDriver class.
        /// </summary>
        /// <param name="contextHandler">The context handler to use for the operation of the new HttpDriver.</param>
        public HttpDriver(ContextHandler contextHandler) : base(contextHandler)
        {
            this.Info = new DriverInfo("Serenity", "HyperText Transmission Protocol", "http", new Version(1, 1));
        }
        #endregion
        #region Destructor
        ~HttpDriver()
        {
            this.listenSocket.Close();
        }
        #endregion
        #region Fields - Private
        private Socket listenSocket;
        private ManualResetEvent allDone = new ManualResetEvent(false);
        #endregion
		#region Methods - Private
		private void AcceptCallback(IAsyncResult ar)
        {
            this.allDone.Set();

			using (Socket socket = ((Socket)ar.AsyncState).EndAccept(ar))
			{
				WebAdapter adapter = this.CreateAdapter();

				while (socket.Connected)
				{
					CommonContext context = new CommonContext(adapter);
					if (adapter.ReadContext(socket, out context) && context != null)
					{
						this.InvokeContextCallback(context);

						adapter.WriteContext(socket, context);
					}
					else
					{
						socket.BeginDisconnect(false, new AsyncCallback(this.DisconnectCallback), socket);
					}
				}
				socket.Close();
			}
        }
        #endregion
        #region Methods - Protected
        protected override bool DriverInitialize()
        {
            this.listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            try
            {
                this.ListenPort = this.Settings.ListenPort;
                this.listenSocket.Bind(new IPEndPoint(IPAddress.Any, this.ListenPort));
            }
            catch
            {
                for (int I = 0; I < this.Settings.FallbackPorts.Length; I++)
                {
                    try
                    {
                        this.ListenPort = this.Settings.FallbackPorts[I];
                        this.listenSocket.Bind(new IPEndPoint(IPAddress.Any, this.ListenPort));
                        
                        break;
                    }
                    catch
                    {

                    }
                }
            }
            if (this.listenSocket.IsBound)
            {
                Log.Write("Listening socket bound to port " + this.ListenPort.ToString(), LogMessageLevel.Info);
                return true;
            }
            else
            {
                Log.Write("Failed to bind listening socket to any port!", LogMessageLevel.Warning);
                return false;
            }
        }

        protected override bool DriverStart()
        {
            this.State = WebDriverState.Started;
            this.listenSocket.Listen(10);

            while (this.State == WebDriverState.Started)
            {
                this.allDone.Reset();
                this.listenSocket.BeginAccept(new AsyncCallback(this.AcceptCallback), this.listenSocket);
                this.allDone.WaitOne();
            }
            return true;
        }

        protected override bool DriverStop()
        {
            this.State = WebDriverState.Stopped;
            return true;
        }
        #endregion
        #region Methods - Public
        public override WebAdapter CreateAdapter()
        {
            return new HttpAdapter();
        }
        #endregion
    }
}
