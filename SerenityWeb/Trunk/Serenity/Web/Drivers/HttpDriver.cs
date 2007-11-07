/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
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
    public sealed class HttpDriver : WebDriver
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the HttpDriver class.
        /// </summary>
        /// <param name="settings">The WebDriverSettings which control the
        /// behaviour of the new WebDriver instance.</param>
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
                    context.Request.RequestData.AddDataStream(Name, Encoding.UTF8.GetBytes(Value)).Origin = RequestDataOrigin.Get;
                }
            }
        }
        #endregion
        #region Methods - Public
        public override CommonContext RecieveContext(Socket socket)
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
            else if (socket == null)
            {
                throw new ArgumentNullException("socket");
            }

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
                    return null;
                }
            }
            byte[] buffer;
            List<byte> listBuffer = new List<byte>();
            while (socket.Available > 0)
            {
                buffer = new byte[socket.Available];
                socket.Receive(buffer);
                listBuffer.AddRange(buffer);
            }
            buffer = listBuffer.ToArray();

            CommonContext context = new CommonContext(this);


            string requestContent = Encoding.ASCII.GetString(buffer);
            int headerSize = requestContent.IndexOf("\r\n\r\n");

            if (headerSize != -1)
            {
                headerSize += 4;

                int indexOf = requestContent.IndexOf("\r\n");
                string line = requestContent.Substring(0, indexOf);
                requestContent = requestContent.Substring(indexOf + 2);
                string requestUriRaw = "/";
                string[] methodParts = line.Split(' ');

                //First line must be "<METHOD> <URI> HTTP/<VERSION>" which
                //translates to 3 elements when split by the space char.
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
                            return context;
                    }
                    //Request URI is the "middle"
                    requestUriRaw = methodParts[1];

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
                            return context;
                    }
                }
                else
                {
                    ErrorHandler.Handle(context, StatusCode.Http400BadRequest, "The first line of the request was invalid");
                    return context;
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
                    if (requestUriRaw.StartsWith("/"))
                    {
                        //relative requesturi
                        context.Request.Url = new Uri("http://"
                            + context.Request.Headers["Host"].PrimaryValue + requestUriRaw);
                    }
                    else if (requestUriRaw.StartsWith("http://") || requestUriRaw.StartsWith("https://"))
                    {
                        //absolute requesturi
                        context.Request.Url = new Uri(requestUriRaw);
                    }
                    else
                    {
                        //invalid url scheme for HTTP.
                        ErrorHandler.Handle(context, StatusCode.Http400BadRequest, "Invalid request URI scheme");
                        return context;
                    }
                    if (context.Request.Url.Query != string.Empty)
                    {
                        this.ProcessUrlEncodedRequestData(context.Request.Url.Query.TrimStart('?'), context);
                    }
                }
                else
                {
                    //Request is invalid because it doesnt have a Host header.
                    ErrorHandler.Handle(context, StatusCode.Http400BadRequest, "No Host header included");
                    return context;
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
                            Console.WriteLine(line);

                            if (context.Request.ContentType == MimeType.ApplicationXWwwFormUrlEncoded)
                            {

                            }
                        }
                        else if (hasTransferEncoding && !hasContentLength)
                        {
                            ErrorHandler.Handle(context, StatusCode.Http501NotImplemented);
                        }
                        else
                        {
                            ErrorHandler.Handle(context, StatusCode.Http400BadRequest, "Content-Length and Transfer-Encoding headers cannot exist in the same request.");
                        }
                    }
                    else
                    {
                        ErrorHandler.Handle(context, StatusCode.Http400BadRequest, "No Content-Type header included with request that includes a message body.");
                    }
                }
                return context;
            }
            return null;
        }
        public override bool SendContext(Socket socket, CommonContext context)
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
            else if (socket == null)
            {
                throw new ArgumentNullException("socket");
            }
            else if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            CommonRequest request = context.Request;
            CommonResponse response = context.Response;
            if (!response.HeadersSent)
            {
                StringBuilder outputText = new StringBuilder();

                if (response.Headers.Contains("Content-Length") == false)
                {
                    response.Headers.Add("Content-Length", response.OutputBuffer.Count.ToString());
                }
                if (response.Headers.Contains("Content-Type") == false)
                {
                    response.Headers.Add("Content-Type", response.MimeType.ToString() + "; charset=UTF-8");
                }
                if (!response.Headers.Contains("Server"))
                {
                    response.Headers.Add(new Header("Server", SerenityInfo.Name + "/" + SerenityInfo.Version));
                }

                outputText.Append("HTTP/1.1 " + response.Status.ToString() + "\r\n");
                foreach (Header header in response.Headers)
                {
                    string value;
                    if (header.Complex == true)
                    {
                        //TODO: Get rid of this worthless switch or make it useful.
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
                socket.Send(output);
            }
            if (response.OutputBuffer.Count > 0)
            {
                byte[] buffer = response.OutputBuffer.ToArray();
                int sent = socket.Send(buffer);
                while (sent < buffer.Length)
                {
                    byte[] newBuffer = new byte[buffer.Length - sent];
                    buffer.CopyTo(newBuffer, sent);
                    buffer = newBuffer;
                    sent = socket.Send(buffer);
                    context.Response.Sent += sent;
                }
                response.OutputBuffer.Clear();
            }

            return true;
        }
        #endregion
        #region Properties - Public
        public override bool CanRecieveAsync
        {
            get
            {
                return false;
            }
        }
        public override bool CanSendAsync
        {
            get
            {
                return false;
            }
        }
        #endregion
    }
}