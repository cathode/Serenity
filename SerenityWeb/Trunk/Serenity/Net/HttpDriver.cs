﻿/******************************************************************************
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

using Serenity.Web;

namespace Serenity.Net
{
    /// <summary>
    /// Provides a <see cref="Serenity.Net.ProtocolDriver"/> implementation that provides support for the HTTP protocol.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class HttpDriver : ProtocolDriver
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the <see cref="Serenity.Net.HttpDriver"/> class.
        /// </summary>
        public HttpDriver()
        {
        }
        #endregion
        #region Methods - Private
        //private void ContentTypeHeaderCallback(Header header)
        //{
        //    if (header.PrimaryValue.Contains(";"))
        //    {
        //        string[] segments = header.PrimaryValue.Split(new char[] { ';' }, 2);
        //        header.PrimaryValue = segments[0];
        //    }
        //    Request.ContentType = MimeType.FromString(header.PrimaryValue);
        //}
        private bool ProcessUrlEncodedRequestData(Request request, string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            string[] pairs = input.Split('&');
            foreach (string escapedPair in pairs)
            {
                string pair = Uri.UnescapeDataString(escapedPair);
                if (pair.IndexOf('=') != -1)
                {
                    string name = pair.Substring(0, pair.IndexOf('='));
                    string value = pair.Substring(pair.IndexOf('=') + 1);
                    request.RequestData.AddDataStream(name, Encoding.UTF8.GetBytes(value)).Method = RequestMethod.GET;
                }
            }
            return true;
        }
        private bool ProcessMultipartFormDataContent(Request request, string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            else if (!request.Headers.Contains("Content-Type"))
            {
                //throw new InvalidOperationException("
            }

            Header ct = request.Headers["Content-Type"];

            return true;
        }
        #endregion
        #region Methods - Public
        public override Request RecieveRequest(Socket socket)
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
            var request = new Request();

            byte[] buffer;
            List<byte> listBuffer = new List<byte>();
            while (socket.Available > 0)
            {
                buffer = new byte[socket.Available];
                socket.Receive(buffer);
                listBuffer.AddRange(buffer);
            }
            buffer = listBuffer.ToArray();
            string requestContent = Encoding.ASCII.GetString(buffer);
            request.RawRequest = requestContent;
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
                        request.Method = (RequestMethod)Enum.Parse(typeof(RequestMethod), methodParts[0]);
                    }
                    catch
                    {
                        ErrorHandler.Handle(StatusCode.Http405MethodNotAllowed, methodParts[0]);
                        return request;
                    }

                    //Request URI is the "middle"
                    requestUriRaw = methodParts[1];

                    switch (methodParts[2])
                    {
                        case "HTTP/0.9":
                            request.ProtocolVersion = new Version(0, 9);
                            break;
                        case "HTTP/1.0":
                            request.ProtocolVersion = new Version(1, 0);
                            break;
                        case "HTTP/1.1":
                            request.ProtocolVersion = new Version(1, 1);
                            break;

                        default:
                            ErrorHandler.Handle(StatusCode.Http505HttpVersionNotSupported,
                                "The HTTP version specified with the request is not supported.");
                            return request;
                    }
                }
                else
                {
                    ErrorHandler.Handle(StatusCode.Http400BadRequest, "The first line of the request was invalid");
                    return request;
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
                                ErrorHandler.Handle(StatusCode.Http400BadRequest, "A header was specified with no value.");
                                return request;
                            }
                            if (!request.Headers.Contains(name))
                            {
                                //TODO: Make sure ignoring duplicated headers is acceptable.
                                request.Headers.Add(name, value);
                            }
                        }
                    }
                    indexOf = requestContent.IndexOf("\r\n");
                }
                //HTTP 1.1 states that requests must define a "Host" header even if an absolute
                //request URI is requested.
                if (request.Headers.Contains("Host"))
                {
                    //HTTP 1.1 and later allows a relative URI or an absolute URI to be requested.
                    if (requestUriRaw.StartsWith("/"))
                    {
                        //relative requesturi
                        request.Url = new Uri("http://"
                            + request.Headers["Host"].PrimaryValue + requestUriRaw);
                    }
                    else if (requestUriRaw.StartsWith("http://") || requestUriRaw.StartsWith("https://"))
                    {
                        //absolute requesturi
                        request.Url = new Uri(requestUriRaw);
                    }
                    else
                    {
                        //invalid url scheme for HTTP.
                        ErrorHandler.Handle(StatusCode.Http400BadRequest, "Invalid request URI scheme");
                        return request;
                    }

                    Domain.Current = SerenityServer.Domains.GetBestMatch(request.Headers["Host"].PrimaryValue);

                    if (request.Url.Query.Length > 0)
                    {
                        this.ProcessUrlEncodedRequestData(request, request.Url.Query.TrimStart('?'));
                    }
                }
                else
                {
                    //Request is invalid because it doesnt have a Host header.
                    ErrorHandler.Handle(StatusCode.Http400BadRequest, "No Host header included");
                    return request;
                }

                //foreach (Header header in Request.Headers)
                //{
                //    this.HandleHeader(header);
                //}

                bool hasContentLength = request.Headers.Contains("Content-Length");
                bool hasTransferEncoding = request.Headers.Contains("Transfer-Encoding");
                bool hasBody = hasContentLength | hasTransferEncoding;

                if (hasBody)
                {
                    //Check if the client sent the Content-Type header, which is required if
                    //a message body is included with the request. 
                    if (request.Headers.Contains("Content-Type"))
                    {
                        //Content-Length and Transfer-Encoding headers can't coexist.
                        if (hasContentLength && hasTransferEncoding)
                        {
                            ErrorHandler.Handle(StatusCode.Http400BadRequest,
                                "Content-Length and Transfer-Encoding headers cannot exist in the same request.");
                        }
                        else
                        {
                            if (request.ContentType == MimeType.ApplicationXWwwFormUrlEncoded)
                            {
                                if (!this.ProcessUrlEncodedRequestData(request, requestContent))
                                {
                                    return request;
                                }
                            }
                            else if (request.ContentType == MimeType.MultipartFormData)
                            {
                                if (!this.ProcessMultipartFormDataContent(request, requestContent))
                                {
                                    return request;
                                }
                            }
                            else
                            {
                                ErrorHandler.Handle(StatusCode.Http501NotImplemented,
                                    "No handler for the specified Content-Type exists.");
                            }
                        }
                    }
                    else
                    {
                        ErrorHandler.Handle(StatusCode.Http400BadRequest, "No Content-Type header included with request that includes a message body.");
                    }
                }
                return request;
            }
            return null;
        }
        public override bool SendResponse(Socket socket, Response response)
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
            else if (socket == null)
            {
                throw new ArgumentNullException("socket");
            }
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
                    response.Sent += sent;
                }
                response.ClearOutputBuffer();
            }

            return true;
        }
        #endregion

        protected override ushort DefaultListeningPort
        {
            get
            {
                return 80;
            }
        }

        protected override string DefaultProviderName
        {
            get
            {
                return "SerenityProject.net";
            }
        }

        protected override string DefaultSchemaName
        {
            get
            {
                return "http";
            }
        }

        protected override string DefaultDescription
        {
            get
            {
                return "Protocol driver for the Hyper Text Transfer Protocol";
            }
        }

        protected override Version DefaultVersion
        {
            get
            {
                return new Version(1, 1, 0, 0);
            }
        }
    }
}