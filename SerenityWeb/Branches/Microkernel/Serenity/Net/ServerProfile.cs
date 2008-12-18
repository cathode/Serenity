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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Serenity.Net
{
    /// <summary>
    /// Represents a set of properties that controls the behavior of a
    /// <see cref="Server"/>.
    /// </summary>
    public struct ServerProfile
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerProfile"/>
        /// struct.
        /// </summary>
        /// <param name="name"></param>
        public ServerProfile(string name)
        {
            this.connectionBacklog = 10;
            this.maxReceiveRateTotal = 0;
            this.maxSendRateTotal = 0;
            this.modules = new string[0];
            this.name = name;
            this.localEndPoint = new IPEndPoint(IPAddress.Any, 0);
        }
        #endregion
        #region Fields
        private static ServerProfile defaultProfile = new ServerProfile();
        private string name;
        private IPEndPoint localEndPoint;
        private int connectionBacklog;
        private int maxReceiveRateTotal;
        private int maxSendRateTotal;
        private string[] modules;
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the size of the <see cref="Server"/>'s connection
        /// backlog.
        /// </summary>
        public int ConnectionBacklog
        {
            get
            {
                return this.connectionBacklog;
            }
            set
            {
                this.connectionBacklog = value;
            }
        }
        /// <summary>
        /// Gets or sets the default <see cref="ServerProfile"/> to use when
        /// a default is required.
        /// </summary>
        public static ServerProfile DefaultProfile
        {
            get
            {
                return ServerProfile.defaultProfile;
            }
            set
            {
                ServerProfile.defaultProfile = value;
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="IPEndPoint"/> to bind the
        /// <see cref="Server"/>'s listening socket to.
        /// </summary>
        public IPEndPoint LocalEndPoint
        {
            get
            {
                return this.localEndPoint;
            }
            set
            {
                this.localEndPoint = value;
            }
        }
        /// <summary>
        /// Gets or sets the name of the current <see cref="ServerProfile"/>,
        /// to allow an administrator to easily distinguish different profiles.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
        /// <summary>
        /// Gets or sets the maximum number of bytes per second that the
        /// <see cref="Server"/> is permitted to receive.
        /// </summary>
        public int MaxReceiveRateTotal
        {
            get
            {
                return this.maxReceiveRateTotal;
            }
            set
            {
                this.maxReceiveRateTotal = value;
            }
        }
        /// <summary>
        /// Gets or sets the maximum number of bytes per second that the
        /// <see cref="Server"/> is permitted to send.
        /// </summary>
        public int MaxSendRateTotal
        {
            get
            {
                return this.maxSendRateTotal;
            }
            set
            {
                this.maxSendRateTotal = value;
            }
        }
        /// <summary>
        /// Gets or sets a list of file paths which identify modules that the
        /// <see cref="Server"/> should load upon initialization.
        /// </summary>
        public string[] Modules
        {
            get
            {
                return this.modules;
            }
            set
            {
                this.modules = value;
            }
        }
        #endregion
    }
}
