using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Web
{
    /// <summary>
    /// Represents supported request methods.
    /// </summary>
    public enum RequestMethod
    {
        /// <summary>
        /// Indicates an unknown, invalid, or otherwise not supported request method.
        /// </summary>
        Unknown,
        /// <summary>
        /// The GET method means retrieve whatever information (in the form of an entity) is identified by the Request-URI.
        /// </summary>
        GET,
        /// <summary>
        /// The POST method is used to request that the origin server accept the entity enclosed in the request as a new
        /// subordinate of the resource identified by the Request-URI in the Request-Line.
        /// </summary>
        POST,
        /// <summary>
        /// The HEAD method is identical to GET except that the server MUST NOT return a message-body in the response.
        /// </summary>
        HEAD,
        PUT,
        DELETE,
        TRACE,
        /// <summary>
        /// The OPTIONS method represents a request for information about the communication options available on the
        /// request/response chain identified by the Request-URI.
        /// </summary>
        OPTIONS,
        CONNECT,
        PROPFIND,
        PROPPATCH,
        MKCOL,
        COPY,
        MOVE,
        LOCK,
        UNLOCK,
    }
}
