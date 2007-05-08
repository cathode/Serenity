/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://serenityproject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Serenity.Web.Drivers
{
    /// <summary>
    /// Represents the state of a WebDriver's operation.
    /// </summary>
    public enum WebDriverState
    {
        /// <summary>
        /// Indicates that the WebDriver is newly created and has not carried out any action so far.
        /// </summary>
        Created = 0,
        /// <summary>
        /// Indicates that the WebDriver has been initialized with a set of parameters which define it's behaviour.
        /// </summary>
        Initialized = 1,
        /// <summary>
        /// Indicates that the WebDriver is in a stopped state.
        /// It is not listening for connections and all pending requests have been responded to.
        /// </summary>
        Stopped = 2,
        /// <summary>
        /// Indicates that the WebDriver is attempting to reach a Stopped state.
        /// It is not listening for connections,
        /// but some some pending requests are still being responded to.
        /// </summary>
        Stopping = 3,
        /// <summary>
        /// Indicates that the WebDriver is attempting to begin runing normally.
        /// It is not yet listening for connections, or other initialization tasks have not completed yet.
        /// </summary>
        Starting = 4,
        /// <summary>
        /// Indicates that the WebDriver is ready to begin Running. It may already be listening,
        /// but it has not recieved any incoming connections yet.
        /// </summary>
        Started = 5,
        /// <summary>
        /// Indicates that the WebDriver is currently active.
        /// </summary>
        Running = 6
    }
    /// <summary>
    /// Provides a mechanism for recieving and responding to requests from clients (browsers).
    /// </summary>
    public abstract class WebDriver
    {
        #region Constructors - Internal
        /// <summary>
        /// Initializes a new instance of the WebDriver class.
        /// </summary>
        /// <param name="contextHandler">An IContextHandler object which handles
        /// incoming CommonContext objects.</param>
        internal WebDriver(ContextHandler contextHandler)
        {
            this.contextHandler = contextHandler;
            this.isInitialized = false;
            this.state = WebDriverState.Stopped;
        }
        #endregion
        #region Fields - Private
        private ContextHandler contextHandler;
        private DriverInfo info;
        private bool isInitialized;
        private ushort listenPort;
        private int recieveInterval;
        private int recieveTimeout;
        private WebDriverSettings settings;
        private WebDriverState state;
        #endregion
        #region Methods - Private
        /// <summary>
        /// Helps the ThreadedStart method start the current WebDriver's listening tasks on a new thread.
        /// </summary>
        /// <param name="NotUsed">This is not used.</param>
        private void ThreadedStartHelper(object NotUsed)
        {
            this.Start();
        }
        #endregion
        #region Methods - Protected
        /// <summary>
        /// Causes the ContextCallback event to be fired for the current WebDriver.
        /// </summary>
        /// <param name="context">The CommonContext object to populate the event with.</param>
        protected void InvokeContextCallback(CommonContext context)
        {
            this.contextHandler.HandleContext(context);
        }
        #endregion
        #region Methods - Protected - Abstract
        /// <summary>
        /// Contains the code that is executed when the current WebDriver is initialized (before handling clients).
        /// </summary>
        protected abstract void DriverInitialize();
        /// <summary>
        /// When overridden in a derived class, causes the current WebDriver to begin listening for
        /// and accepting incoming connections.
        /// </summary>
        /// <remarks>
        /// A call to this method may not immediately result in a Started state of the current WebDriver.
        /// </remarks>
        protected abstract void DriverStart();
        /// <summary>
        /// When overridden in a derived class, causes the current WebDriver to cease operation.
        /// </summary>
        /// <remarks>
        /// A call to this method may not immediately result in a Stopped state of the current WebDriver.
        /// </remarks>
        protected abstract void DriverStop();
        #endregion
        #region Methods - Public
        /// <summary>
        /// Publicly used method to perform pre-start initialization tasks.
        /// </summary>
        public void Initialize(WebDriverSettings Settings)
        {
            this.settings = Settings;
            this.recieveInterval = Settings.RecieveInterval;
            this.recieveTimeout = Settings.RecieveTimeout;
            this.listenPort = Settings.ListenPort;
            this.DriverInitialize();
            this.isInitialized = true;
        }
        /// <summary>
        /// Starts the current WebDriver on a new thread.
        /// </summary>
        public void ThreadedStart()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ThreadedStartHelper));
        }
        /// <summary>
        /// Starts the WebDriver.
        /// </summary>
        public void Start()
        {
            this.state = WebDriverState.Starting;
            this.DriverStart();
        }
        /// <summary>
        /// Stops the WebDriver.
        /// </summary>
        public void Stop()
        {
            this.state = WebDriverState.Stopping;
            this.DriverStop();
            this.state = WebDriverState.Stopped;
        }
        #endregion
        #region Methods - Public - Abstract
        /// <summary>
        /// When overridden in a derived class, gets a new instance of the WebAdapter used to
        /// process the data of requests that are sent or recieved for the current WebDriver.
        /// </summary>
        public abstract WebAdapter CreateAdapter();

        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets a DriverInfo object which contains information about the current WebDriver.
        /// </summary>
        public DriverInfo Info
        {
            get
            {
                return this.info;
            }
            protected set
            {
                this.info = value;
            }
        }
        /// <summary>
        /// Gets a value that indicates whether the current WebDriver has been initialized yet.
        /// </summary>
        public bool IsInitialized
        {
            get
            {
                return this.isInitialized;
            }
        }
        /// <summary>
        /// Gets the port number that the current WebDriver will listen on for incoming connections.
        /// </summary>
        public ushort ListenPort
        {
            get
            {
                return this.listenPort;
            }
            internal set
            {
                this.listenPort = value;
            }
        }
        /// <summary>
        /// Gets the number of miliseconds to wait between attempts to recieve data from the client.
        /// </summary>
        public int RecieveInterval
        {
            get
            {
                return this.recieveInterval;
            }
        }
        /// <summary>
        /// Gets the number of milliseconds to wait before closing the connection, if no data
        /// was recieved within this timeframe.
        /// </summary>
        public int RecieveTimeout
        {
            get
            {
                return this.recieveTimeout;
            }
        }
        /// <summary>
        /// Gets the WebDriverSettings object used to initialize the current WebDriver.
        /// </summary>
        public WebDriverSettings Settings
        {
            get
            {
                return this.settings;
            }
        }
        /// <summary>
        /// Gets the state of the current WebDriver.
        /// </summary>
        public WebDriverState State
        {
            get
            {
                return this.state;
            }
            protected set
            {
                this.state = value;
            }
        }
        #endregion
    }
}
