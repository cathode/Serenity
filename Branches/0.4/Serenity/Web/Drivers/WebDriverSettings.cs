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
    public sealed class WebDriverSettings
    {
        #region Fields - Private
        private ushort[] fallbackPorts;
        private ushort listenPort;
        private int recieveInterval;
        private int recieveTimeout;
        #endregion
        #region Fields - Public
        public const int DefaultRecieveInterval = 0;
        public const int DefaultRecieveTimeout = 10000;
        public const int MinimumRecieveInterval = 0;
        public const int MinimumRecieveTimeout = 1;
        #endregion
        #region Methods - Public
        /// <summary>
        /// Creates and returns a new WebDriverSettings object, calculating optimal values based on the supplied values.
        /// </summary>
        /// <param name="listenPort">The primary port to listen on.</param>
        /// <param name="recieveTimeout">The number of milliseconds to wait before closing the connection and declaring it timed-out.</param>
        /// <returns>The new WebDriverSettings.</returns>
        /// <remarks>
        /// The RecieveInterval property is assigned as one one-thousandth of the supplied recieveTimeout, or 1, whichever is larger.
        /// </remarks>
        public static WebDriverSettings Create(ushort listenPort, int recieveTimeout)
        {
            WebDriverSettings setttings = new WebDriverSettings();
            setttings.listenPort = listenPort;
            setttings.RecieveInterval = WebDriverSettings.DefaultRecieveInterval;
            setttings.RecieveTimeout = (recieveTimeout < WebDriverSettings.MinimumRecieveTimeout) ? WebDriverSettings.MinimumRecieveTimeout : recieveTimeout;

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
                return this.recieveInterval;
            }
            set
            {
                this.recieveInterval = Math.Max(value, WebDriverSettings.MinimumRecieveInterval);
            }
        }
        /// <summary>
        /// Gets or sets the number of milliseconds to wait before the connection should close due to timeout.
        /// </summary>
        public int RecieveTimeout
        {
            get
            {
                return this.recieveTimeout;
            }
            set
            {
                this.recieveTimeout = Math.Max(value, WebDriverSettings.MinimumRecieveTimeout);
            }
        }
        #endregion
    }
}