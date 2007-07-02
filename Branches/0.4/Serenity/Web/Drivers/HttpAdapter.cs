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
using System.IO;
using System.IO.Compression;
using System.Net.Sockets;
using System.Text;

namespace Serenity.Web.Drivers
{

    /// <summary>
    /// Provides a WebAdapter used to translate CommonContexts to and from an HTTP-based WebDriver.
    /// </summary>
    public sealed class HttpAdapter : WebAdapter
    {
        /// <summary>
        /// Initializes a new instance of the HttpAdapter class.
        /// </summary>
        public HttpAdapter()
        {
        }
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
        private void RecieveCallback(IAsyncResult result)
        {

        }
        private void SendCallback(IAsyncResult result)
        {
            try
            {
                Socket socket = (Socket)result.AsyncState;
                socket.EndSend(result);
            }
            catch
            {

            }
        }
        #endregion
        #region Methods - Protected
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
        /// <summary>
        /// Reads data from the supplied socket until a complete CommonContext object has been
        /// populated from the read data.
        /// </summary>
        /// <param name="socket">The socket to read from.</param>
        /// <param name="context">When this method returns, context will contain the created CommonContext.</param>
        /// <returns>True if successful, or false if any error occurred.</returns>
        public override bool ReadContext(Socket socket, out CommonContext context)
        {
            context = new CommonContext(this);

            WebAdapterState state = new WebAdapterState();
            state.Buffer = new byte[socket.Available];
            state.WorkSocket = socket;

            socket.BeginReceive(state.Buffer, 0, state.Buffer.Length,
                SocketFlags.None, new AsyncCallback(this.RecieveCallback), state);

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
                            ErrorHandler.Handle(StatusCode.Http405MethodNotAllowed, context);
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

                        default:
                            //WS: This should probably be changed to send an error to the client.
                            context.ProtocolVersion = new Version(1, 1);
                            break;
                    }
                }
                else
                {
                    //WS: A bad request error page needs to be generated and sent at this point.
                    ErrorHandler.Handle(StatusCode.Http400BadRequest, context);
                    return true;
                }
                int contentLength = 0;
                string multipartBoundary = "";

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
                            string headerName = line.Substring(0, n);
                            string headerValue = line.Substring(n + 2);
                            Header header = new Header(headerName, headerValue);

                            switch (headerName)
                            {
                                case "Content-Length":
                                    contentLength = int.Parse(headerValue);
                                    break;
                                case "Content-Type":
                                    if (headerValue.StartsWith("multipart/form-data") == true)
                                    {
                                        context.Request.ContentType = MimeType.MultipartFormData;
                                        multipartBoundary = headerValue.Substring(headerValue.IndexOf('=') + 1);
                                    }
                                    else
                                    {
                                        context.Request.ContentType = MimeType.ApplicationXWwwFormUrlEncoded;
                                    }
                                    header = new Header(headerName, headerValue);
                                    break;
                                
                                case "Accept":
                                case "Accept-Charset":
                                //AJ: Removed for http compression support
                                //case "Accept-Encoding":
                                case "Accept-Language":
                                case "Keep-Alive":
                                    header = null;
                                    break;
                            }
                            if (header != null)
                            {
                                context.Request.Headers.Add(header);
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
                        ErrorHandler.Handle(StatusCode.Http400BadRequest, context);
                        return true;
                    }
                }
                else
                {
                    //Request is invalid because it doesnt have a Host header.
                    ErrorHandler.Handle(StatusCode.Http400BadRequest, context);
                    return true;
                }

                if (context.Request.Headers.Contains("Content-Length")
                   || context.Request.Headers.Contains("Transfer-Encoding"))
                {
                    if (context.Request.Headers.Contains("Content-Length")
                        && !context.Request.Headers.Contains("Transfer-Encoding"))
                    {
                        contentLength = int.Parse(context.Request.Headers["Content-Length"].PrimaryValue);
                    }
                    else if (!context.Request.Headers.Contains("Content-Length")
                        && context.Request.Headers.Contains("Transfer-Encoding")
                        && context.Request.Headers["Transfer-Encoding"].PrimaryValue == "chunked")
                    {
                        ErrorHandler.Handle(StatusCode.Http501NotImplemented, context);
                        return true;
                    }
                    else
                    {
                        ErrorHandler.Handle(StatusCode.Http400BadRequest, context);
                        return true;
                    }
                }
            }
            return true;
        }
        #endregion
    }
}