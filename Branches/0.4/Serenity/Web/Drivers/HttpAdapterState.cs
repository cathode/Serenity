using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Serenity.Web.Drivers
{
    internal class HttpAdapterState
    {
        internal Socket WorkSocket;
        internal const int BufferSize = 256;
        internal byte[] Buffer = new byte[BufferSize];
    }
}
