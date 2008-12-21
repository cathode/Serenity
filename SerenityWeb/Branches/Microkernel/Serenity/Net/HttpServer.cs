/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *----------------------------------------------------------------------------*
 * Authors:                                                                   *
 * - Will 'AnarkiNet' Shelley (AnarkiNet@gmail.com): Original Author          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Serenity.Web;
using Serenity.Logging;

namespace Serenity.Net
{
    /// <summary>
    /// Provides <see cref="Server"/> implementation that communicates requests
    /// and responses using the HTTP protocol version 1.1.
    /// </summary>
    public class HttpServer : Server
    {
        #region Methods
        protected override ServerAsyncState CreateStateObject()
        {
            return new HttpServerAsyncState()
            {
                Owner = this
            };
        }
        protected override void ReceiveCallback(IAsyncResult result)
        {
            base.ReceiveCallback(result);

            var state = (HttpServerAsyncState)result.AsyncState;
            Request request = state.Request;
            Response response = state.Response;
            bool completed = false;
            string sbuffer;

            lock (state.DataBuffer)
            {
                sbuffer = Encoding.ASCII.GetString(state.DataBuffer.ToArray());

                int bufferSize = sbuffer.Length;
                while (true)
                {
                    if (state.Stage == RequestProcessingStage.None)
                    {
                        int i = sbuffer.IndexOf(' ');
                        if (i > -1)
                        {
                            string method = sbuffer.Substring(0, i);
                            sbuffer = sbuffer.Substring(i + 1);

                            switch (method)
                            {
                                case "CONNECT":
                                case "COPY": //WebDAV method
                                case "DELETE": //WebDAV method
                                case "GET":
                                case "HEAD":
                                case "LOCK": //WebDAV method
                                case "MKCOL": //WebDAV method
                                case "MOVE": //WebDAV method
                                case "OPTIONS":
                                case "POST":
                                case "PROPFIND": //WebDAV method
                                case "PROPPATCH": //WebDAV method
                                case "PUT":
                                case "TRACE":
                                case "UNLOCK": //WebDAV method
                                    request.Method = (RequestMethod)Enum.Parse(typeof(RequestMethod), method);
                                    break;

                                default:
                                    request.Method = RequestMethod.Unknown;
                                    break;
                            }
                            request.RawMethod = method;
                            state.Stage = RequestProcessingStage.MethodProcessed;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (state.Stage == RequestProcessingStage.MethodProcessed)
                    {
                        int i = sbuffer.IndexOf(' ');
                        if (i > -1)
                        {
                            string uri = sbuffer.Substring(0, i);
                            sbuffer = sbuffer.Substring(i + 1);
                            request.RawUrl = uri;
                            request.Url = new Uri(uri, UriKind.RelativeOrAbsolute);

                            state.Stage = RequestProcessingStage.UriProcessed;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (state.Stage == RequestProcessingStage.UriProcessed)
                    {
                        int i = sbuffer.IndexOf("\r\n");

                        if (i > -1)
                        {
                            string version = sbuffer.Substring(0, i);
                            sbuffer = sbuffer.Substring(i + 2);

                            if (version == "HTTP/1.1")
                            {
                                request.ProtocolVersion = new Version(1, 1);
                            }
                            else
                            {
                                request.ProtocolVersion = new Version(0, 0);
                            }

                            state.Stage = RequestProcessingStage.VersionProcessed;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (state.Stage == RequestProcessingStage.VersionProcessed || state.Stage == RequestProcessingStage.HeaderProcessed)
                    {
                        int i = sbuffer.IndexOf("\r\n");

                        if (i == 0)
                        {
                            sbuffer = sbuffer.Substring(i + 2);
                            state.Stage = RequestProcessingStage.AllHeadersProcessed;
                        }
                        else if (i > -1)
                        {
                            int n = sbuffer.IndexOf(':');

                            if (n > -1)
                            {
                                string headerName = sbuffer.Substring(0, n);
                                string headerValue = sbuffer.Substring(n + 2, i - (n + 2));

                                Header h = request.Headers.Add(headerName, headerValue);

                                switch (h.Name)
                                {
                                    case "Host":
                                        if (request.RawUrl.StartsWith("/"))
                                        {
                                            //relative requesturi
                                            request.Url = new Uri("http://"
                                                + h.PrimaryValue + request.RawUrl);
                                        }
                                        else if (request.RawUrl.StartsWith("http://") || request.RawUrl.StartsWith("https://"))
                                        {
                                            //absolute requesturi
                                            request.Url = new Uri(request.RawUrl);
                                        }
                                        break;

                                    case "Content-Length":
                                        int cl;
                                        if (int.TryParse(h.PrimaryValue, out cl))
                                        {
                                            request.ContentLength = cl;
                                        }
                                        break;
                                }

                                state.Stage = RequestProcessingStage.HeaderProcessed;
                            }
                            sbuffer = sbuffer.Substring(i + 2);
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (state.Stage == RequestProcessingStage.AllHeadersProcessed)
                    {
                        if (sbuffer.Length >= request.ContentLength)
                        {
                            if (request.ContentLength > 0)
                            {
                                //TODO: Further process the entity data of the request into individual named data streams if appropriate.
                                request.RequestData.AddDataStream("", Encoding.ASCII.GetBytes(sbuffer.Substring(0, request.ContentLength)));

                                sbuffer = sbuffer.Substring(request.ContentLength);
                            }
                            state.Stage = RequestProcessingStage.ProcessingComplete;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (state.Stage == RequestProcessingStage.ProcessingComplete)
                    {
                        completed = true;
                        //pc.Reset();
                        break;
                    }
                }
                if (sbuffer.Length < bufferSize)
                {
                    int n = bufferSize - sbuffer.Length;

                    if (n == state.DataBuffer.Count)
                    {
                        state.DataBuffer.Clear();
                    }
                    else
                    {
                        for (int i = 0; i < n; i++)
                        {
                            state.DataBuffer.Dequeue();
                        }
                    }
                }
                if (completed)
                {
                    this.RootResource.OnRequest(request, response);

                    if (this.IsDisposed)
                    {
                        throw new ObjectDisposedException(this.GetType().FullName);
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

                        if (response.Cookies.Count > 0)
                        {
                            response.Headers.Add("Set-Cookie", string.Join(",", (from c in response.Cookies
                                                                                 where !string.IsNullOrEmpty(c.Name)
                                                                                 select c.ToString()).ToArray()));
                        }

                        outputText.Append("HTTP/1.1 " + response.Status.ToString() + "\r\n");
                        foreach (Header header in response.Headers)
                        {
                            string value;
                            if (header.Complex == true)
                            {
                                value = string.Format("{0},{1}", header.PrimaryValue, string.Join(",", header.SecondaryValues)).TrimEnd('\r', '\n');
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
                            response.Connection.Send(output);
                        }
                        catch
                        {

                        }
                    }
                    if (response.OutputBuffer.Count > 0)
                    {
                        byte[] buffer = response.OutputBuffer.ToArray();
                        int sent = 0;
                        while (sent < buffer.Length)
                        {
                            byte[] newBuffer = new byte[buffer.Length - sent];
                            buffer.CopyTo(newBuffer, sent);
                            buffer = newBuffer;
                            try
                            {
                                sent = response.Connection.Send(buffer);
                            }
                            catch (SocketException ex)
                            {
                                this.Log.RecordEvent(ex.Message, EventKind.Notice, ex.StackTrace);
                                break;
                            }
                            response.Sent += sent;
                        }
                        response.ClearOutputBuffer();
                    }
                }
            }
        }
        #endregion
    }
}
