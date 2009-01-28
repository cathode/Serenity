/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *----------------------------------------------------------------------------*
 * Authors:                                                                   *
 * - Will 'AnarkiNet' Shelley (AnarkiNet@gmail.com): Original Author          *
 *****************************************************************************/

namespace Serenity.Net
{
    /// <summary>
    /// Represents steps in the process of parsing HTTP request data.
    /// </summary>
    public enum RequestStep
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
