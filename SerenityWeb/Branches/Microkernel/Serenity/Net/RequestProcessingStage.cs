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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Net
{
    public enum RequestProcessingStage
    {
        None = 0x0,
        MethodProcessed = 0x1,
        UriProcessed = 0x2,
        VersionProcessed = 0x3,
        RequestLineProcessed = 0x4,
        HeaderProcessed = 0x5,
        AllHeadersProcessed = 0x6,
        ProcessingComplete = 0x7,
    }
}
