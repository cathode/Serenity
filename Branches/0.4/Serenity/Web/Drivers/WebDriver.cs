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
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Serenity.Web.Drivers
{
    /// <summary>
    /// Provides a mechanism for recieving and responding to requests from clients (browsers).
    /// </summary>
    public abstract class WebDriver
    {
        #region Constructors - Internal
        /// <summary>
        /// Initializes a new instance of the WebDriver class.
        /// </summary>
        /// <param name="contextHandler">A ContextHandler which handles
        /// incoming CommonContext objects.</param>
        protected WebDriver(ContextHandler contextHandler)
        {
            this.contextHandler = contextHandler;
        }
        #endregion
        #region Fields - Private
        private ContextHandler contextHandler;
        private DriverInfo info;
        private ushort listenPort;
        private int recieveInterval;
        private int recieveTimeout;
        private WebDriverSettings settings;
        private WebDriverState state = WebDriverState.None;
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
		protected void DisconnectCallback(IAsyncResult ar)
		{
			((Socket)ar.AsyncState).EndDisconnect(ar);
		}
        /// <summary>
        /// Contains the code that is executed when the current WebDriver is initialized (before handling clients).
        /// </summary>
        protected abstract bool DriverInitialize();
        /// <summary>
        /// When overridden in a derived class, causes the current WebDriver to begin listening for
        /// and accepting incoming connections.
        /// </summary>
        /// <remarks>
        /// A call to this method may not immediately result in a Started state of the current WebDriver.
        /// </remarks>
        protected abstract bool DriverStart();
        /// <summary>
        /// When overridden in a derived class, causes the current WebDriver to cease operation.
        /// </summary>
        /// <remarks>
        /// A call to this method may not immediately result in a Stopped state of the current WebDriver.
        /// </remarks>
        protected abstract bool DriverStop();
        /// <summary>
        /// Causes the ContextCallback event to be fired for the current WebDriver.
        /// </summary>
        /// <param name="context">The CommonContext object to populate the event with.</param>
        protected void InvokeContextCallback(CommonContext context)
        {
            this.contextHandler.HandleContext(context);
        }
        #endregion
        #region Methods - Public
        /// <summary>
        /// When overridden in a derived class, gets a new instance of the WebAdapter used to
        /// process the data of requests that are sent or recieved for the current WebDriver.
        /// </summary>
        public abstract WebAdapter CreateAdapter();
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
            this.state = WebDriverState.Initialized;
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
        public bool Start()
        {
            if (this.state == WebDriverState.Initialized)
            {
                return this.DriverStart();
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Stops the WebDriver.
        /// </summary>
        public bool Stop()
        {
            if (this.state == WebDriverState.Started)
            {
                return this.DriverStop();
            }
            else
            {
                return false;
            }
        }
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
                if (this.state >= WebDriverState.Initialized)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Gets a boolean value that indicates whether the current WebDriver is in a started state.
        /// </summary>
        public bool IsStarted
        {
            get
            {
                if (this.state == WebDriverState.Started)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Gets a boolean value that indicates whether the current WebDriver is in a stopped state.
        /// </summary>
        public bool IsStopped
        {
            get
            {
                if (this.state == WebDriverState.Stopped)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Gets the port number that the current WebDriver is listening on for incoming connections.
        /// </summary>
        public ushort ListenPort
        {
            get
            {
                return this.listenPort;
            }
            protected set
            {
                this.listenPort = value;
            }
        }
        /// <summary>
        /// Gets the WebDriverSettings which determine the behaviour of the current WebDriver.
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
