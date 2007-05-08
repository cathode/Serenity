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
using System.Text;

namespace Serenity.Web.Drivers
{
    internal enum HttpAdapterSteps
    {
        PreParse,
        MethodParsed,
        HeadersParsed,
        FieldsParsed,
    }
    /// <summary>
    /// Provides a WebAdapter used to translate CommonContexts to and from an HTTP-based WebDriver.
    /// </summary>
    public sealed class HttpAdapter : WebAdapter
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the HttpAdapter class.
        /// </summary>
        public HttpAdapter(WebDriver Origin) : base(Origin)
        {
            this.CurrentContext.ProtocolType = "HTTP";
            this.CurrentContext.ProtocolVersion = new Version(1, 1);
            CommonCapabilities c = this.CurrentContext.Request.Capabilities;
            c.SupportsAuthentication = false;
            c.SupportsChunkedTransfer = false;
            c.SupportsContentControl = true;
            c.SupportsFields = true;
            c.SupportsHeaders = true;
            c.SupportsPeerInfo = true;
            c = this.CurrentContext.Response.Capabilities;
            c.SupportsAuthentication = false;
            c.SupportsChunkedTransfer = false;
            c.SupportsContentControl = true;
            c.SupportsFields = true;
            c.SupportsHeaders = true;
            c.SupportsPeerInfo = true;
        }
        #endregion
        #region Fields - Private
        private string buffer;
        private string requestPath;
        private HttpAdapterSteps currentStep = HttpAdapterSteps.PreParse;
        private int contentLength;
        private string multipartBoundary;
        #endregion
        #region Methods - Private
        private byte[] CompressContent(CommonContext Context)
        {
            CommonRequest request = Context.Request;
            CommonResponse response = Context.Response;
            byte[] contentBuffer;
            bool useGzip = false;
            bool useDeflate = false;

            if (response.UseCompression == false)
            {
                contentBuffer = response.SendBuffer;
            }
            else
            {
                Header compressionHeader = request.Headers["Accept-Encoding"];

                if (compressionHeader.PrimaryValue.Contains("gzip") == true)
                {
                    useGzip = true;
                }
                else if (compressionHeader.PrimaryValue.Contains("deflate") == true)
                {
                    useDeflate = true;
                }

                if ((useGzip == false) && (useDeflate == false) && (compressionHeader.Complex == true))
                {
                    foreach (string secondaryValue in compressionHeader.SecondaryValues)
                    {
                        if (secondaryValue.Contains("gzip") == true)
                        {
                            useGzip = true;
                            break;
                        }
                        else if (secondaryValue.Contains("deflate") == true)
                        {
                            useDeflate = true;
                            break;
                        }
                    }
                }
                if (useGzip == true)
                {
                    using (MemoryStream memoryStream = new System.IO.MemoryStream())
                    {
                        using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                        {
                            //AJ: Compress content
                            gZipStream.Write(response.SendBuffer, 0, response.SendBuffer.Length);
                            gZipStream.Flush();
                            gZipStream.Close();
                            contentBuffer = memoryStream.ToArray();
                        }
                    }
                    Log.Write("GZipped message: "
                        + response.SendBuffer.Length.ToString()
                        + " bytes to "
                        + contentBuffer.Length.ToString()
                        + " bytes.",
                        LogMessageLevel.Debug);
                }
                else if (useDeflate == true)
                {
                    using (MemoryStream memoryStream = new System.IO.MemoryStream())
                    {
                        using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress))
                        {
                            //AJ: Compress content
                            deflateStream.Write(response.SendBuffer, 0, response.SendBuffer.Length);
                            deflateStream.Flush();
                            deflateStream.Close();
                            contentBuffer = memoryStream.ToArray();
                        }
                    }
                    Log.Write("Deflated message: "
                        + response.SendBuffer.Length.ToString()
                        + " bytes to "
                        + contentBuffer.Length.ToString()
                        + " bytes.",
                        LogMessageLevel.Debug);
                }
                else
                {
                    contentBuffer = response.SendBuffer;
                }
                if (contentBuffer.Length > response.SendBuffer.Length)
                {
                    contentBuffer = response.SendBuffer;
                }
                else
                {
                    if (useGzip == true)
                    {
                        response.Headers.Add("Content-Encoding", "gzip");
                        
                    }
                    else if (useDeflate == true)
                    {
                        response.Headers.Add("Content-Encoding", "deflate");
                    }
                }

            }

            if (response.Headers.Contains("Content-Length") == false)
            {
                response.Headers.Add("Content-Length", contentBuffer.Length.ToString());
            }
            else
            {
                response.Headers["Content-Length"].PrimaryValue = contentBuffer.Length.ToString();
            }
            return contentBuffer;
        }
        private string GetWholeValue(Header H)
        {
            if (H.Complex == true)
            {
                switch (H.Name)
                {
                    default:
                        return string.Format("{0},{1}", H.PrimaryValue, string.Join(",", H.SecondaryValues)).TrimEnd('\r', '\n');
                }
            }
            else
            {
                return H.PrimaryValue.TrimEnd('\r', '\n');
            }
        }
        private void ProcessUrlEncodedRequestData(string Input)
        {
            string[] Pairs = Input.Split('&');
            foreach (string Pair in Pairs)
            {
                if (Pair.IndexOf('=') != -1)
                {
                    string Name = Pair.Substring(0, Pair.IndexOf('='));
                    string Value = Pair.Substring(Pair.IndexOf('=') + 1);
                    this.CurrentContext.Request.RequestData.AddDataStream(Name, Encoding.UTF8.GetBytes(Value));
                }
            }
        }
        #endregion
        #region Methods - Public
        /// <summary>
        /// Translates an HTTP request, represented as an array of bytes,
        /// to the CommonRequest portion of a new CommonContext object.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="unused"></param>
        /// <returns></returns>
        public override void ConstructRequest(Byte[] source, out Byte[] unused)
        {
            if ((source == null) || (source.Length == 0))
            {
                unused = new Byte[0];
                return;
            }
            string Input = Encoding.ASCII.GetString(source);
            this.buffer = Input;
            while (true)
            {
                int IndexOf = 0;
                switch (this.currentStep)
                {
                    case HttpAdapterSteps.PreParse:
                        IndexOf = this.buffer.IndexOf("\r\n");
                        if (IndexOf != -1)
                        {
                            if (IndexOf == 0)
                            {
                                this.currentStep = HttpAdapterSteps.HeadersParsed;
                                break;
                            }
                            string Line = this.buffer.Substring(0, IndexOf);
                            this.buffer = this.buffer.Substring(IndexOf + 2);

                            string[] MethodParts = Line.Split(' ');
                            //First line must be "<METHOD> <URI> HTTP/<VERSION>" which translates to 3 elements
                            //when split by the space char.
                            if (MethodParts.Length == 3)
                            {
                                //Get down to business.
                                switch (MethodParts[0])
                                {
                                    case "HEAD":
                                    case "POST":
                                        this.CurrentContext.Request.Method = MethodParts[0];
                                        break;

                                    default:
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
                        IndexOf = this.buffer.IndexOf("\r\n");
                        while (IndexOf != -1)
                        {
                            string Line = this.buffer.Substring(0, IndexOf);
                            this.buffer = this.buffer.Substring(IndexOf + 2);
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
                            IndexOf = this.buffer.IndexOf("\r\n");
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
                        this.Recycle();
                        unused = new Byte[0];
                        this.currentStep = HttpAdapterSteps.PreParse;
                        //Console.WriteLine("Returning from parsing method");
                        return;
                }
            }
        }
        /// <summary>
        /// Converts an array of bytes to a CommonResponse, or part of a CommonResponse.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="unused"></param>
        public override void ConstructResponse(Byte[] source, out Byte[] unused)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// Converts a CommonRequest to an array of bytes.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Byte[] DestructRequest(CommonContext context)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// Translates the CommonResponse portion of a CommonContext
        /// object to an HTTP response, represented as an array of bytes.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Byte[] DestructResponse(CommonContext context)
        {
            CommonRequest Request = context.Request;
            CommonResponse Response = context.Response;
            StringBuilder outputText = new StringBuilder();
            outputText.Append("HTTP/1.1 " + Response.Status.ToString() + "\r\n");
            Byte[] contentBuffer;
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
                Response.Headers.Add("Content-Type", Response.MimeType + "; charset=UTF-8\r\n");
            }
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
    }
}