/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2015 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/

namespace Serenity.Net
{
    /// <summary>
    /// Represents steps in the process of parsing HTTP request data.
    /// </summary>
    public enum HttpRequestParseStep
    {
        Method = 0x0,
        Uri = 0x1,
        Version = 0x2,
        HeaderName = 0x3,
        HeaderValue = 0x4,
        Content = 0x5,
        CreateResponse = 0x6,
    }
}
