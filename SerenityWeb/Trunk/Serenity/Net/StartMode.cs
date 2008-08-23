using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Net
{
    /// <summary>
    /// Represents ways that a <see cref="ProtocolDriver2"/> can be started.
    /// </summary>
    public enum StartMode
    {
        /// <summary>
        /// Represents normal blocking mode.
        /// </summary>
        Normal,
        /// <summary>
        /// Represents normal blocking mode, but a new thread is used to start the <see cref="ProtocolDriver2"/> on.
        /// </summary>
        Threaded,
        /// <summary>
        /// Represents asynchronous mode.
        /// </summary>
        Async,
        /// <summary>
        /// Represents asynchronous mode, but the 
        /// </summary>
        BlockingAsync,
    }
}
