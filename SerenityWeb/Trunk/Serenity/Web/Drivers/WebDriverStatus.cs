/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Web.Drivers
{
	/// <summary>
	/// Represents the status of a WebDriver's operation.
	/// </summary>
	public enum WebDriverStatus
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
