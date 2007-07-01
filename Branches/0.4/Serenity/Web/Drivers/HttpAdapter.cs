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
                string requestPath = "/";
                string hostName = "localhost";
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
                            context.Request.Method = "GET";
                            break;
                    }

                    if (methodParts[1].Contains("?") == true)
                    {
                        requestPath = methodParts[1].Substring(0, methodParts[1].IndexOf('?'));

                        string[] pairs = methodParts[1].Substring(methodParts[1].IndexOf('?') + 1).Split('&');
                        foreach (string pair in pairs)
                        {
                            int i = pair.IndexOf('=');
                            if (i != -1)
                            {
                                string name = pair.Substring(0, i);
                                string value = pair.Substring(i + 1);
                                context.Request.RequestData.AddDataStream(name, Encoding.UTF8.GetBytes(value));
                            }
                        }
                    }
                    else
                    {
                        requestPath = methodParts[1];
                    }

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

                                case "Host":
                                    hostName = headerValue;
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

                context.Request.Url = new Uri("http://" + hostName + requestPath);

                if (contentLength > 0)
                {
                    if (context.Request.Headers.Contains("Content-Type"))
                    {

                    }
                    else
                    {
                        state.Buffer = new byte[contentLength];
                    }
                }
            }
            return true;
        }
        #endregion
        #region Old
        /*
        #region Methods - Public
        /// <summary>
        /// Translates an HTTP request, represented as an array of bytes,
        /// to the CommonRequest portion of a new CommonContext object.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="unused"></param>
        /// <returns></returns>
        public override void ConstructRequest(byte[] source, out byte[] unused)
        {
            if ((source == null) || (source.Length == 0))
            {
                unused = new byte[0];
                return;
            }
            string input = Encoding.ASCII.GetString(source);
            this.buffer = input;
            int indexOf = 0;
            while (indexOf != -1)
            {
                switch (this.currentStep)
                {
                    case HttpAdapterSteps.PreParse:
                        indexOf = this.buffer.IndexOf("\r\n");
                        if (indexOf != -1)
                        {
                            if (indexOf == 0)
                            {
                                this.currentStep = HttpAdapterSteps.HeadersParsed;
                                break;
                            }
                            string Line = this.buffer.Substring(0, indexOf);
                            this.buffer = this.buffer.Substring(indexOf + 2);

                            string[] MethodParts = Line.Split(' ');
                            //First line must be "<METHOD> <URI> HTTP/<VERSION>" which translates to 3 elements
                            //when split by the space char.
                            if (MethodParts.Length == 3)
                            {
                                //Get down to business.
                                switch (MethodParts[0])
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
                                        this.CurrentContext.Request.Method = MethodParts[0];
                                        break;

                                    default:
                                        //WS: We need to generate an error here if the method is not supported.
                                        this.CurrentContext.Request.Method = "GET";
                                        break;
                                }
                                switch (MethodParts[2])
                                {
                                    case "HTTP/0.9":
                                        this.CurrentContext.ProtocolVersion = new Version(0, 9);
                                        break;
                                    case "HTTP/1.0":
                                        this.CurrentContext.ProtocolVersion = new Version(1, 0);
                                        break;
                                        
                                    default:
                                        //WS: This should probably be changed to send an error to the client.
                                        this.CurrentContext.ProtocolVersion = new Version(1, 1);
                                        break;
                                }
                                if (MethodParts[1].Contains("?") == true)
                                {
                                    this.requestPath = MethodParts[1].Substring(0, MethodParts[1].IndexOf('?'));
                                    this.ProcessUrlEncodedRequestData(MethodParts[1].Substring(MethodParts[1].IndexOf('?') + 1));
                                }
                                else
                                {
                                    this.requestPath = MethodParts[1];
                                }
                                this.currentStep = HttpAdapterSteps.MethodParsed;
                            }
                            //The first line is malformed, oops!
                            else
                            {

                            }
                        }
                        break;

                    case HttpAdapterSteps.MethodParsed:
                        indexOf = this.buffer.IndexOf("\r\n");
                        while (indexOf != -1)
                        {
                            string Line = this.buffer.Substring(0, indexOf);
                            this.buffer = this.buffer.Substring(indexOf + 2);
                            if (Line.Length > 0)
                            {
                                int N = Line.IndexOf(':');
                                if (N != -1)
                                {
                                    string HeaderName = Line.Substring(0, N);
                                    string HeaderValue = Line.Substring(N + 2);
                                    Header H = new Header(HeaderName, HeaderValue);
                                    switch (HeaderName)
                                    {
                                        case "Content-Length":
                                            this.contentLength = int.Parse(HeaderValue);
                                            break;
                                        case "Content-Type":
                                            if (HeaderValue.StartsWith("multipart/form-data") == true)
                                            {
                                                this.CurrentContext.Request.ContentType = "multipart/form-data";
                                                this.multipartBoundary = HeaderValue.Substring(HeaderValue.IndexOf('=') + 1);
                                            }
                                            else
                                            {
                                                this.CurrentContext.Request.ContentType = "application/x-www-form-urlencoded";
                                            }
                                            H = new Header(HeaderName, HeaderValue);
                                            break;

                                        case "Host":
                                            this.CurrentContext.Request.Url = new Uri("http://" + HeaderValue + this.requestPath);
                                            break;

                                        case "Accept":
                                        case "Accept-Charset":
                                        //AJ: Removed for http compression support
                                        //case "Accept-Encoding":
                                        case "Accept-Language":
                                        case "Keep-Alive":
                                            
                                            H = null;
                                            break;
                                    }
                                    if (H != null)
                                    {
                                        this.CurrentContext.Request.Headers.Add(H);
                                    }
                                }
                            }
                            else
                            {
                                //The line is empty
                                this.currentStep = HttpAdapterSteps.HeadersParsed;
                            }
                            indexOf = this.buffer.IndexOf("\r\n");
                        }
                        this.currentStep = HttpAdapterSteps.HeadersParsed;
                        break;

                    case HttpAdapterSteps.HeadersParsed:
                        //parse fields (GET/POST data)
                        if (this.contentLength > 0)
                        {
                            if (this.buffer.Length < this.contentLength)
                            {
                                unused = Encoding.ASCII.GetBytes(this.buffer);
                                return;
                            }
                            else
                            {
                                if (this.buffer.Length > this.contentLength)
                                {
                                    unused = Encoding.ASCII.GetBytes(this.buffer.Substring(this.contentLength));
                                }
                                this.buffer = this.buffer.Substring(0, this.contentLength);

                                switch (this.CurrentContext.Request.ContentType)
                                {
                                    case "application/x-www-form-urlencoded":
                                        this.ProcessUrlEncodedRequestData(this.buffer);
                                        break;
                                }
                            }
                        }
                        this.currentStep = HttpAdapterSteps.FieldsParsed;
                        break;

                    case HttpAdapterSteps.FieldsParsed:
                        if (this.CurrentContext.Request.Url == null)
                        {
                            this.CurrentContext.Request.Url = new Uri("http://localhost/");
                        }
                        this.Recycle();
                        unused = new byte[0];
                        this.currentStep = HttpAdapterSteps.PreParse;
                        //Console.WriteLine("Returning from parsing method");
                        return;
                }
            }
            unused = new byte[source.Length];
            source.CopyTo(unused, 0);
        }
        /// <summary>
        /// Converts an array of bytes to a CommonResponse, or part of a CommonResponse.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="unused"></param>
        public override void ConstructResponse(byte[] source, out byte[] unused)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// Converts a CommonRequest to an array of bytes.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override byte[] DestructRequest(CommonContext context)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// Translates the CommonResponse portion of a CommonContext
        /// object to an HTTP response, represented as an array of bytes.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override byte[] DestructResponse(CommonContext context)
        {
            CommonRequest Request = context.Request;
            CommonResponse Response = context.Response;
            StringBuilder outputText = new StringBuilder();
            outputText.Append("HTTP/1.1 " + Response.Status.ToString() + "\r\n");
            byte[] contentBuffer;
            if (Request.Headers.Contains("Accept-Encoding"))
            {
                contentBuffer = this.CompressContent(context);
            }
            else
            {
                contentBuffer = Response.SendBuffer;
            }

            if (Response.Headers.Contains("Content-Length") == false)
            {
                Response.Headers.Add("Content-Length", contentBuffer.Length.ToString());
            }
            if (Response.Headers.Contains("Content-Type") == false)
            {
                Response.Headers.Add("Content-Type", Response.MimeType + "; charset=UTF-8");
            }
            if (Response.Headers.Contains("Server"))
            {
                Response.Headers.Remove("Server");
            }
            Response.Headers.Add(new Header("Server", SerenityInfo.Name + "/" + SerenityInfo.Version));

            foreach (Header H in Response.Headers)
            {
                outputText.Append(string.Format("{0}: {1}\r\n", H.Name, this.GetWholeValue(H)));
            }
            outputText.Append("\r\n");
            List<Byte> output = new List<Byte>(Encoding.ASCII.GetBytes(outputText.ToString()));
            output.AddRange(contentBuffer);

            return output.ToArray();
        }
        #endregion
        */
        #endregion
    }
}