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
                int sleepTime = 0;
                WebAdapter adapter = this.CreateAdapter();

                while (socket.Connected == true)
                {
                    if (socket.Available > 0)
                    {
                        CommonContext context = new CommonContext(adapter);
                        adapter.ReadContext(socket, out context);

                        this.InvokeContextCallback(context);
                    }
                    else
                    {
                        if (sleepTime <= this.Settings.RecieveTimeout)
                        {
                            Thread.Sleep(this.Settings.RecieveInterval);
                            sleepTime += this.Settings.RecieveInterval;
                        }
                        else
                        {
                            socket.Close();
                        }
                    }
                }
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
        #region Old
        /*
        #region Fields - Private
        private int usedListenPort;
        #endregion
        #region Methods - Private
        private void HandleAcceptedSocket(object socketObject)
        {
            if (socketObject is Socket)
            {
                using (Socket socket = (Socket)socketObject)
                {
                    int sleepTime = 0;
                    
                    List<Byte> recieveBuffer = new List<Byte>(socket.Available);
                    
                    WebAdapter adapter = this.CreateAdapter();

                    WebDriverSettings settings = this.Settings;

                    while (socket.Connected == true)
                    {
                        if (socket.Available > 0)
                        {
                            byte[] Temp = new byte[socket.Available];
                            socket.Receive(Temp);
                            recieveBuffer.AddRange(Temp);
                            sleepTime = 0;
                        }
                        else
                        {
                            if (sleepTime <= settings.RecieveTimeout)
                            {
                                Thread.Sleep(settings.RecieveInterval);
                                sleepTime += settings.RecieveInterval;
                            }
                            else
                            {
                                socket.Close();
                                return;
                            }
                        }
                    }
                    if ((this.State == WebDriverState.Stopping) || (this.State == WebDriverState.Stopped))
                    {
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close(100);
                    }
                }
            }
            else
            {
                throw new ArgumentException("AcceptedObject parameter must be a System.Net.Socket instance!");
            }
        }
        #endregion
        #region Methods - Protected
        protected override void DriverInitialize()
        {

        }
        protected override void DriverStart()
        {
            using (Socket ListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP))
            {
                try
                {
                    this.usedListenPort = this.ListenPort;
                    ListenSocket.Bind(new IPEndPoint(IPAddress.Any, this.usedListenPort));
                    Log.Write("Listening on port " + this.usedListenPort.ToString(), LogMessageLevel.Info);
                }
                catch
                {
                    for (int I = 0; I < this.Settings.FallbackPorts.Length; I++)
                    {
                        try
                        {
                            this.usedListenPort = this.Settings.FallbackPorts[I];
                            ListenSocket.Bind(new IPEndPoint(IPAddress.Any, this.usedListenPort));
                            Log.Write("Listening on port " + this.usedListenPort.ToString(), LogMessageLevel.Info);
                            break;
                        }
                        catch
                        {

                        }
                    }
                }
                finally
                {
                    ListenSocket.Listen(10);
                }
                this.State = WebDriverState.Running;
                WaitCallback SocketHandleCallback = new WaitCallback(this.HandleAcceptedSocket);
                while (true)
                {
                    ThreadPool.QueueUserWorkItem(SocketHandleCallback, ListenSocket.Accept());
                    if ((this.State == WebDriverState.Stopping) || (this.State == WebDriverState.Stopped))
                    {
                        break;
                    }
                }
            }
        }
        
        protected override void DriverStop()
        {
            using (Socket cleanupSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP))
            {
                try
                {
                    cleanupSocket.Connect(new IPEndPoint(IPAddress.Loopback, this.usedListenPort));
                    cleanupSocket.Close();
                }
                catch
                {

                }
            }
            this.State = WebDriverState.Stopped;
        }
        #endregion
        */
        #endregion
    }
}
