/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

using Serenity.Web.Drivers;

namespace Serenity.Web
{
    /// <summary>
    /// Represents a request for a specific resource on a server.
    /// </summary>
    public sealed class CommonRequest
    {
        #region Constructors - Internal
        internal CommonRequest(CommonContext Owner)
        {
            this.context = Owner;
        }
        #endregion
        #region Fields - Private
        private Encoding contentEncoding;
        private int contentLength;
        private MimeType contentType;
        private CommonContext context;
        private CookieCollection cookies;
        private bool hasEntityBody;
        private HeaderCollection headers = new HeaderCollection();
        private Stream inputStream;
        private bool isAuthenticated;
        private bool isLocal;
        private bool isSecureConnection;
        private bool keepAlive;
        private IPEndPoint localEndPoint;
        private RequestMethod method;
        private RequestDataCollection requestData = new RequestDataCollection();
        private string rawUrl;
        private IPEndPoint remoteEndPoint;
        private Uri url = new Uri("http://localhost/");
        private Uri referrer = new Uri("http://localhost/");
        private string userAgent;
        private string userHostName;
        private string[] userLanguages;
        private string userPrimaryLanguage;
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets or sets the content encoding used for content sent with the
        /// current request.
        /// </summary>
        public Encoding ContentEncoding
        {
            get
            {
                return this.contentEncoding;
            }
            set
            {
                this.contentEncoding = value;
            }
        }
        /// <summary>
        /// Gets or sets the length of content sent with the current request.
        /// </summary>
        public int ContentLength
        {
            get
            {
                return this.contentLength;
            }
            set
            {
                this.contentLength = value;
            }
        }
        /// <summary>
        /// Gets or sets the mime type of the content sent with the current
        /// request.
        /// </summary>
        public MimeType ContentType
        {
            get
            {
                return this.contentType;
            }
            set
            {
                this.contentType = value;
            }
        }
        /// <summary>
        /// Gets a collection of cookies sent with the current request.
        /// </summary>
        public CookieCollection Cookies
        {
            get
            {
                return this.cookies;
            }
            internal set
            {
                this.cookies = value;
            }
        }
        /// <summary>
        /// Gets or sets an indication of whether or not the current request
        /// was sent containing any entity body.
        /// </summary>
        public bool HasEntityBody
        {
            get
            {
                return this.hasEntityBody;
            }
            set
            {
                this.hasEntityBody = value;
            }
        }
        public HeaderCollection Headers
        {
            get
            {
                return this.headers;
            }
            internal set
            {
                this.headers = value;
            }
        }
        public Stream InputStream
        {
            get
            {
                return this.inputStream;
            }
            internal set
            {
                this.inputStream = value;
            }
        }
        public bool IsAuthenticated
        {
            get
            {
                return this.isAuthenticated;
            }
            internal set
            {
                this.isAuthenticated = value;
            }
        }
        public bool IsLocal
        {
            get
            {
                return this.isLocal;
            }
            internal set
            {
                this.isLocal = value;
            }
        }
        public bool IsSecureConnection
        {
            get
            {
                return this.isSecureConnection;
            }
            internal set
            {
                this.isSecureConnection = value;
            }
        }
        public bool KeepAlive
        {
            get
            {
                return this.keepAlive;
            }
            internal set
            {
                this.keepAlive = value;
            }
        }
        public IPEndPoint LocalEndPoint
        {
            get
            {
                return this.localEndPoint;
            }
            internal set
            {
                this.localEndPoint = value;
            }
        }
        public RequestMethod Method
        {
            get
            {
                return this.method;
            }
            internal set
            {
                this.method = value;
            }
        }
        public string RawUrl
        {
            get
            {
                return this.rawUrl;
            }
            internal set
            {
                this.rawUrl = value;
            }
        }
        public Uri Referrer
        {
            get
            {
                return referrer;
            }
            set
            {
                referrer = value;
            }
        }
        public IPEndPoint RemoteEndPoint
        {
            get
            {
                return this.remoteEndPoint;
            }
            internal set
            {
                this.remoteEndPoint = value;
            }
        }
        public RequestDataCollection RequestData
        {
            get
            {
                return requestData;
            }
            internal set
            {
                requestData = value;
            }
        }
        public Uri Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
            }
        }

        public string UserAgent
        {
            get
            {
                return userAgent;
            }
            set
            {
                userAgent = value;
            }
        }
        public string UserHostName
        {
            get
            {
                return userHostName;
            }
            internal set
            {
                userHostName = value;
            }
        }
        public string[] UserLanguages
        {
            get
            {
                return userLanguages;
            }
            set
            {
                userLanguages = value;
            }
        }
        public string UserPrimaryLanguage
        {
            get
            {
                return userPrimaryLanguage;
            }
            set
            {
                userPrimaryLanguage = value;
            }
        }
        #endregion
    }
}
