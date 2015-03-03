/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2015 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/

namespace Serenity
{
    /// <summary>
    /// Represents a code returned to a client along with a web response,
    /// which indicates the state of the response.
    /// </summary>
    public struct StatusCode
    {
        #region Fields
        /// <summary>
        /// Backing field for the <see cref="StatusCode.Code"/> property.
        /// </summary>
        private readonly int code;

        /// <summary>
        /// Backing field for the <see cref="StatusCode.Message"/> property.
        /// </summary>
        private readonly string message;
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusCode"/> struct.
        /// </summary>
        /// <param name="code">The code number of the new <see cref="StatusCode"/>.</param>
        /// <param name="message">The status message of the new <see cref="StatusCode"/>.</param>
        private StatusCode(int code, string message)
        {
            this.code = code;
            this.message = message;
        }
        #endregion
        #region Properties

        /// <summary>
        /// Gets a status code indicating an HTTP 100 Continue response.
        /// </summary>
        public static StatusCode Http100Continue
        {
            get
            {
                return new StatusCode(100, "Continue");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 101 Switching Protocols response.
        /// </summary>
        public static StatusCode Http101SwitchingProtocols
        {
            get
            {
                return new StatusCode(101, "Switching Protocols");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 200 Ok response.
        /// This is the most common status code.
        /// </summary>
        public static StatusCode Http200Ok
        {
            get
            {
                return new StatusCode(200, "OK");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 201 Created response.
        /// This means that the client's request caused the requested URI to be created on the server.
        /// </summary>
        public static StatusCode Http201Created
        {
            get
            {
                return new StatusCode(201, "Created");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 202 Accepted response.
        /// </summary>
        public static StatusCode Http202Accepted
        {
            get
            {
                return new StatusCode(202, "Accepted");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 203 Non-Authoritative Information response.
        /// </summary>
        public static StatusCode Http203NonAuthoritativeInformation
        {
            get
            {
                return new StatusCode(203, "Non-Authoritative Information");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 204 No Content response.
        /// </summary>
        public static StatusCode Http204NoContent
        {
            get
            {
                return new StatusCode(204, "No Content");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 205 Reset Content response.
        /// </summary>
        public static StatusCode Http205ResetContent
        {
            get
            {
                return new StatusCode(205, "Reset Content");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 206 Partial Content response.
        /// </summary>
        public static StatusCode Http206PartialContent
        {
            get
            {
                return new StatusCode(206, "Partial Content");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 300 Multiple Choices response.
        /// </summary>
        public static StatusCode Http300MultipleChoices
        {
            get
            {
                return new StatusCode(300, "Multiple Choices");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 301 Moved Permenently response.
        /// This is used when the requested URI is located elsewhere, permenently.
        /// </summary>
        public static StatusCode Http301MovedPermenently
        {
            get
            {
                return new StatusCode(301, "Moved Permenently");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 302 Found response.
        /// </summary>
        public static StatusCode Http302Found
        {
            get
            {
                return new StatusCode(302, "Found");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 303 See Other response.
        /// </summary>
        public static StatusCode Http303SeeOther
        {
            get
            {
                return new StatusCode(303, "See Other");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 304 Not Modified response.
        /// </summary>
        public static StatusCode Http304NotModified
        {
            get
            {
                return new StatusCode(304, "Not Modified");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 305 Use Proxy response.
        /// </summary>
        public static StatusCode Http305UseProxy
        {
            get
            {
                return new StatusCode(305, "Use Proxy");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 306 Switch Proxy response.
        /// </summary>
        public static StatusCode Http306SwitchProxy
        {
            get
            {
                return new StatusCode(306, "Switch Proxy");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 307 Temporary Redirect response.
        /// </summary>
        public static StatusCode Http307TemporaryRedirect
        {
            get
            {
                return new StatusCode(307, "Temporary Redirect");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 400 Bad Request response.
        /// </summary>
        public static StatusCode Http400BadRequest
        {
            get
            {
                return new StatusCode(400, "Bad Request");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 401 Unauthorized response.
        /// </summary>
        public static StatusCode Http401Unauthorized
        {
            get
            {
                return new StatusCode(401, "Unauthorized");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 402 Payment Required response.
        /// This status code is not used.
        /// </summary>
        public static StatusCode Http402PaymentRequired
        {
            get
            {
                return new StatusCode(402, "Payment Required");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 403 Forbidden response.
        /// </summary>
        public static StatusCode Http403Forbidden
        {
            get
            {
                return new StatusCode(403, "Forbidden");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 404 Not Found response.
        /// This is used when the requested URI does not exist on the server.
        /// </summary>
        public static StatusCode Http404NotFound
        {
            get
            {
                return new StatusCode(404, "Not Found");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 405 Method Not Allowed response.
        /// </summary>
        public static StatusCode Http405MethodNotAllowed
        {
            get
            {
                return new StatusCode(405, "Method Not Allowed");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 406 Not Acceptable response.
        /// </summary>
        public static StatusCode Http406NotAcceptable
        {
            get
            {
                return new StatusCode(406, "Not Acceptable");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 407 Proxy Authentication Required response.
        /// </summary>
        public static StatusCode Http407ProxyAuthenticationRequired
        {
            get
            {
                return new StatusCode(407, "Proxy Authentication Required");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 408 Request Timeout response.
        /// </summary>
        public static StatusCode Http408RequestTimeout
        {
            get
            {
                return new StatusCode(408, "Request Timeout");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 409 Conflict response.
        /// </summary>
        public static StatusCode Http409Conflict
        {
            get
            {
                return new StatusCode(409, "Conflict");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 410 Gone response.
        /// This is used when a resource on the server has been intentionally removed.
        /// </summary>
        public static StatusCode Http410Gone
        {
            get
            {
                return new StatusCode(410, "Gone");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 411 Length Required response.
        /// </summary>
        public static StatusCode Http411LengthRequired
        {
            get
            {
                return new StatusCode(411, "Length Required");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 412 Precondition Failed response.
        /// </summary>
        public static StatusCode Http412PreconditionFailed
        {
            get
            {
                return new StatusCode(412, "Precondition Failed");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 413 Request Entity Too Large response.
        /// </summary>
        public static StatusCode Http413RequestEntityTooLarge
        {
            get
            {
                return new StatusCode(413, "Request Entity Too Large");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 414 Request-URI Too Long response.
        /// </summary>
        public static StatusCode Http414RequestUriTooLong
        {
            get
            {
                return new StatusCode(414, "Request-URI Too long");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 415 Unsupported Media Type response.
        /// </summary>
        public static StatusCode Http415UnsupportedMediaType
        {
            get
            {
                return new StatusCode(415, "Unsupported Media Type");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 416 Requested Range Not Satisfiable response.
        /// </summary>
        public static StatusCode Http416RequestedRangeNotSatisfiable
        {
            get
            {
                return new StatusCode(416, "Requested Range Not Satisfiable");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 417 Expectation Failed response.
        /// </summary>
        public static StatusCode Http417ExpectationFailed
        {
            get
            {
                return new StatusCode(417, "Expectation Failed");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 426 Upgrade Required response.
        /// </summary>
        public static StatusCode Http426UpgradeRequired
        {
            get
            {
                return new StatusCode(426, "Upgrade Required");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 500 Internal Server Error response.
        /// </summary>
        public static StatusCode Http500InternalServerError
        {
            get
            {
                return new StatusCode(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 501 Not Implemented response.
        /// </summary>
        public static StatusCode Http501NotImplemented
        {
            get
            {
                return new StatusCode(501, "Not Implemented");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 502 Bad Gateway response.
        /// </summary>
        public static StatusCode Http502BadGateway
        {
            get
            {
                return new StatusCode(502, "Bad Gateway");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 503 Service Unavailable response.
        /// </summary>
        public static StatusCode Http503ServiceUnavailable
        {
            get
            {
                return new StatusCode(503, "Service Unavailable");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 504 Gateway Timeout response.
        /// </summary>
        public static StatusCode Http504GatewayTimeout
        {
            get
            {
                return new StatusCode(504, "Gateway Timeout");
            }
        }

        /// <summary>
        /// Gets a status code indicating an HTTP 505 HTTP Version Not Supported response.
        /// </summary>
        public static StatusCode Http505HttpVersionNotSupported
        {
            get
            {
                return new StatusCode(505, "HTTP Version Not Supported");
            }
        }

        /// <summary>
        /// Gets a status code indicating a WebDAV 102 Processing response.
        /// </summary>
        public static StatusCode WebDav102Processing
        {
            get
            {
                return new StatusCode(102, "Processing");
            }
        }

        /// <summary>
        /// Gets a status code indicating a WebDAV 207 Multi-status response.
        /// </summary>
        public static StatusCode WebDav207MultiStatus
        {
            get
            {
                return new StatusCode(207, "Multi-Status");
            }
        }

        /// <summary>
        /// Gets a status code indicating a WebDAV 422 Unprocessable Entity response.
        /// </summary>
        public static StatusCode WebDav422UnprocessableEntity
        {
            get
            {
                return new StatusCode(422, "Unprocessable Entity");
            }
        }

        /// <summary>
        /// Gets a status code indicating a WebDAV 423 Locked response.
        /// </summary>
        public static StatusCode WebDav423Locked
        {
            get
            {
                return new StatusCode(423, "Locked");
            }
        }

        /// <summary>
        /// Gets a status code indicating a WebDAV 424 Failed Dependency response.
        /// </summary>
        public static StatusCode WebDav424FailedDependency
        {
            get
            {
                return new StatusCode(424, "Failed Dependency");
            }
        }

        /// <summary>
        /// Gets a status code indicating a WebDAV 425 Unordered Collection response.
        /// </summary>
        public static StatusCode WebDav425UnorderedCollection
        {
            get
            {
                return new StatusCode(425, "Unordered Collection");
            }
        }

        /// <summary>
        /// Gets a status code indicating a WebDAV 507 Insufficient Storage response.
        /// </summary>
        public static StatusCode WebDav507InsufficientStorage
        {
            get
            {
                return new StatusCode(507, "Insufficient Storage");
            }
        }

        /// <summary>
        /// Gets the numeric portion of the current ResponseCode.
        /// </summary>
        public int Code
        {
            get
            {
                return this.code;
            }
        }

        /// <summary>
        /// Gets the description or command word associated with the current ResponseCode.
        /// </summary>
        public string Message
        {
            get
            {
                return this.message;
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Gets a unique hashcode for the current instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode() ^ 0x5d1f609c;
        }

        /// <summary>
        /// Converts the current <see cref="StatusCode"/> to it's string representation.
        /// </summary>
        /// <returns>The string representation of the current <see cref="StatusCode"/></returns>
        public override string ToString()
        {
            return this.Code.ToString() + " " + this.Message;
        }
        #endregion
    }
}