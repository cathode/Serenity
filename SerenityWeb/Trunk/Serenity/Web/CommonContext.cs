/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

using Serenity.Web.Drivers;

namespace Serenity.Web
{
	/// <summary>
	/// Encapsulates a CommonRequest/CommonResponse pair.
	/// </summary>
	public sealed class CommonContext
	{
		#region Constructors - Internal
		internal CommonContext(WebDriver driver)
		{
			this.request = new CommonRequest(this);
			this.response = new CommonResponse(this);
			this.driver = driver;
		}
		#endregion
		#region Fields - Private
		private WebDriver driver;
		private bool headersWritten = false;
		private string protocolType;
		private Version protocolVersion;
		private CommonRequest request;
		private CommonResponse response;
		private Socket socket;
		private bool supportsAuthentication;
		private bool supportsChunkedTransfer;
		private bool supportsContentControl;
		private bool supportsFields;
		private bool supportsHeaders;
		private bool supportsPeerInfo;
		#endregion
		#region Properties - Internal
		internal Socket Socket
		{
			get
			{
				return this.socket;
			}
			set
			{
				this.socket = value;
			}
		}
		#endregion
		#region Properties - Public
		/// <summary>
		/// Gets the WebDriver from which the current CommonContext originated from.
		/// </summary>
		public WebDriver Driver
		{
			get
			{
				return this.driver;
			}
		}
		/// <summary>
		/// Gets or sets a value indicating if the headers contained in the current CommonContext's
		/// CommonResponse have been sent to the client yet.
		/// </summary>
		public bool HeadersWritten
		{
			get
			{
				return this.headersWritten;
			}
			set
			{
				this.headersWritten = value;
			}
		}
		/// <summary>
		/// Gets a string describing the protocol type that handled the current CommonContext.
		/// </summary>
		public string ProtocolType
		{
			get
			{
				return this.protocolType;
			}
			set
			{
				this.protocolType = value;
			}
		}
		/// <summary>
		/// Gets the Version of the protocol type that handled the current CommonContext.
		/// </summary>
		public Version ProtocolVersion
		{
			get
			{
				return this.protocolVersion;
			}
			set
			{
				this.protocolVersion = value;
			}
		}
		/// <summary>
		/// Gets the underlying CommonRequest instance for the current CommonContext.
		/// </summary>
		public CommonRequest Request
		{
			get
			{
				return this.request;
			}
		}
		/// <summary>
		/// Gets the underlying CommonResponse instance for the current CommonContext.
		/// </summary>
		public CommonResponse Response
		{
			get
			{
				return this.response;
			}
		}
		/// <summary>
		/// Gets a boolean value which indicates if authentication is supported.
		/// </summary>
		public bool SupportsAuthentication
		{
			get
			{
				return this.supportsAuthentication;
			}
			set
			{
				this.supportsAuthentication = value;
			}
		}
		/// <summary>
		/// Gets a boolean value which indicates if the transmission can be sent
		/// in pieces smaller than the entire transmission.
		/// </summary>
		public bool SupportsChunkedTransfer
		{
			get
			{
				return this.supportsChunkedTransfer;
			}
			set
			{
				this.supportsChunkedTransfer = value;
			}
		}
		/// <summary>
		/// Gets a value which indicates if the sent or recieved content can be controlled,
		/// e.g. the encoding, mimetype, etc.
		/// </summary>
		public bool SupportsContentControl
		{
			get
			{
				return this.supportsContentControl;
			}
			set
			{
				this.supportsContentControl = value;
			}
		}
		/// <summary>
		/// Gets a value which indicates if arbitrary named fields are supported.
		/// </summary>
		public bool SupportsFields
		{
			get
			{
				return this.supportsFields;
			}
			set
			{
				this.supportsFields = value;
			}
		}
		/// <summary>
		/// Gets a boolean value which indicates if arbitrary sets of headers are supported.
		/// </summary>
		public bool SupportsHeaders
		{
			get
			{
				return this.supportsHeaders;
			}
			set
			{
				this.supportsHeaders = value;
			}
		}
		/// <summary>
		/// Gets a value which indicates if obtaining information about the peer is supported.
		/// </summary>
		public bool SupportsPeerInfo
		{
			get
			{
				return this.supportsPeerInfo;
			}
			set
			{
				this.supportsPeerInfo = value;
			}
		}
		#endregion
	}
}
