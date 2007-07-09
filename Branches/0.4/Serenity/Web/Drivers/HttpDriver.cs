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
        public HttpDriver(WebDriverSettings settings) : base(settings)
        {
            this.Info = new DriverInfo("Serenity", "HyperText Transmission Protocol", "http", new Version(1, 1));
        }
        #endregion
        #region Destructor
        ~HttpDriver()
        {
            this.ListeningSocket.Close();
        }
        #endregion
        #region Fields - Private
        private ManualResetEvent allDone = new ManualResetEvent(false);
        #endregion
		#region Methods - Private
		private void AcceptCallback(IAsyncResult ar)
		{
			this.allDone.Set();

			Socket socket = ((Socket)ar.AsyncState).EndAccept(ar);

			CommonContext context = new CommonContext(this);
			if (this.ReadContext(socket, out context) && context != null)
			{
				this.Settings.ContextHandler.HandleContext(context);

				this.WriteContext(socket, context);
			}
		}
		private void ProcessUrlEncodedRequestData(string input, CommonContext context)
		{
			string[] Pairs = input.Split('&');
			foreach (string Pair in Pairs)
			{
				if (Pair.IndexOf('=') != -1)
				{
					string Name = Pair.Substring(0, Pair.IndexOf('='));
					string Value = Pair.Substring(Pair.IndexOf('=') + 1);
					context.Request.RequestData.AddDataStream(Name, Encoding.UTF8.GetBytes(Value));
				}
			}
		}
		private void RecieveCallback(IAsyncResult ar)
		{
			WebDriverContext context = (WebDriverContext)ar.AsyncState;
			context.Signal.Set();
			context.WorkSocket.EndReceive(ar);
		}
		private void SendCallback(IAsyncResult ar)
		{
			if (ar.AsyncState is Socket)
			{
				try
				{
					((Socket)ar.AsyncState).EndSend(ar);
				}
				catch
				{
				}
			}
		}
        #endregion
        #region Methods - Protected
        protected override bool DriverInitialize()
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
				return false;
			}
            if (this.ListeningSocket.IsBound)
            {
                Log.Write("Listening socket bound to port " + this.ListeningPort.ToString(), LogMessageLevel.Info);
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
			if (this.State >= WebDriverState.Initialized)
			{
				this.State = WebDriverState.Started;
				this.ListeningSocket.Listen(10);

				while (this.State == WebDriverState.Started)
				{
					this.allDone.Reset();
					this.ListeningSocket.BeginAccept(new AsyncCallback(this.AcceptCallback), this.ListeningSocket);
					this.allDone.WaitOne();
				}
				this.ListeningSocket.EndAccept(null);
				return true;
			}
			else
			{
				return false;
			}
        }
        protected override bool DriverStop()
        {
            this.State = WebDriverState.Stopped;
            return true;
        }
		protected override bool WriteHeaders(Socket socket, CommonContext context)
		{
			if (socket != null && socket.Connected)
			{
				CommonRequest request = context.Request;
				CommonResponse response = context.Response;
				StringBuilder outputText = new StringBuilder();

				outputText.Append("HTTP/1.1 " + response.Status.ToString() + "\r\n");

				if (response.Headers.Contains("Content-Length") == false)
				{
					response.Headers.Add("Content-Length", response.SendBuffer.Length.ToString());
				}
				if (response.Headers.Contains("Content-Type") == false)
				{
					response.Headers.Add("Content-Type", response.MimeType.ToString() + "; charset=UTF-8");
				}
				if (!response.Headers.Contains("Server"))
				{
					response.Headers.Add(new Header("Server", SerenityInfo.Name + "/" + SerenityInfo.Version));
				}

				foreach (Header header in response.Headers)
				{
					string value;
					if (header.Complex == true)
					{
						switch (header.Name)
						{
							default:
								value = string.Format("{0},{1}", header.PrimaryValue, string.Join(",", header.SecondaryValues)).TrimEnd('\r', '\n');
								break;
						}
					}
					else
					{
						value = header.PrimaryValue.TrimEnd('\r', '\n');
					}
					outputText.Append(header.Name + ": " + value + "\r\n");
				}
				outputText.Append("\r\n");

				byte[] output = Encoding.ASCII.GetBytes(outputText.ToString());

				try
				{
					socket.BeginSend(output, 0, output.Length, SocketFlags.None, new AsyncCallback(this.SendCallback), null);
				}
				catch
				{
					return false;
				}
				return true;
			}
			else
			{
				return false;
			}
		}
		protected override bool WriteContent(Socket socket, CommonContext context)
		{
			try
			{
				socket.BeginSend(context.Response.SendBuffer, 0, context.Response.SendBuffer.Length, SocketFlags.None, new AsyncCallback(this.SendCallback), socket);
			}
			catch
			{
				return false;
			}
			return true;
		}
        #endregion
		#region Methods - Public
		public override bool ReadContext(Socket socket, out CommonContext context)
		{
			if (socket.Available == 0)
			{
				context = null;
				return false;
			}

			context = new CommonContext(this);

			WebDriverContext state = new WebDriverContext();
			state.Buffer = new byte[socket.Available];
			state.WorkSocket = socket;

			try
			{
				state.Signal.Reset();
				socket.BeginReceive(state.Buffer, 0, state.Buffer.Length,
					SocketFlags.None, new AsyncCallback(this.RecieveCallback), state);
				state.Signal.WaitOne();
			}
			catch
			{
				context = null;
				return false;
			}

			string requestContent = Encoding.ASCII.GetString(state.Buffer);
			int headerSize = requestContent.IndexOf("\r\n\r\n");

			if (headerSize != -1)
			{
				headerSize += 4;

				int indexOf = requestContent.IndexOf("\r\n");
				string line = requestContent.Substring(0, indexOf);
				requestContent = requestContent.Substring(indexOf + 2);
				string requestUri = "/";
				string[] methodParts = line.Split(' ');

				//First line must be "<METHOD> <URI> HTTP/<VERSION>" which translates to 3 elements
				//when split by the space char.
				if (methodParts.Length == 3)
				{
					//Get down to business.
					switch (methodParts[0])
					{
						//WS: Normal HTTP methods:
						case "HEAD":
						case "GET":
						case "POST":
						case "PUT":
						case "DELETE":
						case "TRACE":
						case "OPTIONS":
						case "CONNECT":
						//WS: WebDAV extension methods:
						case "PROPFIND":
						case "PROPPATCH":
						case "MKCOL":
						case "COPY":
						case "MOVE":
						case "LOCK":
						case "UNLOCK":
							context.Request.Method = methodParts[0];
							break;

						default:
							//WS: We need to generate an error here if the method is not supported.
							ErrorHandler.Handle(context, StatusCode.Http405MethodNotAllowed, methodParts[0]);
							return true;
					}
					//Request URI is the "middle"
					requestUri = methodParts[1];

					switch (methodParts[2])
					{
						case "HTTP/0.9":
							context.ProtocolVersion = new Version(0, 9);
							break;
						case "HTTP/1.0":
							context.ProtocolVersion = new Version(1, 0);
							break;
						case "HTTP/1.1":
							context.ProtocolVersion = new Version(1, 1);
							break;

						default:
							ErrorHandler.Handle(context, StatusCode.Http400BadRequest, "An invalid HTTP version was detected");
							return true;
					}
				}
				else
				{
					ErrorHandler.Handle(context, StatusCode.Http400BadRequest, "The first line of the request was invalid");
					return true;
				}
				indexOf = requestContent.IndexOf("\r\n");
				while (indexOf != -1)
				{
					line = requestContent.Substring(0, indexOf);
					requestContent = requestContent.Substring(indexOf + 2);
					if (line.Length > 0)
					{
						int n = line.IndexOf(':');
						if (n != -1)
						{
							context.Request.Headers.Add(line.Substring(0, n), line.Substring(n + 2));
						}
					}
					indexOf = requestContent.IndexOf("\r\n");
				}
				//HTTP 1.1 states that requests must define a "Host" header even if an absolute
				//request URI is requested.
				if (context.Request.Headers.Contains("Host"))
				{
					//HTTP 1.1 and later allows a relative URI or an absolute URI to be requested.
					if (requestUri.StartsWith("/"))
					{
						//relative requesturi
						context.Request.Url = new Uri("http://"
							+ context.Request.Headers["Host"].PrimaryValue + requestUri);
					}
					else if (requestUri.StartsWith("http://") || requestUri.StartsWith("https://"))
					{
						//absolute requesturi
						context.Request.Url = new Uri(requestUri);
					}
					else
					{
						//invalid url scheme for HTTP.
						ErrorHandler.Handle(context, StatusCode.Http400BadRequest, "Invalid request URI scheme");
						return true;
					}
				}
				else
				{
					//Request is invalid because it doesnt have a Host header.
					ErrorHandler.Handle(context, StatusCode.Http400BadRequest, "No Host header included");
					return true;
				}

				bool hasContentLength = context.Request.Headers.Contains("Content-Length");
				bool hasTransferEncoding = context.Request.Headers.Contains("Transfer-Encoding");
				bool hasBody = hasContentLength | hasTransferEncoding;

				if (hasBody)
				{
					//Check if the client sent the Content-Type header, which is required if
					//a message body is included with the request. 
					if (context.Request.Headers.Contains("Content-Type"))
					{
						//Content-Length and Transfer-Encoding headers can't coexist.
						if (hasContentLength && !hasTransferEncoding)
						{
							ErrorHandler.Handle(context, StatusCode.Http501NotImplemented);
							return true;
						}
						else if (hasTransferEncoding && !hasContentLength)
						{
							ErrorHandler.Handle(context, StatusCode.Http501NotImplemented);
							return true;
						}
						else
						{
							ErrorHandler.Handle(context, StatusCode.Http400BadRequest, "Content-Length and Transfer-Encoding headers cannot exist in the same request.");
							return true;
						}
					}
					else
					{
						ErrorHandler.Handle(context, StatusCode.Http400BadRequest, "No Content-Type header included with request that includes a message body.");
					}
				}
				return true;
			}
			return false;
		}
		#endregion
	}
}