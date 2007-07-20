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
	/// Provides a mechanism for recieving and responding to requests from clients (browsers).
	/// </summary>
	public abstract class WebDriver
	{
		#region Constructors - Protected
		/// <summary>
		/// Initializes a new instance of the WebDriver class.
		/// </summary>
		/// <param name="contextHandler">A ContextHandler which handles
		/// incoming CommonContext objects.</param>
		protected WebDriver(WebDriverSettings settings)
		{
			this.settings = settings;
		}
		#endregion
		#region Destructor
		~WebDriver()
		{
			this.ListeningSocket.Close();
		}
		#endregion
		#region Fields - Private
		private Socket listeningSocket;
		private DriverInfo info;
		private WebDriverSettings settings;
		private WebDriverStatus status = WebDriverStatus.None;
		#endregion
		#region Methods - Protected
		protected virtual void AcceptCallback(IAsyncResult ar)
		{
			Socket workSocket;
			Socket socket;
			if (ar.AsyncState.GetType().TypeHandle.Equals(typeof(WebDriverState).TypeHandle))
			{
				WebDriverState state = (WebDriverState)ar.AsyncState;
				state.Signal.Set();
				workSocket = state.WorkSocket;
				socket = workSocket.EndAccept(ar);
				workSocket.BeginAccept(new AsyncCallback(this.AcceptCallback), state);
			}
			else if (ar.AsyncState is Socket)
			{
				workSocket = (Socket)ar.AsyncState;
				socket = workSocket.EndAccept(ar);
				workSocket.BeginAccept(new AsyncCallback(this.AcceptCallback), workSocket);
			}
			else
			{
				return;
			}
			this.HandleAcceptedSocket(socket);
		}
		/// <summary>
		/// Provides a callback method to use for an async socket disconnection.
		/// </summary>
		/// <param name="ar"></param>
		protected virtual void DisconnectCallback(IAsyncResult ar)
		{
			if (ar.AsyncState.GetType().TypeHandle.Equals(typeof(WebDriverState).TypeHandle))
			{
				WebDriverState state = ar.AsyncState as WebDriverState;

				state.Signal.Set();
				state.WorkSocket.EndDisconnect(ar);
			}
			else if (ar.AsyncState is Socket)
			{
				((Socket)ar.AsyncState).EndDisconnect(ar);
			}
		}
		/// <summary>
		/// Contains the code that is executed when the current WebDriver is initialized (before handling clients).
		/// </summary>
		protected abstract bool DriverInitialize();
		/// <summary>
		/// When overridden in a derived class, causes the current WebDriver to begin listening for
		/// and accepting incoming connections.
		/// </summary>
		/// <remarks>
		/// A call to this method may not immediately result in a Started status of the current WebDriver.
		/// </remarks>
		protected abstract bool DriverStart(bool block);
		/// <summary>
		/// When overridden in a derived class, causes the current WebDriver to cease operation.
		/// </summary>
		/// <remarks>
		/// A call to this method may not immediately result in a Stopped status of the current WebDriver.
		/// </remarks>
		protected abstract bool DriverStop();
		protected virtual void HandleAcceptedSocket(Socket socket)
		{
			CommonContext context = new CommonContext(this);
			context.Socket = socket;

			if (this.ReadContext(socket, out context) && context != null)
			{
				this.Settings.ContextHandler.HandleContext(context);

				this.WriteContext(socket, context, true);
			}

			socket.Shutdown(SocketShutdown.Both);
			socket.Close();
			socket = null;
		}
		protected virtual void RecieveCallback(IAsyncResult ar)
		{
			if (ar.AsyncState.GetType().TypeHandle.Equals(typeof(WebDriverState).TypeHandle))
			{
				WebDriverState state = ar.AsyncState as WebDriverState;

				state.WorkSocket.EndReceive(ar);
				state.Signal.Set();
			}
			else if (ar.AsyncState is Socket)
			{
				((Socket)ar.AsyncState).EndReceive(ar);
			}
		}
		protected virtual void SendCallback(IAsyncResult ar)
		{
			if (ar.AsyncState.GetType().TypeHandle.Equals(typeof(WebDriverState).TypeHandle))
			{
				WebDriverState state = ar.AsyncState as WebDriverState;
				state.WorkSocket.EndSend(ar);
				state.Signal.Set();
			}
			else if (ar.AsyncState is Socket)
			{
				((Socket)ar.AsyncState).EndSend(ar);
			}
		}
		protected bool WriteContent(Socket socket, CommonContext context)
		{
			return this.WriteContent(socket, context, false);
		}
		protected virtual bool WriteContent(Socket socket, CommonContext context, bool block)
		{
			if (block)
			{
				WebDriverState state = new WebDriverState();
				state.WorkSocket = socket;
				state.Signal.Reset();
				socket.BeginSend(context.Response.SendBuffer, 0, context.Response.SendBuffer.Length, SocketFlags.None, new AsyncCallback(this.SendCallback), state);
				state.Signal.WaitOne();
			}
			else
			{
				socket.BeginSend(context.Response.SendBuffer, 0, context.Response.SendBuffer.Length, SocketFlags.None, new AsyncCallback(this.SendCallback), socket);
			}
			return true;

		}
		protected bool WriteHeaders(Socket socket, CommonContext context)
		{
			return this.WriteHeaders(socket, context, false);
		}
		protected abstract bool WriteHeaders(Socket socket, CommonContext context, bool block);
		#endregion
		#region Methods - Public
		/// <summary>
		/// Publicly used method to perform pre-start initialization tasks.
		/// </summary>
		public void Initialize()
		{
			if (this.status < WebDriverStatus.Initialized)
			{
				this.DriverInitialize();
				this.status = WebDriverStatus.Initialized;
			}
		}
		public abstract bool ReadContext(Socket socket, out CommonContext context);

		/// <summary>
		/// Starts the WebDriver in non-blocking (asnychronus) mode.
		/// </summary>
		public bool Start()
		{
			return this.Start(false);
		}
		/// <summary>
		/// Starts the WebDriver. If block is true,
		/// the current thread will block while the current WebDriver is running.
		/// </summary>
		/// <param name="block"></param>
		/// <returns></returns>
		public bool Start(bool block)
		{
			if (this.status == WebDriverStatus.Initialized)
			{
				return this.DriverStart(block);
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// Stops the WebDriver.
		/// </summary>
		public bool Stop()
		{
			if (this.status == WebDriverStatus.Started)
			{
				return this.DriverStop();
			}
			else
			{
				return false;
			}
		}
		public bool WriteContext(Socket socket, CommonContext context)
		{
			return this.WriteContext(socket, context, false);
		}
		public virtual bool WriteContext(Socket socket, CommonContext context, bool block)
		{
			if (!context.HeadersWritten)
			{
				context.HeadersWritten = this.WriteHeaders(socket, context, block);
			}

			bool ret = false;

			if (context.HeadersWritten)
			{
				ret = this.WriteContent(socket, context, block);
			}
			return ret;
		}
		#endregion
		#region Properties - Protected
		protected Socket ListeningSocket
		{
			get
			{
				return this.listeningSocket;
			}
			set
			{
				this.listeningSocket = value;
			}
		}
		#endregion
		#region Properties - Public
		/// <summary>
		/// Gets the port number that the current WebDriver is listening on for incoming connections.
		/// </summary>
		public ushort ListeningPort
		{
			get
			{
				return (ushort)((IPEndPoint)this.listeningSocket.LocalEndPoint).Port;
			}
		}
		/// <summary>
		/// Gets a DriverInfo object which contains information about the current WebDriver.
		/// </summary>
		public DriverInfo Info
		{
			get
			{
				return this.info;
			}
			protected set
			{
				this.info = value;
			}
		}
		/// <summary>
		/// Gets a value that indicates whether the current WebDriver has been initialized yet.
		/// </summary>
		public bool IsInitialized
		{
			get
			{
				if (this.status >= WebDriverStatus.Initialized)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}
		/// <summary>
		/// Gets a boolean value that indicates whether the current WebDriver is in a started status.
		/// </summary>
		public bool IsStarted
		{
			get
			{
				if (this.status == WebDriverStatus.Started)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}
		/// <summary>
		/// Gets a boolean value that indicates whether the current WebDriver is in a stopped status.
		/// </summary>
		public bool IsStopped
		{
			get
			{
				if (this.status == WebDriverStatus.Stopped)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}
		/// <summary>
		/// Gets the WebDriverSettings which determine the behaviour of the current WebDriver.
		/// </summary>
		public WebDriverSettings Settings
		{
			get
			{
				return this.settings;
			}
		}
		/// <summary>
		/// Gets the status of the current WebDriver.
		/// </summary>
		public WebDriverStatus Status
		{
			get
			{
				return this.status;
			}
			protected set
			{
				this.status = value;
			}
		}
		#endregion
	}
}
