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
using System.Net.Sockets;
using System.Threading;
using Serenity.Properties;
using Serenity.Web;
using Serenity.Web.Resources;
using System.Text;
using System.Collections.Generic;

namespace Serenity.Net
{
    /// <summary>
    /// Represents basic functionality for server objects. A server handles
    /// network communication with clients and directs request/response
    /// generation.
    /// </summary>
    public class Server : IDisposable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Server"/> class.
        /// </summary>
        public Server()
        {
            this.rootResource = new RootResource(this);
        }
        #endregion
        #region Events
        /// <summary>
        /// Raised when the current <see cref="Server"/> is being initialized.
        /// </summary>
        public event EventHandler Initializing;
        /// <summary>
        /// Raised when the current <see cref="Server"/> is being started.
        /// </summary>
        public event EventHandler Starting;
        /// <summary>
        /// Raised when the current <see cref="Server"/> is being stopped.
        /// </summary>
        public event EventHandler Stopping;
        #endregion
        #region Fields
        private ServerProfile profile;
        private bool isRunning;
        private bool isDisposed;
        private bool isInitialized;
        private Socket listener;
        private readonly ModuleCollection modules = new ModuleCollection();
        private readonly EventLog log = new EventLog();
        private Resource rootResource;
        private static readonly int MinimumRequestLength = "GET / HTTP1.1/\r\nHost:\r\n\r\n".Length;
        #endregion
        #region Methods
        /// <summary>
        /// Provides a callback method for asynchronous accept operations.
        /// </summary>
        /// <param name="result"></param>
        protected virtual void AcceptCallback(IAsyncResult result)
        {
            if (result == null)
                throw new ArgumentNullException("result");

            try
            {
                var state = this.CreateStateObject(this.Listener.EndAccept(result));
                this.Listener.BeginAccept(new AsyncCallback(this.AcceptCallback), null);

                state.Reset();
                state.ReceiveTimer = new Timer(new TimerCallback(this.ReceiveTimeoutCallback), state,
                   this.Profile.ReceiveTimeout, Timeout.Infinite);
                state.Connection.BeginReceive(state.Buffer.Receive, 0,
                          Math.Min(state.Connection.Available, state.Buffer.Receive.Length),
                          SocketFlags.None, new AsyncCallback(this.ReceiveCallback), state);

            }
            catch (SocketException ex)
            {
                this.Log.RecordEvent(ex.Message, EventKind.Notice, ex.StackTrace);
                return;
            }
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
        protected virtual ServerAsyncState CreateStateObject(Socket connection)
        {
            return new ServerAsyncState(connection);
        }
        /// <summary>
        /// Disposes the current <see cref="Server"/>.
        /// </summary>
        public void Dispose()
        {
            if (!this.IsDisposed)
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
                this.isDisposed = true;
            }
        }
        /// <summary>
        /// Disposes the current <see cref="Server"/>.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            //TODO: Implement dispose for Server
        }
        /// <summary>
        /// Initializes the current <see cref="Server"/>. Commonly, tasks such
        /// as creating and binding sockets, loading modules, and other similar
        /// actions are carried out during initialization.
        /// </summary>
        public void Initialize()
        {
            if (!this.IsInitialized)
            {
                this.OnInitializing(null);
                this.isInitialized = true;
            }
        }
        /// <summary>
        /// Raises the <see cref="Initializing"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnInitializing(EventArgs e)
        {
            if (this.Initializing != null)
            {
                this.Initializing(this, e);
            }

            foreach (string modulePath in this.Profile.Modules)
            {
                foreach (var module in Module.LoadModules(modulePath))
                {
                    DirectoryResource modTree = new DirectoryResource()
                    {
                        Name = module.Name,
                        Owner = this
                    };

                    modTree.Add(module.Resources);

                    if (this.RootResource is DirectoryResource)
                    {
                        ((DirectoryResource)this.RootResource).Add(modTree);
                    }
                    this.modules.Add(module);
                }
            }
        }
        /// <summary>
        /// Raises the <see cref="Starting"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnStarting(EventArgs e)
        {
            if (this.Starting != null)
                this.Starting(this, e);

            if (this.Profile.UseIPv6)
            {
                this.Listener = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                this.Listener.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName)27, 0); //Set IPV6_V6ONLY to false
            }
            else
            {
                this.Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            this.listener.Bind(this.Profile.LocalEndPoint);
            this.Listener.Listen(this.Profile.ConnectionBacklog);
            this.Listener.BeginAccept(new AsyncCallback(this.AcceptCallback), null);
        }
        /// <summary>
        /// Raises the <see cref="Stopping"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnStopping(EventArgs e)
        {
            if (this.Stopping != null)
                this.Stopping(this, e);

            this.Listener.Close();
        }
        protected virtual void ProcessRequest(Request request, Response response)
        {
            this.RootResource.OnRequest(request, response);
        }
        /// <summary>
        /// Provides a callback method for asynchronous socket receive calls.
        /// </summary>
        /// <param name="result"></param>
        protected virtual void ReceiveCallback(IAsyncResult result)
        {
            if (result == null)
                throw new ArgumentNullException("result");

            var state = (ServerAsyncState)result.AsyncState;

            if (state.Connection.Connected)
            {
                // Only try and receive if the client is still connected.

                try
                {
                    int received = state.Connection.EndReceive(result);
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

                    if (buffer.Count > Server.MinimumRequestLength)
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
                                    case RequestStep.Method:
                                        // Nothing processed, first we need to build the HTTP Method.
                                        if (c == ' ')
                                        {
                                            // Method has been terminated by a space.
                                            request.RawMethod = state.CurrentToken.ToString();
                                            state.CurrentToken = new StringBuilder();
                                            state.Stage = RequestStep.Uri;
                                            appendToken = false;
                                        }
                                        break;

                                    case RequestStep.Uri:
                                        // Now we need to get the URI
                                        if (c == ' ')
                                        {
                                            // End of URI token
                                            request.RawUrl = state.CurrentToken.ToString();
                                            state.CurrentToken = new StringBuilder();
                                            state.Stage = RequestStep.Version;
                                            appendToken = false;
                                        }
                                        break;
                                    case RequestStep.Version:
                                        // Next is the HTTP version token
                                        if (c == '\n' && state.CurrentToken[state.CurrentToken.Length - 1] == '\r')
                                        {
                                            // HTTP version is terminated by end of line
                                            string rawVersion = state.CurrentToken.ToString();
                                            rawVersion = rawVersion.Substring(0, rawVersion.Length - 1);

                                            state.CurrentToken = new StringBuilder();
                                            state.Stage = RequestStep.HeaderName;
                                            appendToken = false;
                                        }
                                        break;
                                    case RequestStep.HeaderName:
                                        // Parse the next header name
                                        if (c == ':')
                                        {
                                            // Header name token terminated with ':'
                                            string headerName = state.CurrentToken.ToString().Trim('\r', '\n', ' ', ':');
                                            state.PreviousToken = headerName;
                                            state.CurrentToken = new StringBuilder();
                                            state.Stage = RequestStep.HeaderValue;
                                            appendToken = false;
                                        }
                                        else if (c == '\n' && prev == '\r')
                                        {
                                            state.CurrentToken = new StringBuilder();
                                            state.Stage = RequestStep.Content;
                                            repeat = true;
                                        }
                                        break;
                                    case RequestStep.HeaderValue:
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
                                                {
                                                    state.Stage = RequestStep.Content;
                                                }
                                                else
                                                {
                                                    state.Stage = RequestStep.HeaderName;
                                                }
                                                appendToken = false;
                                            }
                                        }
                                        break;

                                    case RequestStep.Content:
                                    //TODO: Implement content parsing.
                                    // For now, skip it and fall down to creating a response.
                                    case RequestStep.CreateResponse:
                                        if (this.ValidateRequest(request, response))
                                        {
                                            this.ProcessRequest(request, response);
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
                        state.ReceiveTimer.Change(this.Profile.ReceiveTimeout, Timeout.Infinite);
                        state.Connection.BeginReceive(state.Buffer.Receive, 0,
                                Math.Min(state.Connection.Available, state.Buffer.Receive.Length),
                                SocketFlags.None, new AsyncCallback(this.ReceiveCallback), state);
                    }
                }
                catch (SocketException ex)
                {
                    this.Log.RecordEvent(ex.Message, EventKind.Info, ex.StackTrace);
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
            var serverState = (ServerAsyncState)state;
            byte[] data = Encoding.ASCII.GetBytes(@"HTTP/1.1 408 Request Timeout");
            serverState.Connection.Send(data);
            //TODO: Implement async sending.
            //serverState.Client.BeginSend(data, SocketFlags.None, new AsyncCallback(this.SendCallback), state);

            serverState.Connection.Close();
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

            response.Connection.Close(180);
        }
        /// <summary>
        /// Starts the current <see cref="Server"/>.
        /// </summary>
        public void Start()
        {
            this.OnStarting(null);

            this.isRunning = true;
        }
        /// <summary>
        /// Stops the current <see cref="Server"/>.
        /// </summary>
        public void Stop()
        {
            this.OnStopping(null);

            this.isRunning = false;
        }
        /// <summary>
        /// Determines if a syntactically-correct request is well-formed.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        protected virtual bool ValidateRequest(Request request, Response response)
        {
            if (request.Headers.Contains("Host"))
            {
                request.Url = new Uri("http://" + request.Headers["Host"].Value + request.RawUrl);
                return true;
            }

            return false;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets a value that indicates if the current <see cref="Server"/>
        /// has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                return this.isDisposed;
            }
        }
        /// <summary>
        /// Gets a value that indicates if the current <see cref="Server"/> 
        /// has been initialized.
        /// </summary>
        public bool IsInitialized
        {
            get
            {
                return this.isInitialized;
            }
        }
        /// <summary>
        /// Gets a value that indicates if the current <see cref="Server"/> is running.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
        }
        /// <summary>
        /// Gets the <see cref="Socket"/> that is used to listen for incoming
        /// connections from clients.
        /// </summary>
        protected Socket Listener
        {
            get
            {
                return this.listener;
            }
            set
            {
                this.listener = value;
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="ServerProfile"/> which controls the operating behavior of the current <see cref="Server"/>.
        /// </summary>
        /// <remarks>
        /// <para>This property can only be set when the <see cref="Server"/> is not running.</para>
        /// If an attempt is made to alter the server's profile while it is running, a <see cref="InvalidOperationException"/> will be thrown.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown when an attempt
        /// is made to alter the profile of the current <see cref="Server"/>
        /// while it is running.</exception>
        public ServerProfile Profile
        {
            get
            {
                return this.profile;
            }
            set
            {
                if (this.IsRunning)
                    throw new InvalidOperationException("Cannot alter the server profile while the server is running.");

                this.profile = value;
            }
        }
        /// <summary>
        /// Gets the <see cref="EventLog"/> which handles events generated by the current <see cref="Server"/>.
        /// </summary>
        public EventLog Log
        {
            get
            {
                return this.log;
            }
        }
        /// <summary>
        /// Gets or sets the root resource for the current <see cref="Server"/>.
        /// </summary>
        public Resource RootResource
        {
            get
            {
                return this.rootResource;
            }
            set
            {
                this.rootResource = value;
            }
        }
        #endregion
    }
}