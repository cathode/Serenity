using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Serenity.Web
{
    public sealed class Request
    {
        #region Constructors - Public
        public Request()
        {
            this.contentEncoding = null;
            this.contentLength = 0;
            this.contentType = MimeType.Default;
            this.cookies = new CookieCollection();
            this.hasEntityBody = false;
            this.headers = new HeaderCollection();
            this.isAuthenticated = false;
            this.isLocal = false;
            this.isSecureConnection = false;
            this.keepAlive = false;
            this.localEndPoint = null;
            this.method = RequestMethod.GET;
            this.protocolType = null;
            this.protocolVersion = null;
            this.rawMethod = null;
            this.rawRequest = null;
            this.rawUrl = null;
            this.referrer = null;
            this.remoteEndPoint = null;
            this.requestData = new RequestDataCollection();
            this.url = null;
            this.userAgent = null;
            this.userHostName = null;
        }
        #endregion
        #region Fields - Private
        private Socket connection;
        private Encoding contentEncoding;
        private int contentLength;
        private MimeType contentType;
        private CookieCollection cookies;
        [ThreadStatic]
        private static Request current;
        private bool hasEntityBody;
        private HeaderCollection headers;
        private bool isAuthenticated;
        private bool isLocal;
        private bool isSecureConnection;
        private bool keepAlive;
        private IPEndPoint localEndPoint;
        private RequestMethod method;
        private RequestDataCollection requestData;
        private string rawMethod;
        private string rawRequest;
        private string rawUrl;
        private IPEndPoint remoteEndPoint;
        private Uri url;
        private Uri referrer;
        private string userAgent;
        private string userHostName;
        private string protocolType;
        private Version protocolVersion;
      
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets or sets the <see cref="Socket"/> used to communicate the current <see cref="Request"/>.
        /// </summary>
        public Socket Connection
        {
            get
            {
                return this.connection;
            }
            set
            {
                this.connection = value;
            }
        }
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
        }
        public static Request Current
        {
            get
            {
                return Request.current;
            }
            internal set
            {
                Request.current = value;
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
        }
        public bool IsAuthenticated
        {
            get
            {
                return this.isAuthenticated;
            }
            set
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
            set
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
            set
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
            set
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
            set
            {
                this.localEndPoint = value;
            }
        }
        /// <summary>
        /// Gets or sets the supported method of the current <see cref="Request"/>.
        /// </summary>
        public RequestMethod Method
        {
            get
            {
                return this.method;
            }
            set
            {
                this.method = value;
            }
        }
        /// <summary>
        /// Gets or sets the actual string representing the method of the current <see cref="Request"/>.
        /// </summary>
        public string RawMethod
        {
            get
            {
                return this.rawMethod;
            }
            set
            {
                this.rawMethod = value;
            }
        }
        public string RawRequest
        {
            get
            {
                return this.rawRequest;
            }
            set
            {
                this.rawRequest = value;
            }
        }
        public string RawUrl
        {
            get
            {
                return this.rawUrl;
            }
            set
            {
                this.rawUrl = value;
            }
        }
        public Uri Referrer
        {
            get
            {
                return this.referrer;
            }
            set
            {
                this.referrer = value;
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
                return this.requestData;
            }
        }
        public Uri Url
        {
            get
            {
                return this.url;
            }
            set
            {
                this.url = value;
            }
        }
        public string UserAgent
        {
            get
            {
                return this.userAgent;
            }
            set
            {
                this.userAgent = value;
            }
        }
        public string UserHostName
        {
            get
            {
                return this.userHostName;
            }
            internal set
            {
                this.userHostName = value;
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
            set
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
            set
            {
                this.protocolVersion = value;
            }
        }
        #endregion
    }
}
