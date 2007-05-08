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
            this.origin = origin;
        }
        #endregion
        #region Fields - Private
        private bool quickClose = true;
        private string protocolType;
        private Version protocolVersion;
        private CommonRequest request;
        private CommonResponse response;
        private WebDriver origin;
        #endregion
        #region Properties - Public
        
        /// <summary>
        /// Gets or sets a boolean value which determines if the connection which transmits the current CommonContext
        /// should be terminated after the transmission finishes.
        /// </summary>
        public bool QuickClose
        {
            get
            {
                return this.quickClose;
            }
            set
            {
                this.quickClose = value;
            }
        }
        /// <summary>
        /// Gets the WebDriver from which the current CommonContext originated from.
        /// </summary>
        public WebDriver Origin
        {
            get
            {
                return this.origin;
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
        #endregion
    }
}
