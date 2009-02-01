/* Serenity - The next evolution of web server technology.
 * Copyright © 2006-2009 Serenity Project - http://SerenityProject.net/
 * 
 * This software is released under the terms and conditions of the Microsoft Public License (MS-PL),
 * a copy of which should have been included with this distribution as License.txt.
 */
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;


namespace Serenity.Net
{
    /// <summary>
    /// Represents a set of properties that controls the behavior of a
    /// <see cref="Server"/>.
    /// </summary>
    /// <remarks>
    /// Altering values of a <see cref="ServerProfile"/> after it has been
    /// assigned to a <see cref="Server"/> while that server is running may
    /// produce undesired results. At the very least, some options may not
    /// take effect until the server is stopped and restarted.
    /// </remarks>
    public class ServerProfile
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerProfile"/>
        /// class with default values.
        /// </summary>
        public ServerProfile()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerProfile"/>
        /// class with a specified name.
        /// </summary>
        /// <param name="name"></param>
        public ServerProfile(string name)
        {
            this.name = name;
        }
        #endregion
        #region Fields
        private static ServerProfile defaultProfile = new ServerProfile();
        private string name = null;
        private IPEndPoint localEndPoint = new IPEndPoint(IPAddress.IPv6Any, 0);
        private int connectionBacklog = 20;
        private int maxReceiveRateTotal;
        private int maxSendRateTotal;
        private string[] modules = new string[0];
        private string path;
        private bool useIPv6;
        private bool useCaseSensitiveUris;
        private int receiveTimeout = 30000; //30 second default timeout.
        #endregion
        #region Methods
        /// <summary>
        /// Loads profile values defined in an XML file located at the 
        /// specified path into the current <see cref="ServerProfile"/>.
        /// </summary>
        /// <param name="path">The location of the XML file to load from.
        /// </param>
        /// <remarks>
        /// This method is provided as an instance method to enable multiple
        /// profiles to be effectively "merged", however for best results, use
        /// this method on a new <see cref="ServerProfile"/> instance.
        /// </remarks>
        public void LoadXmlProfile(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            else if (!File.Exists(path))
            {
                throw new FileNotFoundException("The specified file was not found.", path);
            }
            var settings = new XmlReaderSettings();
            settings.CheckCharacters = true;
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.IgnoreComments = true;

            using (XmlReader reader = XmlReader.Create(path, settings))
            {
                //TODO: Implement XML profile reading
            }

            //reader.Read();
            //reader.ReadStartElement("


        }
        /// <summary>
        /// Saves profile values defined in the current
        /// <see cref="ServerProfile"/> to an XML file located at the specified
        /// path.
        /// </summary>
        /// <param name="path"></param>
        /// <remarks>
        /// This will overwrite the target file. If the <see cref="Path"/>
        /// property has not been set on the current
        /// <see cref="ServerProfile"/>, then it will be set to
        /// <paramref name="path"/>.
        /// </remarks>
        public void SaveXmlProfile(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            using (XmlTextWriter writer = new XmlTextWriter(path, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 4;
                writer.IndentChar = ' ';

                writer.WriteStartDocument();
                writer.WriteStartElement("ServerProfile");
                if (this.Name != null)
                {
                    writer.WriteAttributeString("Name", this.Name);
                }
                writer.WriteElementString("ConnectionBacklog", this.ConnectionBacklog.ToString());
                writer.WriteElementString("MaxReceiveRateTotal", this.MaxReceiveRateTotal.ToString());
                writer.WriteElementString("MaxSendRateTotal", this.MaxSendRateTotal.ToString());
                writer.WriteElementString("LocalEndPoint", this.LocalEndPoint.ToString());

                foreach (string module in this.Modules)
                {
                    writer.WriteElementString("Module", module);
                }
                writer.WriteEndDocument();

                writer.Flush();
                writer.Close();
            }
        }
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
        /// <summary>
        /// Gets or sets a value that controls whether the <see cref="Server"/>
        /// attempts to listen for incoming Internet Protocol v6 connections.
        /// </summary>
        /// <remarks>
        /// If this option is enabled, the server's listening socket will try
        /// to use dual-mode operation, so IPv4 clients will be accepted as
        /// well. Dual-mode socket operation is only supported on Windows
        /// Vista and Windows Server 2008.
        /// </remarks>
        public bool UseIPv6
        {
            get
            {
                return this.useIPv6;
            }
            set
            {
                this.useIPv6 = value;
            }
        }
        /// <summary>
        /// Gets or sets the filename which the current
        /// <see cref="ServerProfile"/> was last loaded from or saved to, if
        /// applicable. 
        /// </summary>
        /// <remarks>If the profile has not been persisted to disk, there
        /// will be no path set (null).
        /// </remarks>
        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                this.path = value;
            }
        }
        public int ReceiveTimeout
        {
            get
            {
                return this.receiveTimeout;
            }
            set
            {
                this.receiveTimeout = value;
            }
        }
        public bool UseCaseSensitiveUris
        {
            get
            {
                return this.useCaseSensitiveUris;
            }
            set
            {
                this.useCaseSensitiveUris = value;
            }
        }
        #endregion
    }
}
