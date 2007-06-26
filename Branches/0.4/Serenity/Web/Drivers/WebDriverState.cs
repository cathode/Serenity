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
using System.Text;

namespace Serenity.Web.Drivers
{
    /// <summary>
    /// Represents the state of a WebDriver's operation.
    /// </summary>
    public enum WebDriverState
    {
        /// <summary>
        /// Indicates that the WebDriver is newly created and has not carried out any action so far.
        /// </summary>
        None = 0,
        /// <summary>
        /// Indicates that the WebDriver has been initialized with a set of parameters which define it's behaviour.
        /// </summary>
        Initialized = 1,
        /// <summary>
        /// Indicates that the WebDriver is in a stopped state.
        /// It is not listening for connections and all pending requests have been responded to.
        /// </summary>
        Stopped = 2,
        /// <summary>
        /// Indicates that the WebDriver is ready to begin Running. It may already be listening,
        /// but it has not recieved any incoming connections yet.
        /// </summary>
        Started = 3,
    }
}
