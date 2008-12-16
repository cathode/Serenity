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
    /// Represents a set of properties that influences the behavior of a <see cref="Server"/>.
    /// </summary>
    public struct ServerProfile
    {
        #region Constructors
        public ServerProfile(string name)
        {
            this.name = name;
            this.localEndPoint = new IPEndPoint(IPAddress.Any, 0);
        }
        public ServerProfile(ushort listeningPort)
        {
            this.name = string.Empty;
            this.localEndPoint = new IPEndPoint(IPAddress.Any, listeningPort);
        }
        #endregion
        #region Fields
        public static ServerProfile Default = new ServerProfile();
        private string name;
        private IPEndPoint localEndPoint;
        #endregion
        #region Properties
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
        #endregion
    }
}
