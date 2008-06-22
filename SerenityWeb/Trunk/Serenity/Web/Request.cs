using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Serenity.Web
{
    public static class Request
    {
        #region Fields - Private
        [ThreadStatic]
        private static Encoding contentEncoding;
        [ThreadStatic]
        private static int contentLength;
        [ThreadStatic]
        private static MimeType contentType;
        [ThreadStatic]
        private static CookieCollection cookies;
        [ThreadStatic]
        private static bool hasEntityBody;
        [ThreadStatic]
        private static HeaderCollection headers;
        [ThreadStatic]
        private static bool isAuthenticated;
        [ThreadStatic]
        private static bool isInitialized;
        [ThreadStatic]
        private static bool isLocal;
        [ThreadStatic]
        private static bool isSecureConnection;
        [ThreadStatic]
        private static bool keepAlive;
        [ThreadStatic]
        private static IPEndPoint localEndPoint;
        [ThreadStatic]
        private static RequestMethod method;
        [ThreadStatic]
        private static RequestDataCollection requestData;
        [ThreadStatic]
        private static string rawRequest;
        [ThreadStatic]
        private static string rawUrl;
        [ThreadStatic]
        private static IPEndPoint remoteEndPoint;
        [ThreadStatic]
        private static Uri url;
        [ThreadStatic]
        private static Uri referrer;
        [ThreadStatic]
        private static string userAgent;
        [ThreadStatic]
        private static string userHostName;
        [ThreadStatic]
        private static string protocolType;
        [ThreadStatic]
        private static Version protocolVersion;
        #endregion
        #region Methods - Public
        public static void Initialize()
        {
            if (!Request.isInitialized)
            {
                Request.contentEncoding = null;
                Request.contentLength = 0;
                Request.contentType = MimeType.Default;
                Request.cookies = new CookieCollection();
                Request.hasEntityBody = false;
                Request.headers = new HeaderCollection();
                Request.isAuthenticated = false;
                Request.isLocal = false;
                Request.isSecureConnection = false;
                Request.keepAlive = false;
                Request.localEndPoint = null;
                Request.method = RequestMethod.GET;
                Request.protocolType = null;
                Request.protocolVersion = null;
                Request.rawRequest = null;
                Request.rawUrl = null;
                Request.referrer = null;
                Request.remoteEndPoint = null;
                Request.requestData = new RequestDataCollection();
                Request.url = null;
                Request.userAgent = null;
                Request.userHostName = null;
                
                Request.isInitialized = true;
            }
        }
        public static void Reset()
        {
            Request.isInitialized = false;
            Request.Initialize();
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets or sets the content encoding used for content sent with the
        /// current request.
        /// </summary>
        public static Encoding ContentEncoding
        {
            get
            {
                return Request.contentEncoding;
            }
            set
            {
                Request.contentEncoding = value;
            }
        }
        /// <summary>
        /// Gets or sets the length of content sent with the current request.
        /// </summary>
        public static int ContentLength
        {
            get
            {
                return Request.contentLength;
            }
            set
            {
                Request.contentLength = value;
            }
        }
        /// <summary>
        /// Gets or sets the mime type of the content sent with the current
        /// request.
        /// </summary>
        public static MimeType ContentType
        {
            get
            {
                return Request.contentType;
            }
            set
            {
                Request.contentType = value;
            }
        }
        /// <summary>
        /// Gets a collection of cookies sent with the current request.
        /// </summary>
        public static CookieCollection Cookies
        {
            get
            {
                return Request.cookies;
            }
        }
        /// <summary>
        /// Gets or sets an indication of whether or not the current request
        /// was sent containing any entity body.
        /// </summary>
        public static bool HasEntityBody
        {
            get
            {
                return Request.hasEntityBody;
            }
            set
            {
                Request.hasEntityBody = value;
            }
        }
        public static HeaderCollection Headers
        {
            get
            {
                return Request.headers;
            }
        }
        public static bool IsAuthenticated
        {
            get
            {
                return Request.isAuthenticated;
            }
            set
            {
                Request.isAuthenticated = value;
            }
        }
        public static bool IsLocal
        {
            get
            {
                return Request.isLocal;
            }
            set
            {
                Request.isLocal = value;
            }
        }
        public static bool IsSecureConnection
        {
            get
            {
                return Request.isSecureConnection;
            }
            set
            {
                Request.isSecureConnection = value;
            }
        }
        public static bool KeepAlive
        {
            get
            {
                return Request.keepAlive;
            }
            set
            {
                Request.keepAlive = value;
            }
        }
        public static IPEndPoint LocalEndPoint
        {
            get
            {
                return Request.localEndPoint;
            }
            set
            {
                Request.localEndPoint = value;
            }
        }
        public static RequestMethod Method
        {
            get
            {
                return Request.method;
            }
            set
            {
                Request.method = value;
            }
        }
        public static string RawRequest
        {
            get
            {
                return Request.rawRequest;
            }
            set
            {
                Request.rawRequest = value;
            }
        }
        public static string RawUrl
        {
            get
            {
                return Request.rawUrl;
            }
            set
            {
                Request.rawUrl = value;
            }
        }

        public static Uri Referrer
        {
            get
            {
                return Request.referrer;
            }
            set
            {
                Request.referrer = value;
            }
        }
        public static IPEndPoint RemoteEndPoint
        {
            get
            {
                return Request.remoteEndPoint;
            }
            internal set
            {
                Request.remoteEndPoint = value;
            }
        }
        public static RequestDataCollection RequestData
        {
            get
            {
                return Request.requestData;
            }
        }
        public static Uri Url
        {
            get
            {
                return Request.url;
            }
            set
            {
                Request.url = value;
            }
        }
        public static string UserAgent
        {
            get
            {
                return Request.userAgent;
            }
            set
            {
                Request.userAgent = value;
            }
        }
        public static string UserHostName
        {
            get
            {
                return Request.userHostName;
            }
            internal set
            {
                Request.userHostName = value;
            }
        }
        /// <summary>
        /// Gets a string describing the protocol type that handled the current CommonContext.
        /// </summary>
        public static string ProtocolType
        {
            get
            {
                return Request.protocolType;
            }
            set
            {
                Request.protocolType = value;
            }
        }
        /// <summary>
        /// Gets the Version of the protocol type that handled the current CommonContext.
        /// </summary>
        public static Version ProtocolVersion
        {
            get
            {
                return Request.protocolVersion;
            }
            set
            {
                Request.protocolVersion = value;
            }
        }
        #endregion
    }
}
