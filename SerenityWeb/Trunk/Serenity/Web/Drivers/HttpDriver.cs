/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
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
            this.RegisterHeaderHandler("Content-Type", new HeaderHandlerCallback(this.ContentTypeHeaderCallback));
        }
        #endregion
        #region Methods - Private
        private void ContentTypeHeaderCallback(CommonContext context, Header header)
        {
            if (header.PrimaryValue.Contains(";"))
            {
                string[] segments = header.PrimaryValue.Split(new char[] { ';' }, 2);
                header.PrimaryValue = segments[0];
            }
            context.Request.ContentType = MimeType.FromString(header.PrimaryValue);
        }
        private bool ProcessUrlEncodedRequestData(string input, CommonContext context)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            else if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            string[] pairs = input.Split('&');
            foreach (string escapedPair in pairs)
            {
                string pair = Uri.UnescapeDataString(escapedPair);
                if (pair.IndexOf('=') != -1)
                {
                    string name = pair.Substring(0, pair.IndexOf('='));
                    string value = pair.Substring(pair.IndexOf('=') + 1);
                    context.Request.RequestData.AddDataStream(name, Encoding.UTF8.GetBytes(value)).Method = RequestMethod.GET;
                }
            }
            return true;
        }
        private bool ProcessMultipartFormDataContent(string input, CommonContext context)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            else if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            else if (!context.Request.Headers.Contains("Content-Type"))
            {
                //throw new InvalidOperationException("
            }

            Header ct = context.Request.Headers["Content-Type"];

            return true;
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
            context.Request.RawRequest = requestContent;
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
                    try
                    {
                        context.Request.Method = (RequestMethod)Enum.Parse(typeof(RequestMethod), methodParts[0]);
                    }
                    catch
                    {
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
                            ErrorHandler.Handle(context, StatusCode.Http505HttpVersionNotSupported,
                                "The HTTP version specified with the request is not supported.");
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
                            string name = line.Substring(0, n);
                            string value = line.Substring(n + 2);
                            if (value.Length == 0)
                            {
                                ErrorHandler.Handle(context, StatusCode.Http400BadRequest, "A header was specified with no value.");
                                return context;
                            }
                            if (!context.Request.Headers.Contains(name))
                            {
                                //TODO: Make sure ignoring duplicated headers is acceptable.
                                context.Request.Headers.Add(name, value);
                            }
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

                    context.Domain = SerenityServer.Domains.GetBestMatch(context.Request.Headers["Host"].PrimaryValue);

                    if (context.Request.Url.Query.Length > 0)
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

                foreach (Header header in context.Request.Headers)
                {
                    this.HandleHeader(context, header);
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
                        if (hasContentLength && hasTransferEncoding)
                        {
                            ErrorHandler.Handle(context, StatusCode.Http400BadRequest,
                                "Content-Length and Transfer-Encoding headers cannot exist in the same request.");
                        }
                        else
                        {
                            if (context.Request.ContentType == MimeType.ApplicationXWwwFormUrlEncoded)
                            {
                                if (!this.ProcessUrlEncodedRequestData(requestContent, context))
                                {
                                    return context;
                                }
                            }
                            else if (context.Request.ContentType == MimeType.MultipartFormData)
                            {
                                if (!this.ProcessMultipartFormDataContent(requestContent, context))
                                {
                                    return context;
                                }
                            }
                            else
                            {
                                ErrorHandler.Handle(context, StatusCode.Http501NotImplemented,
                                    "No handler for the specified Content-Type exists.");
                            }
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

                if (!response.Headers.Contains("Content-Length"))
                {
                    response.Headers.Add("Content-Length", response.OutputBuffer.Count.ToString());
                }
                if (!response.Headers.Contains("Content-Type"))
                {
                    response.Headers.Add("Content-Type", response.ContentType.ToString() + "; charset=UTF-8");
                }
                else if (response.Headers["Content-Type"].PrimaryValue != response.ContentType.ToString())
                {
                    response.Headers["Content-Type"].PrimaryValue = response.ContentType.ToString();
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
                try
                {
                    socket.Send(output);
                }
                catch
                {
                    return false;
                }
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