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
using System.Text;

namespace Serenity.Web.Drivers
{
    /// <summary>
    /// Represents a collection of options used to initialize a WebDriver with.
    /// </summary>
    public class WebDriverSettings
    {
        
        #region Fields - Private
        private ushort[] fallbackPorts;
        private ushort listenPort;
        private int recieveInterval;
        private int recieveIntervalIdle;
        private int recieveTimeout;
        private int recieveTimeoutIdle;
        private int timeToIdle;
        #endregion
        #region Methods - Public
        /// <summary>
        /// Creates and returns a new WebDriverSettings object, calculating optimal values based on the supplied values.
        /// </summary>
        /// <param name="listenPort">The primary port to listen on.</param>
        /// <param name="recieveInterval">The number of milliseconds between attempts to recieve from the client.</param>
        /// <param name="recieveTimeout">The number of milliseconds to wait before closing the connection and declaring it timed-out.</param>
        /// <returns></returns>
        public static WebDriverSettings Create(ushort listenPort, int recieveInterval, int recieveTimeout)
        {
            WebDriverSettings setttings = new WebDriverSettings();
            setttings.listenPort = listenPort;
            setttings.recieveInterval = recieveInterval;
            setttings.recieveIntervalIdle = recieveInterval * 4;
            setttings.recieveTimeout = recieveTimeout;
            setttings.recieveTimeoutIdle = recieveTimeout * 8;
            setttings.timeToIdle = setttings.recieveTimeoutIdle * 2;

            return setttings;
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets or sets a list of port numbers that should be used,
        /// in the event that the primary listen port is already in use.
        /// </summary>
        public ushort[] FallbackPorts
        {
            get
            {
                return fallbackPorts;
            }
            set
            {
                fallbackPorts = value;
            }
        }
        /// <summary>
        /// Gets or sets the primary port number that the WebDriver should listen on.
        /// </summary>
        public ushort ListenPort
        {
            get
            {
                return listenPort;
            }
            set
            {
                listenPort = value;
            }
        }
        /// <summary>
        /// Gets or sets the number of milliseconds to wait between each data availability
        /// check of the client socket.
        /// </summary>
        public int RecieveInterval
        {
            get
            {
                return recieveInterval;
            }
            set
            {
                recieveInterval = value;
            }
        }
        /// <summary>
        /// Gets or sets the number of milliseconds to wait between each data availability
        /// check of the client socket, when in idle mode.
        /// </summary>
        public int RecieveIntervalIdle
        {
            get
            {
                return recieveIntervalIdle;
            }
            set
            {
                recieveIntervalIdle = value;
            }
        }
        /// <summary>
        /// Gets or sets the number of milliseconds to wait before the connection should close
        /// due to timeout.
        /// </summary>
        public int RecieveTimeout
        {
            get
            {
                return recieveTimeout;
            }
            set
            {
                recieveTimeout = value;
            }
        }
        /// <summary>
        /// Gets or sets the number of milliseconds to wait before the connection should close
        /// due to timeout, when in idle mode.
        /// </summary>
        public int RecieveTimeoutIdle
        {
            get
            {
                return recieveTimeoutIdle;
            }
            set
            {
                recieveTimeoutIdle = value;
            }
        }
        /// <summary>
        /// Gets or sets the number of milliseconds before the WebDriver goes into idle mode.
        /// </summary>
        public int TimeToIdle
        {
            get
            {
                return timeToIdle;
            }
            set
            {
                timeToIdle = value;
            }
        }
        #endregion
    }
}