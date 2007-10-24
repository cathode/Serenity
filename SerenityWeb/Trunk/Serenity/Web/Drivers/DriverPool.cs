/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Web.Drivers
{
    /// <summary>
    /// Manages multiple drivers and provides unified access to incoming
    /// CommonContexts.
    /// </summary>
    public sealed class DriverPool
    {
        public DriverPool()
        {

        }
        private List<WebDriver> drivers = new List<WebDriver>();

    }
}