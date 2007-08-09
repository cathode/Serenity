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
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Serenity.Web.Drivers
{
	/// <summary>
	/// Provides a simple data structure used to pass objects to and from async callback methods.
	/// </summary>
	public sealed class WebDriverState
	{
		#region Constructors - Public
		/// <summary>
		/// Initializes a new instance of the WebDriverState class using the default buffer size.
		/// </summary>
		public WebDriverState()
			: this(WebDriverState.DefaultBufferSize)
		{
		}
		/// <summary>
		/// Initializes a new instance of the WebDriverState class using the supplied buffer size.
		/// </summary>
		/// <param name="bufferSize"></param>
		public WebDriverState(int bufferSize)
		{
			this.Buffer = new byte[bufferSize];
		}
		#endregion
		#region Fields - Private
		private byte[] buffer = new byte[DefaultBufferSize];
		private Socket workSocket;
		private ManualResetEvent signal = new ManualResetEvent(false);
		#endregion
		#region Fields - Public
		/// <summary>
		/// Holds the maximum buffer size.
		/// </summary>
		public const int MaxBufferSize = 65536;
		/// <summary>
		/// Holds the minimum buffer size.
		/// </summary>
		public const int MinBufferSize = 32;
		/// <summary>
		/// Holds the default (optimal) buffer size.
		/// </summary>
		public const int DefaultBufferSize = MinBufferSize * 4;
		#endregion
		#region Properties - Public
		/// <summary>
		/// Gets or sets the data buffer associated with the current WebDriverState.
		/// </summary>
		public byte[] Buffer
		{
			get
			{
				return this.buffer;
			}
			set
			{
				if (value != null && value.Length > WebDriverState.MinBufferSize)
				{
					this.buffer = value;
				}
			}
		}
		/// <summary>
		/// Gets the ManualResetEvent, used to assist in syncronization, associated
		/// with the current WebDriverState.
		/// </summary>
		public ManualResetEvent Signal
		{
			get
			{
				return this.signal;
			}
		}
		/// <summary>
		/// Gets or sets the socket, used to perform operations on, associated with the
		/// current WebDriverState.
		/// </summary>
		public Socket WorkSocket
		{
			get
			{
				return this.workSocket;
			}
			set
			{
				this.workSocket = value;
			}
		}
		#endregion
	}
}
