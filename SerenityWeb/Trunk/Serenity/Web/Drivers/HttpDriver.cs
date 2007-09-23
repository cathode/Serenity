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
	public class HttpDriver : WebDriver
	{
		#region Constructors - Public
		/// <summary>
		/// Initializes a new instance of the HttpDriver class.
		/// </summary>
		/// <param name="contextHandler">The context handler to use for the operation of the new HttpDriver.</param>
		public HttpDriver(WebDriverSettings settings)
			: base(settings)
		{
			this.Info = new DriverInfo("Serenity", "HyperText Transmission Protocol", "http", new Version(1, 1));
		}
		#endregion
		#region Methods - Private
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
		#endregion
		#region Methods - Protected
		protected override void RecieveCallback(IAsyncResult ar)
		{
			if (ar.AsyncState.GetType().TypeHandle.Equals(typeof(WebDriverState).TypeHandle))
			{
                WebDriverState state = (WebDriverState)ar.AsyncState;
                Socket socket = state.WorkSocket;
                socket.EndReceive(ar);

                int available = socket.Available;
                if (available > 0)
                {
                    WebDriverState newState = new WebDriverState(state.Buffer.Length + socket.Available);
                    state.Buffer.CopyTo(newState.Buffer, 0);
                    newState.Signal = state.Signal;
                    newState.WorkSocket = state.WorkSocket;
                    socket.BeginReceive(newState.Buffer, state.Buffer.Length, available,
                        SocketFlags.None, new AsyncCallback(this.RecieveCallback), newState);
                }
                else
                {
                    state.Signal.Set();
                }
			}
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
					response.Headers.Add("Content-Length", response.OutputBuffer.Length.ToString());
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

				if (this.Settings.Block)
				{
					socket.Send(output);
				}
				else
				{
					socket.BeginSend(output, 0, output.Length, SocketFlags.None, new AsyncCallback(this.SendCallback), socket);
				}
				return true;
			}
			else
			{
				return false;
			}
		}
		#endregion
		#region Methods - Public
		public override bool ReadContext(Socket socket, out CommonContext context)
		{
			context = new CommonContext(this);
			byte[] buffer = new byte[socket.Available];

			if (this.Settings.Block)
			{
                if (socket.Available == 0)
                {
                    int waits = 0;
                    while (socket.Available == 0 && waits < 100)
                    {
                        Thread.Sleep(1);
                        waits++;
                    }
                    if (socket.Available == 0)
                    {
                        context = null;
                        return false;
                    }
                }

				List<byte> listBuffer = new List<byte>();
				while (socket.Available > 0)
				{
					buffer = new byte[socket.Available];
					socket.Receive(buffer);
					listBuffer.AddRange(buffer);
				}
				buffer = listBuffer.ToArray();

                HttpReader reader = new HttpReader(this);
                bool result;
                context = reader.Read(buffer, out result);
                
                if (result)
                {
                    return true;
                }
                else
                {
                    context = null;
                    return false;
                }
			}
			else
			{
                context = new CommonContext(this);

				WebDriverState state = new WebDriverState();
				state.Buffer = buffer;
				state.WorkSocket = socket;

                state.Signal.Reset();

				socket.BeginReceive(state.Buffer, 0, state.Buffer.Length,
					SocketFlags.None, new AsyncCallback(this.RecieveCallback), state);

                state.Signal.WaitOne();
                bool result = false;
                context = new HttpReader(this).Read(state.Buffer, out result);

                if (result)
                {
                    return true;
                }
                else
                {
                    context = null;
                    return false;
                }
			}
			
		}
		#endregion
	}
}