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

namespace Serenity.Web
{
    /// <summary>
    /// Represents the capabilities of a CommonRequest or CommonResponse.
    /// </summary>
    public class CommonCapabilities
    {
        #region Constructors - Internal
        /// <summary>
        /// Initializes a new instance of the CommonCapabilities class.
        /// </summary>
        internal CommonCapabilities()
        {
        }
        #endregion
        #region Fields - Private
        private bool supportsAuthentication;
        private bool supportsChunkedTransfer;
        private bool supportsContentControl;
        private bool supportsFields;
        private bool supportsHeaders;
        private bool supportsPeerInfo;
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets a value which indicates if secure authentication is supported.
        /// </summary>
        public bool SupportsAuthentication
        {
            get
            {
                return this.supportsAuthentication;
            }
            set
            {
                this.supportsAuthentication = value;
            }
        }
        /// <summary>
        /// Gets a boolean value which indicates if the transmission can be sent
        /// in pieces smaller than the entire transmission.
        /// </summary>
        public bool SupportsChunkedTransfer
        {
            get
            {
                return this.supportsChunkedTransfer;
            }
            set
            {
                this.supportsChunkedTransfer = value;
            }
        }
        /// <summary>
        /// Gets a value which indicates if the sent or recieved content can be controlled,
        /// e.g. the encoding, mimetype, etc.
        /// </summary>
        public bool SupportsContentControl
        {
            get
            {
                return this.supportsContentControl;
            }
            set
            {
                this.supportsContentControl = value;
            }
        }
        /// <summary>
        /// Gets a value which indicates if arbitrary named fields are supported.
        /// </summary>
        public bool SupportsFields
        {
            get
            {
                return this.supportsFields;
            }
            internal set
            {
                this.supportsFields = value;
            }
        }
        /// <summary>
        /// Gets a boolean value which indicates if arbitrary sets of headers are supported.
        /// </summary>
        public bool SupportsHeaders
        {
            get
            {
                return this.supportsHeaders;
            }
            set
            {
                this.supportsHeaders = value;
            }
        }
        /// <summary>
        /// Gets a value which indicates if obtaining information about the peer is supported.
        /// </summary>
        public bool SupportsPeerInfo
        {
            get
            {
                return this.supportsPeerInfo;
            }
            set
            {
                this.supportsPeerInfo = value;
            }
        }
        #endregion
    }
}