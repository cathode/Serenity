/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Serenity.Web;
using System.Diagnostics;

namespace Serenity.Net
{
    /// <summary>
    /// Listens for and accepts incoming HTTP connections.
    /// </summary>
    public class HttpServer : IDisposable
    {
        #region Constructors
        public HttpServer()
        {
            this.port = HttpServer.DefaultPort;
            this.RequestValidator = new DefaultRequestValidator();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServer"/> class.
        /// </summary>
        /// <param name="port">The port number to listen for connections on. Typically, the HTTP protocol uses port 80.</param>
        public HttpServer(ushort port)
        {
            Contract.Requires(port > 0);
            this.port = port;
            this.RequestValidator = new DefaultRequestValidator();
        }
        #endregion
        #region Fields
        /// <summary>
        /// Holds the default port number to listen on. Typically, the HTTP protocol uses port 80, this is the default.
        /// </summary>
        public const ushort DefaultPort = 80;
        public const int MinimumRequestLength = 31; // Length of shortest valid HTTP 1.1 request: "GET / HTTP1.1/\r\nHost:\r\n\r\n"
        public const int DefaultReceiveTimeout = 30000;
        private ushort port;
        private int backlog = 10;
        private Socket listenSocket;
        public Action<Request, Response> ProcessRequestCallback
        {
            get;
            set;
        }

        public RequestValidator RequestValidator
        {
            get;
            set;
        }
        #endregion
        #region Methods
        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.listenSocket != null)
                this.listenSocket.Dispose();
        }

        /// <summary>
        /// Starts the <see cref="HttpServer"/> and 
        /// </summary>
        public void Start()
        {
            if (this.listenSocket != null)
                this.listenSocket.Dispose();

            this.listenSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            this.listenSocket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, 0); // Set IPV6_V6ONLY to false
            this.listenSocket.Bind(new IPEndPoint(IPAddress.IPv6Any, this.port));
            this.listenSocket.Listen(this.backlog);
            this.ResumeListening();
        }

        public void Stop()
        {

        }

        /// <summary>
        /// Provides a callback method for asynchronous accept operations.
        /// </summary>
        /// <param name="result"></param>
        protected virtual void AcceptCallback(IAsyncResult result)
        {
            Contract.Requires(result != null);
            

            try
            {
                var socket = this.listenSocket.EndAccept(result);
               
                if (socket != null)
                {
                    Trace.WriteLine(DateTime.Now.ToString() + "Accepted connection from " + socket.RemoteEndPoint.ToString());
                    var state = this.CreateStateObject(socket);
                    state.ReceiveTimer = new Timer(this.ReceiveTimeoutCallback, state, HttpServer.DefaultReceiveTimeout, Timeout.Infinite);
                    state.Connection.BeginReceive(state.Buffer.Receive, 0, Math.Min(state.Connection.Available, state.Buffer.Receive.Length), SocketFlags.None, new AsyncCallback(this.ReceiveCallback), state);
                }

                this.listenSocket.BeginAccept(this.AcceptCallback, null);
            }
            catch (SocketException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Provides a callback method for asynchronous socket receive calls.
        /// </summary>
        /// <param name="result"></param>
        protected virtual void ReceiveCallback(IAsyncResult result)
        {
            Contract.Requires(result != null);

            var state = (HttpServerAsyncState)result.AsyncState;

            if (state.Connection.Connected) // Only try and receive if the client is still connected.
            {
                try
                {
                    int received = state.Connection.EndReceive(result);
                    Console.WriteLine("Received: {0}", received);
                    if (received == 0)
                    {
                        state.Connection.Close();
                        state.Dispose();
                        return;
                    }
                    state.Buffer.SwapBuffers(received);


                    var request = state.Request;
                    var response = state.Response;

                    var buffer = state.Buffer.Data;

                    if (buffer.Count > HttpServer.MinimumRequestLength || state.Stage > HttpRequestParseStep.Method)
                    {
                        char c = (char)0;
                        char prev = (char)0;
                        int imax = buffer.Count;
                        for (int i = 0; i < imax; i++)
                        {
                            prev = c;
                            c = (char)buffer.Dequeue();

                            state.RawRequest.Append(c);
                            bool appendToken = true;
                            bool repeat = false;
                            do
                            {
                                switch (state.Stage)
                                {
                                    case HttpRequestParseStep.Method:
                                        // Nothing processed, first we need to build the HTTP Method.
                                        if (c == ' ')
                                        {
                                            // Method has been terminated by a space.
                                            request.RawMethod = state.CurrentToken.ToString();
                                            state.CurrentToken = new StringBuilder();
                                            state.Stage = HttpRequestParseStep.Uri;
                                            appendToken = false;
                                        }
                                        break;

                                    case HttpRequestParseStep.Uri:
                                        // Now we need to get the URI
                                        if (c == ' ')
                                        {
                                            // End of URI token
                                            request.RawUrl = state.CurrentToken.ToString();
                                            request.Url = new Uri(request.RawUrl, UriKind.RelativeOrAbsolute);
                                            state.CurrentToken = new StringBuilder();
                                            state.Stage = HttpRequestParseStep.Version;
                                            appendToken = false;
                                        }
                                        break;
                                    case HttpRequestParseStep.Version:
                                        // Next is the HTTP version token
                                        if (c == '\n' && state.CurrentToken[state.CurrentToken.Length - 1] == '\r')
                                        {
                                            // HTTP version is terminated by end of line
                                            string rawVersion = state.CurrentToken.ToString();
                                            rawVersion = rawVersion.Substring(0, rawVersion.Length - 1);

                                            state.CurrentToken = new StringBuilder();
                                            state.Stage = HttpRequestParseStep.HeaderName;
                                            appendToken = false;
                                        }
                                        break;
                                    case HttpRequestParseStep.HeaderName:
                                        // Parse the next header name
                                        if (c == ':')
                                        {
                                            // Header name token terminated with ':'
                                            string headerName = state.CurrentToken.ToString().Trim('\r', '\n', ' ', ':');
                                            state.PreviousToken = headerName;
                                            state.CurrentToken = new StringBuilder();
                                            state.Stage = HttpRequestParseStep.HeaderValue;
                                            appendToken = false;
                                        }
                                        else if (c == '\n' && prev == '\r')
                                        {
                                            state.CurrentToken = new StringBuilder();
                                            state.Stage = HttpRequestParseStep.Content;
                                            repeat = true;
                                        }
                                        break;
                                    case HttpRequestParseStep.HeaderValue:
                                        if (state.CurrentToken.Length >= 3)
                                        {
                                            string last4 = state.CurrentToken.ToString().Substring(state.CurrentToken.Length - 3) + c;
                                            string last2 = last4.Substring(2);
                                            if (last2 == "\r\n")
                                            {
                                                string headerValue = state.CurrentToken.ToString().Trim('\r', '\n', ' ', '\t');
                                                Header h = new Header(state.PreviousToken, headerValue);
                                                state.Request.Headers.Add(h);
                                                state.PreviousToken = headerValue;
                                                state.CurrentToken = new StringBuilder();

                                                if (last4 == "\r\n\r\n")
                                                    state.Stage = HttpRequestParseStep.Content;
                                                else
                                                    state.Stage = HttpRequestParseStep.HeaderName;
                                                
                                                appendToken = false;
                                            }
                                        }
                                        break;

                                    case HttpRequestParseStep.Content:
                                    // TODO: Implement content parsing.

                                        if (request.Headers.Contains("Content-Type"))
                                        {
                                        }
                                        // Done, move to next stage.
                                        state.Stage = HttpRequestParseStep.CreateResponse;
                                        break;

                                    // For now, skip it and fall down to creating a response.
                                    case HttpRequestParseStep.CreateResponse:
                                        request.Url = new Uri(new Uri("http://" + request.Headers["Host"].Value), request.RawUrl);
                                        if (this.RequestValidator.ValidateRequest(request, response))
                                        {
                                            this.ProcessRequestCallback(request, response);
                                        }
                                        this.SendResponse(request, response);
                                        repeat = false;
                                        break;

                                    default:
                                        throw new NotImplementedException();
                                }

                                if (appendToken)
                                {
                                    state.CurrentToken.Append(c);
                                }

                            }
                            while (repeat);
                        }
                    }
                    if (state.Connection.Connected)
                    {
                        
                        state.ReceiveTimer.Change(30000, Timeout.Infinite);
                        state.Connection.BeginReceive(state.Buffer.Receive, 0,
                                Math.Min(state.Connection.Available, state.Buffer.Receive.Length),
                                SocketFlags.None, this.ReceiveCallback, state);
                       
                    }
                }
                catch (SocketException ex)
                {
                    throw ex;
                }
            }
            if (!state.Connection.Connected)
            {
                // If control reaches this line, then we need to clean up.
                state.Dispose();
            }
        }

        /// <summary>
        /// Callback used when a client fails to send data
        /// </summary>
        /// <param name="state"></param>
        protected virtual void ReceiveTimeoutCallback(object state)
        {
            var serverState = (HttpServerAsyncState)state;
            byte[] data = Encoding.ASCII.GetBytes(@"HTTP/1.1 408 Request Timeout");
            try
            {
                serverState.Connection.Send(data);

                // TODO: Implement async sending.
                // serverState.Client.BeginSend(data, SocketFlags.None, new AsyncCallback(this.SendCallback), state);

                serverState.Connection.Close(600);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected virtual void SendResponse(Request request, Response response)
        {
            StringBuilder content = new StringBuilder();

            content.AppendFormat("HTTP/1.1 {0}\r\n", response.Status.ToString());

            //TODO: Implement the response text creation.

            if (!response.Headers.Contains("Content-Type"))
                response.Headers.Add(new Header("Content-Type", response.ContentType.ToString()));

            foreach (Header h in response.Headers)
            {
                content.Append(h.ToString());
                content.Append("\r\n");
            }

            content.Append("\r\n");
            byte[] headerData = Encoding.ASCII.GetBytes(content.ToString());
            byte[] data = new byte[headerData.Length + response.OutputBuffer.Count];

            headerData.CopyTo(data, 0);
            response.OutputBuffer.CopyTo(data, headerData.Length);

            if (response.Connection.Connected)
                response.Connection.Send(data);

            response.Connection.Close(600);
        }

        /// <summary>
        /// Creates a new <see cref="ServerAsyncState"/>.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// To utilize a more complex async state object, make your async state
        /// object inherit from <see cref="ServerAsyncState"/> and then
        /// override this method to return a new instance of your derived type.
        /// </remarks>
        protected virtual HttpServerAsyncState CreateStateObject(Socket socket)
        {
            Contract.Requires(socket != null);

            return new HttpServerAsyncState(socket);
        }

        protected void ResumeListening()
        {
            this.listenSocket.BeginAccept(this.AcceptCallback, null);
        }

      

        [ContractInvariantMethod]
        private void __InvariantMethod()
        {
            Contract.Invariant(this.port > 0);
        }
        #endregion
    }
}