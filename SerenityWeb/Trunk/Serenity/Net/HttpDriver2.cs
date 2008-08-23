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
using System.Linq;
using System.Text;
using SerenityProject.Common;
using System.Net;
using System.Net.Sockets;
using Serenity.Web;

namespace Serenity.Net
{
    /// <summary>
    /// Provides support for HTTP (Hyper Text Transfer Protocol), implemented as a <see cref="ProtocolDriver2"/>.
    /// </summary>
    public class HttpDriver2 : ProtocolDriver2
    {
        #region Constructors - Public
        public HttpDriver2()
        {
        }
        #endregion
        #region Fields - Private
        private TcpListener listener;
        #endregion
        #region Methods - Protected
        protected virtual void AcceptCallback(IAsyncResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }
            //Get the accepted socket, don't need TcpClient because it's overkill.
            Socket s = this.listener.EndAcceptSocket(result);

            this.listener.BeginAcceptSocket(new AsyncCallback(this.AcceptCallback), null);

            //Create a new request/response pair and begin constructing them.
            RequestProcessingContext pc = RequestProcessingContext.Create(s);

            s.BeginReceive(pc.ReceiveBuffer, 0, pc.ReceiveBuffer.Length, SocketFlags.None, new AsyncCallback(this.RecieveCallback), pc);
        }
        protected override void OnStarted(EventArgs e)
        {
            this.listener = new TcpListener(IPAddress.IPv6Any, this.ListeningPort);

            this.listener.Start();

            this.listener.BeginAcceptSocket(new AsyncCallback(this.AcceptCallback), null);

            base.OnStarted(e);
        }
        protected virtual void RecieveCallback(IAsyncResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }
            RequestProcessingContext pc = (RequestProcessingContext)result.AsyncState;
            if (pc.Connection.Connected)
            {
                try
                {
                    pc.Connection.EndReceive(result);
                }
                catch (SocketException ex)
                {
                    SerenityServer.OperationLog.Write("Client connection error", Serenity.Logging.LogMessageLevel.Notice);
                }
            }
            else
            {
                return;
            }
            pc.SwapBuffers();

            //Check that we're still connected.
            if (pc.Connection.Connected)
            {
                // Asynchronously get more data while processing the data we've received.
                pc.Connection.BeginReceive(pc.ReceiveBuffer, 0, pc.ReceiveBuffer.Length, SocketFlags.None, new AsyncCallback(this.RecieveCallback), pc);
            }
            else
            {
                return;
            }

            Request request = pc.Request; //For readability
            string sbuffer;

            lock (pc.DataBuffer)
            {
                sbuffer = Encoding.ASCII.GetString(pc.DataBuffer.ToArray());

                int bufferSize = sbuffer.Length;
                while (true)
                {
                    if (pc.Stage == RequestProcessingStage.None)
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
                            pc.Stage = RequestProcessingStage.MethodProcessed;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (pc.Stage == RequestProcessingStage.MethodProcessed)
                    {
                        int i = sbuffer.IndexOf(' ');
                        if (i > -1)
                        {
                            string uri = sbuffer.Substring(0, i);
                            sbuffer = sbuffer.Substring(i + 1);
                            request.RawUrl = uri;
                            request.Url = new Uri(uri, UriKind.RelativeOrAbsolute);

                            pc.Stage = RequestProcessingStage.UriProcessed;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (pc.Stage == RequestProcessingStage.UriProcessed)
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

                            pc.Stage = RequestProcessingStage.VersionProcessed;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (pc.Stage == RequestProcessingStage.VersionProcessed || pc.Stage == RequestProcessingStage.HeaderProcessed)
                    {
                        int i = sbuffer.IndexOf("\r\n");

                        if (i == 0)
                        {
                            sbuffer = sbuffer.Substring(i + 2);
                            pc.Stage = RequestProcessingStage.AllHeadersProcessed;
                        }
                        else if (i > -1)
                        {
                            int n = sbuffer.IndexOf(':');

                            if (n > -1)
                            {
                                string headerName = sbuffer.Substring(0, n);
                                string headerValue = sbuffer.Substring(n + 2, i - (n + 2));

                                request.Headers.Add(headerName, headerValue);

                                pc.Stage = RequestProcessingStage.HeaderProcessed;
                            }
                            sbuffer = sbuffer.Substring(i + 2);
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (pc.Stage == RequestProcessingStage.AllHeadersProcessed)
                    {
                        if (sbuffer.Length >= request.ContentLength)
                        {
                            if (request.ContentLength > 0)
                            {
                                //TODO: Further process the entity data of the request into individual named data streams if appropriate.
                                request.RequestData.AddDataStream("", Encoding.ASCII.GetBytes(sbuffer.Substring(0, request.ContentLength)));
                            }
                            pc.Stage = RequestProcessingStage.ProcessingComplete;
                        }
                    }
                    if (pc.Stage == RequestProcessingStage.ProcessingComplete)
                    {
                        //TODO: Send "completed" request to the handling pipeline so a response can be generated.
                        break;
                    }
                }
                if (sbuffer.Length < bufferSize)
                {
                    int n = bufferSize - sbuffer.Length;

                    if (n == pc.DataBuffer.Count)
                    {
                        pc.DataBuffer.Clear();
                    }
                    else
                    {
                        for (int i = 0; i < n; i++)
                        {
                            pc.DataBuffer.Dequeue();
                        }
                    }
                }
            }
        }
        #endregion
        #region Properties - Protected
        protected override string DefaultDescription
        {
            get
            {
                return "HyperText Transfer Protocol driver";
            }
        }
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
                return "SerenityProject";
            }
        }
        protected override string DefaultSchemeName
        {
            get
            {
                return "http";
            }
        }
        protected override VersionRange DefaultSupportedVersions
        {
            get
            {
                return new VersionRange(new Version(1, 1), new Version(1, 1));
            }
        }
        #endregion
        #region Types - Protected
        protected sealed class RequestProcessingContext
        {
            #region Constructors - Private
            private RequestProcessingContext()
            {
                this.dataBuffer = new Queue<byte>();
            }
            #endregion
            #region Fields - Private
            private Request request;
            private Response response;
            private Socket connection;
            private readonly Queue<byte> dataBuffer;
            private byte[] receiveBuffer;
            private RequestProcessingStage stage;
            #endregion
            #region Methods - Public
            public static RequestProcessingContext Create(Socket s)
            {
                return new RequestProcessingContext()
                {
                    connection = s,
                    request = new Request()
                    {
                        Connection = s,
                    },
                    response = new Response()
                    {
                        Connection = s,
                    },
                    receiveBuffer = new byte[s.Available],
                };
            }
            /// <summary>
            /// Swaps the receive buffer to the data buffer.
            /// </summary>
            /// <remarks>
            /// The data buffer is appended, not replaced by this operation.
            /// The receive buffer is emptied and it's new size is set to the number of available bytes
            /// that can be read from the socket.
            /// </remarks>
            public void SwapBuffers()
            {
                for (int i = 0; i < this.receiveBuffer.Length; i++)
                {
                    this.dataBuffer.Enqueue(this.receiveBuffer[i]);
                }
                this.ResetReceiveBuffer();
            }
            public void ResetReceiveBuffer()
            {
                this.receiveBuffer = new byte[this.connection.Available];
            }
            #endregion
            #region Properties - Public
            public Socket Connection
            {
                get
                {
                    return this.connection;
                }
            }
            public Queue<byte> DataBuffer
            {
                get
                {
                    return this.dataBuffer;
                }
            }
            public byte[] ReceiveBuffer
            {
                get
                {
                    return this.receiveBuffer;
                }
            }
            public Request Request
            {
                get
                {
                    return this.request;
                }
            }
            public Response Response
            {
                get
                {
                    return this.response;
                }
            }
            public RequestProcessingStage Stage
            {
                get
                {
                    return this.stage;
                }
                set
                {
                    this.stage = value;
                }
            }
            #endregion
        }
        #endregion
    }
}
