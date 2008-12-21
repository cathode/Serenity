using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity
{
    public enum EventKind
    {
        /// <summary>
        /// Indicates that the message is only useful for debugging purposes,
        /// the message is only useful to developers.
        /// </summary>
        Debug = 0,
        /// <summary>
        /// Indicates that the message contains informational content about
        /// something that has taken place.
        /// </summary>
        Info = 1,
        /// <summary>
        /// Indicates that the message might be related to an issue with the
        /// current behaviour of the server.
        /// </summary>
        Notice = 2,
        /// <summary>
        /// Indicates that the message is informing the reader about unstable
        /// or unsafe behaviour or configuration of the server.
        /// </summary>
        Warning = 3,
        /// <summary>
        /// Indicates that the message describes a critical problem that has taken place.
        /// </summary>
        Error = 4,
    }
}
