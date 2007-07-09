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
		private WebDriverStatus state = WebDriverStatus.None;
		#endregion
		#region Methods - Protected
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
		/// A call to this method may not immediately result in a Started state of the current WebDriver.
		/// </remarks>
		protected abstract bool DriverStart();
		/// <summary>
		/// When overridden in a derived class, causes the current WebDriver to cease operation.
		/// </summary>
		/// <remarks>
		/// A call to this method may not immediately result in a Stopped state of the current WebDriver.
		/// </remarks>
		protected abstract bool DriverStop();
		protected virtual void RecieveCallback(IAsyncResult ar)
		{
			if (ar.AsyncState.GetType().TypeHandle.Equals(typeof(WebDriverState).TypeHandle))
			{
				WebDriverState state = ar.AsyncState as WebDriverState;

				state.Signal.Set();
				state.WorkSocket.EndReceive(ar);
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

				state.Signal.Set();
				state.WorkSocket.EndSend(ar);
			}
			else if (ar.AsyncState is Socket)
			{
				((Socket)ar.AsyncState).EndSend(ar);
			}
		}
		protected virtual bool WriteContent(Socket socket, CommonContext context)
		{
			try
			{
				WebDriverState state = new WebDriverState();
				state.WorkSocket = socket;
				state.Signal.Reset();
				socket.BeginSend(context.Response.SendBuffer, 0, context.Response.SendBuffer.Length, SocketFlags.None, new AsyncCallback(this.SendCallback), socket);
				state.Signal.WaitOne();
				return true;
			}
			catch
			{
				return false;
			}
		}
		protected abstract bool WriteHeaders(Socket socket, CommonContext context);
		#endregion
		#region Methods - Public
		/// <summary>
		/// Publicly used method to perform pre-start initialization tasks.
		/// </summary>
		public void Initialize()
		{
			if (this.state < WebDriverStatus.Initialized)
			{
				this.DriverInitialize();
				this.state = WebDriverStatus.Initialized;
			}
		}
		public abstract bool ReadContext(Socket socket, out CommonContext context);
		/// <summary>
		/// Starts the WebDriver.
		/// </summary>
		public bool Start()
		{
			if (this.state == WebDriverStatus.Initialized)
			{
				return this.DriverStart();
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
			if (this.state == WebDriverStatus.Started)
			{
				return this.DriverStop();
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

			if (context.HeadersWritten)
			{
				return this.WriteContent(socket, context);
			}
			else
			{
				return false;
			}
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
				if (this.state >= WebDriverStatus.Initialized)
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
		/// Gets a boolean value that indicates whether the current WebDriver is in a started state.
		/// </summary>
		public bool IsStarted
		{
			get
			{
				if (this.state == WebDriverStatus.Started)
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
		/// Gets a boolean value that indicates whether the current WebDriver is in a stopped state.
		/// </summary>
		public bool IsStopped
		{
			get
			{
				if (this.state == WebDriverStatus.Stopped)
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
		/// Gets the state of the current WebDriver.
		/// </summary>
		public WebDriverStatus Status
		{
			get
			{
				return this.state;
			}
			protected set
			{
				this.state = value;
			}
		}
		#endregion
	}
}
