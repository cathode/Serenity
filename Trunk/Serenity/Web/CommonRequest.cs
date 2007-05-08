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
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

using Serenity.Web.Drivers;

namespace Serenity.Web
{
    public sealed class CommonRequest
    {
        internal CommonRequest(CommonContext Owner)
        {
            this._RequestTraceIdentifier = Guid.NewGuid();
            this._Owner = Owner;
            this._Capabilities = new CommonCapabilities();
        }
        private CommonCapabilities _Capabilities;
        /// <summary>
        /// Gets or sets a CommonCapabilities object describing what the current CommonRequest is able to do.
        /// </summary>
        public CommonCapabilities Capabilities
        {
            get
            {
                return this._Capabilities;
            }
            set
            {
                this._Capabilities = value;
            }
        }
        public int ClientCertificateError
        {
            get
            {
                return this._ClientCertificateError;
            }
            internal set
            {
                this._ClientCertificateError = value;
            }
        }
        public Encoding ContentEncoding
        {
            get
            {
                return this._ContentEncoding;
            }
            set
            {
                this._ContentEncoding = value;
            }
        }
        public int ContentLength
        {
            get
            {
                return this._ContentLength;
            }
            set
            {
                this._ContentLength = value;
            }
        }
        public string ContentType
        {
            get
            {
                return this._ContentType;
            }
            set
            {
                this._ContentType = value;
            }
        }
        public CookieCollection Cookies
        {
            get
            {
                return this._Cookies;
            }
            internal set
            {
                this._Cookies = value;
            }
        }
        public bool HasEntityBody
        {
            get
            {
                return this._HasEntityBody;
            }
            set
            {
                this._HasEntityBody = value;
            }
        }
        public HeaderCollection Headers
        {
            get
            {
                return this._Headers;
            }
            internal set
            {
                this._Headers = value;
            }
        }
        public Stream InputStream
        {
            get
            {
                return this._InputStream;
            }
            internal set
            {
                this._InputStream = value;
            }
        }
        public bool IsAuthenticated
        {
            get
            {
                return this._IsAuthenticated;
            }
            internal set
            {
                this._IsAuthenticated = value;
            }
        }
        public bool IsLocal
        {
            get
            {
                return this._IsLocal;
            }
            internal set
            {
                this._IsLocal = value;
            }
        }
        public bool IsSecureConnection
        {
            get
            {
                return this._IsSecureConnection;
            }
            internal set
            {
                this._IsSecureConnection = value;
            }
        }
        public bool KeepAlive
        {
            get
            {
                return this._KeepAlive;
            }
            internal set
            {
                this._KeepAlive = value;
            }
        }
        public IPEndPoint LocalEndPoint
        {
            get
            {
                return this._LocalEndPoint;
            }
            internal set
            {
                this._LocalEndPoint = value;
            }
        }
        public string Method
        {
            get
            {
                return this._Method;
            }
            internal set
            {
                this._Method = value;
            }
        }

        public RequestDataCollection RequestData
        {
            get
            {
                return _RequestData;
            }
            internal set
            {
                _RequestData = value;
            }
        }
        public string RawUrl
        {
            get
            {
                return this._RawUrl;
            }
            internal set
            {
                this._RawUrl = value;
            }
        }
        public IPEndPoint RemoteEndPoint
        {
            get
            {
                return this._RemoteEndPoint;
            }
            internal set
            {
                this._RemoteEndPoint = value;
            }
        }
        public Guid RequestTraceIdentifier
        {
            get
            {
                return this._RequestTraceIdentifier;
            }
        }
        public Uri Url
        {
            get
            {
                return _Url;
            }
            set
            {
                _Url = value;
            }
        }
        public Uri Referrer
        {
            get
            {
                return _Referrer;
            }
            set
            {
                _Referrer = value;
            }
        }
        public string UserAgent
        {
            get
            {
                return _UserAgent;
            }
            set
            {
                _UserAgent = value;
            }
        }
        public string UserHostName
        {
            get
            {
                return _UserHostName;
            }
            internal set
            {
                _UserHostName = value;
            }
        }
        public string[] UserLanguages
        {
            get
            {
                return _UserLanguages;
            }
            set
            {
                _UserLanguages = value;
            }
        }
        public string UserPrimaryLanguage
        {
            get
            {
                return _UserPrimaryLanguage;
            }
            set
            {
                _UserPrimaryLanguage = value;
            }
        }
        private int _ClientCertificateError;
        private Encoding _ContentEncoding;
        private int _ContentLength;
        private string _ContentType;
        private CookieCollection _Cookies;
        private bool _HasEntityBody;
        private HeaderCollection _Headers = new HeaderCollection();
        private Stream _InputStream;
        private bool _IsAuthenticated;
        private bool _IsLocal;
        private bool _IsSecureConnection;
        private bool _KeepAlive;
        private IPEndPoint _LocalEndPoint;
        private string _Method;
        private RequestDataCollection _RequestData = new RequestDataCollection();
        private string _RawUrl;
        private IPEndPoint _RemoteEndPoint;
        private Guid _RequestTraceIdentifier;
        private Uri _Url;
        private Uri _Referrer;
        private string _UserAgent;
        private string _UserHostName;
        private string[] _UserLanguages;
        private string _UserPrimaryLanguage;
        /// <summary>
        /// Gets the WebDriver from which the current CommonContext originated from.
        /// </summary>
        public WebDriver Origin
        {
            get
            {
                return this._Owner.Origin;
            }
        }
        public CommonContext Owner
        {
            get
            {
                return this._Owner;
            }
        }
        private CommonContext _Owner;
    }
}
