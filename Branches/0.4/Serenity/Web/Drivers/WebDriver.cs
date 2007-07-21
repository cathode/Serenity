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
				state.WorkSocket.EndDisconnect(ar);
				state.Signal.Set();
			}
			else if (ar.AsyncState is Socket)
			{
				((Socket)ar.AsyncState).EndDisconnect(ar);
			}
		}
		protected virtual void HandleAcceptedSocket(object socket)
		{
			this.HandleAcceptedSocket(socket as Socket);
		}
		protected virtual void HandleAcceptedSocket(Socket socket)
		{
			if (socket != null)
			{
				CommonContext context = new CommonContext(this);
				context.Socket = socket;

				if (this.ReadContext(socket, out context) && context != null)
				{
					this.Settings.ContextHandler.HandleContext(context);

					if (!this.WriteContext(socket, context))
					{
						Log.Write("Failed to write context", LogMessageLevel.Warning);
					}
				}
				else
				{
					Log.Write("Failed to read context", LogMessageLevel.Warning);
				}
				
				if (this.Settings.Block)
				{
					socket.Disconnect(false);
				}
				else
				{
					socket.BeginDisconnect(false, new AsyncCallback(this.DisconnectCallback), socket);
				}
				socket.Close();
			}
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
			if (this.Settings.Block)
			{
				socket.Send(context.Response.OutputBuffer);
				context.Response.ClearOutputBuffer();
			}
			else
			{
				socket.BeginSend(context.Response.OutputBuffer, 0, context.Response.OutputBuffer.Length, SocketFlags.None, new AsyncCallback(this.SendCallback), socket);
			}
			return true;

		}
		protected abstract bool WriteHeaders(Socket socket, CommonContext context);
		#endregion
		#region Methods - Public
		/// <summary>
		/// Creates and binds the listening socket, preparing the WebDriver so that it can be started.
		/// </summary>
		public virtual bool Initialize()
		{
			if (this.status < WebDriverStatus.Initialized)
			{
				this.ListeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

				foreach (ushort port in this.Settings.Ports)
				{
					try
					{
						this.ListeningSocket.Bind(new IPEndPoint(IPAddress.Any, port));
						break;
					}
					catch
					{
					}
				}
				if (this.ListeningSocket.IsBound)
				{
					Log.Write("Listening socket bound to port " + this.ListeningPort.ToString(), LogMessageLevel.Info);
					this.status = WebDriverStatus.Initialized;
					return true;
				}
				else
				{
					Log.Write("Failed to bind listening socket to any port!", LogMessageLevel.Warning);
					return false;
				}
			}
			else
			{
				return false;
			}
		}
		public abstract bool ReadContext(Socket socket, out CommonContext context);

		/// <summary>
		/// Starts the WebDriver.
		/// </summary>
		public virtual bool Start()
		{
			if (this.Status >= WebDriverStatus.Initialized)
			{
				this.Status = WebDriverStatus.Started;
				this.ListeningSocket.Listen(10);

				if (this.Settings.Block)
				{
					while (this.Status == WebDriverStatus.Started)
					{
						ThreadPool.QueueUserWorkItem(new WaitCallback(this.HandleAcceptedSocket), this.ListeningSocket.Accept());
					}
					return true;
				}
				else
				{
					WebDriverState state = new WebDriverState();
					state.Signal.Reset();
					state.WorkSocket = this.ListeningSocket;
					this.ListeningSocket.BeginAccept(new AsyncCallback(this.AcceptCallback), state);

					return true;
				}
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// Stops the WebDriver.
		/// </summary>
		public virtual bool Stop()
		{
			if (this.status == WebDriverStatus.Started)
			{
				this.status = WebDriverStatus.Stopped;
				return true;
			}
			else
			{
				return false;
			}
		}
		public virtual bool WriteContext(Socket socket, CommonContext context)
		{
			if (!context.HeadersWritten)
			{
				context.HeadersWritten = this.WriteHeaders(socket, context);
			}

			bool ret = false;

			if (context.HeadersWritten)
			{
				ret = this.WriteContent(socket, context);
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
