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
using System.Net.Sockets;
using System.Text;

namespace Serenity.Web.Drivers
{
    internal class WebAdapterState
    {
        internal Socket WorkSocket;
        internal const int BufferSize = 256;
        internal byte[] Buffer = new byte[BufferSize];
    }
}
