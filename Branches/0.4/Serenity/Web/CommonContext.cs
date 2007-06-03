/*
Serenity - The next evolution of web server technology

Copyright � 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.Text;

using Serenity.Web.Drivers;

namespace Serenity.Web
{
    /// <summary>
    /// Encapsulates a CommonRequest/CommonResponse pair.
    /// </summary>
    public sealed class CommonContext
    {
        #region Constructors - Internal
        internal CommonContext(WebDriver origin)
        {
            this.request = new CommonRequest(this);
            this.response = new CommonResponse(this);
            this.driver = origin;
        }
        #endregion
        #region Fields - Private
        private string protocolType;
        private Version protocolVersion;
        private CommonRequest request;
        private CommonResponse response;
        private WebDriver driver;
        private bool supportsAuthentication;
        private bool supportsChunkedTransfer;
        private bool supportsContentControl;
        private bool supportsFields;
        private bool supportsHeaders;
        private bool supportsPeerInfo;
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets the WebDriver from which the current CommonContext originated from.
        /// </summary>
        public WebDriver Driver
        {
            get
            {
                return this.driver;
            }
        }
        /// <summary>
        /// Gets a string describing the protocol type that handled the current CommonContext.
        /// </summary>
        public string ProtocolType
        {
            get
            {
                return this.protocolType;
            }
            internal set
            {
                this.protocolType = value;
            }
        }
        /// <summary>
        /// Gets the Version of the protocol type that handled the current CommonContext.
        /// </summary>
        public Version ProtocolVersion
        {
            get
            {
                return this.protocolVersion;
            }
            internal set
            {
                this.protocolVersion = value;
            }
        }
        /// <summary>
        /// Gets the underlying CommonRequest instance for the current CommonContext.
        /// </summary>
        public CommonRequest Request
        {
            get
            {
                return this.request;
            }
        }
        /// <summary>
        /// Gets the underlying CommonResponse instance for the current CommonContext.
        /// </summary>
        public CommonResponse Response
        {
            get
            {
                return this.response;
            }
        }
        /// <summary>
        /// Gets a boolean value which indicates if authentication is supported.
        /// </summary>
        public bool SupportsAuthentication
        {
            get
            {
                return this.supportsAuthentication;
            }
            internal set
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
