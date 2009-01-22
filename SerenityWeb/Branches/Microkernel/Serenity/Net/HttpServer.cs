/* Serenity - The next evolution of web server technology.
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/
 * 
 * This software is released under the terms and conditions of the Microsoft
 * Public License (Ms-PL), a copy of which should have been included with
 * this distribution as License.txt.
 *
 * Contributors:
 * Will 'AnarkiNet' Shelley (AnarkiNet@gmail.com)
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Serenity.Web;

namespace Serenity.Net
{
    /// <summary>
    /// Provides <see cref="Server"/> implementation that communicates requests
    /// and responses using the HTTP protocol version 1.1.
    /// </summary>
    public class HttpServer : Server
    {
        #region Fields
        private static readonly int MinimumRequestLength = "GET / HTTP1.1/\r\nHost:\r\n\r\n".Length;
        #endregion
        #region Methods
        /// <summary>
        /// Overridden. Creates a <see cref="ServerAsyncState"/> instance to
        /// enable state preservation between asynchronous method calls.
        /// </summary>
        /// <returns></returns>
        protected override ServerAsyncState CreateStateObject()
        {
            return new HttpServerAsyncState()
            {
                Owner = this
            };
        }
        protected override bool ValidateRequest(Request request, Response response)
        {
            //TODO: Perform validation of request.

            // Temporarily bypass validation.
            return true;
        }
        protected override int ParseRequest(byte[] buffer, int start, Request request)
        {
            if (buffer.Length - start < HttpServer.MinimumRequestLength)
            {
                return -1;
            }
            int n = buffer.Length - start;
            string sbuffer = Encoding.ASCII.GetString(buffer, start, n);

            int headerEnd = sbuffer.IndexOf("\r\n\r\n");
            string rawHeaders = sbuffer.Substring(0, headerEnd);

            string methodLine = rawHeaders.Substring(0, rawHeaders.IndexOf("\r\n"));


            foreach (string headerLine in rawHeaders.Substring(rawHeaders.IndexOf("\r\n") + 2).Split(new string[] { "\r\n" }, StringSplitOptions.None))
            {
                string headerName = headerLine.Substring(0, headerLine.IndexOf(':'));
                string headerValue = headerLine.Substring(headerLine.IndexOf(':') + 1);

                request.Headers.Add(headerName, headerValue.Trim());
            }
            return rawHeaders.Length + "\r\n\r\n".Length;
        }
        protected override void ProcessRequest(Request request, Response response)
        {
            this.RootResource.OnRequest(request, response);
        }
        protected override void ReceiveTimeoutCallback(object state)
        {
            var serverState = (HttpServerAsyncState)state;
            byte[] data = Encoding.ASCII.GetBytes(@"HTTP/1.1 408 Request Timeout");
            serverState.Client.Send(data);
            //TODO: Implement async sending.
            //serverState.Client.BeginSend(data, SocketFlags.None, new AsyncCallback(this.SendCallback), state);
            base.ReceiveTimeoutCallback(state);
        }
        #endregion
    }
}
