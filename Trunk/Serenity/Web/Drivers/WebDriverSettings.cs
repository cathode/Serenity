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

namespace Serenity.Web.Drivers
{
    /// <summary>
    /// Represents a collection of options used to initialize a WebDriver with.
    /// </summary>
    public class WebDriverSettings
    {
        /// <summary>
        /// The port that the WebDriver should listen on.
        /// </summary>
        public ushort ListenPort;
        /// <summary>
        /// The number of milliseconds to wait before the connection should close
        /// due to timeout.
        /// </summary>
        public int RecieveTimeout;
        /// <summary>
        /// The number of milliseconds to wait between each data availability
        /// check of the client socket.
        /// </summary>
        public int RecieveInterval;
        /// <summary>
        /// The number of milliseconds to wait before the connection should close
        /// due to timeout, when in idle mode.
        /// </summary>
        public int RecieveTimeoutIdle;
        /// <summary>
        /// The number of milliseconds to wait between each data availability
        /// check of the client socket, when in idle mode.
        /// </summary>
        public int RecieveIntervalIdle;
        /// <summary>
        /// Gets or sets the number of milliseconds before the WebDriver goes into idle mode.
        /// </summary>
        public int TimeToIdle;
        /// <summary>
        /// Gets or sets a list of port numbers that should be used,
        /// in the event that the primary listen port is already in use.
        /// </summary>
        public ushort[] FallbackPorts;
    }
}
